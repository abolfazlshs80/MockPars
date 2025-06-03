using ErrorOr;
using Microsoft.Data.SqlClient;
using MockPars.Application.DTO.@base;
using MockPars.Application.DTO.Database;
using MockPars.Application.DTO.Table;
using MockPars.Application.Extention;
using MockPars.Application.Static.FakeValue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MockPars.Application.Static.Message;

namespace MockPars.Application.Services.Interfaces
{
    public interface ISqlProvider
    {
        Task<ErrorOr<IEnumerable<TableInfoDto>>> GetTablesAsync(ConnectionDatabaseDto model,
            CancellationToken cancellationToken);

        Task<object?> GetRandomForeignKeyValueAsync(SqlConnection connection, string foreignTable);
        Task<ErrorOr<List<ColumnInfoDto>>> GetTableColumnAsync(GetColumnByTableDto model);
        Task<ErrorOr<Dictionary<string, FakeDataTypesDto>>> AddFakeDataAsync(FakeDataToTableDto model);
        Task<ErrorOr<bool>> AddFakeDataAsync(FakeDataToCustomColumnsDto model);
        object GenerateFake(FakeDataTypesDto type);

    }


    public class SqlProvider : ISqlProvider
    {
        public async Task<ErrorOr<IEnumerable<TableInfoDto>>> GetTablesAsync(ConnectionDatabaseDto model, CancellationToken cancellationToken)
        {
            List<TableInfoDto> TableListBox = new();

            using (var connection = new SqlConnection(model.ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("select  * from INFORMATION_SCHEMA.TABLES ", connection);
                var reader = command.ExecuteReader();
                TableListBox.Clear();
                while (reader.Read())
                {
                    TableListBox.Add(new TableInfoDto() { Schema = reader["TABLE_SCHEMA"].ToString(), TableName = reader["TABLE_NAME"].ToString() });
                }

            }

            return TableListBox;
        }

        public async Task<ErrorOr<List<ColumnInfoDto>>> GetTableColumnAsync(GetColumnByTableDto model)
        {
            List<ColumnInfoDto> databaseModels = new List<ColumnInfoDto>();
            string firstSchema = model.Table.Schema;
            string firstTable = model.Table.TableName;
            using (var con = new SqlConnection(model.Database.ConnectionString))
            {
                con.Open();

                var CommandPrimaryKey = new SqlCommand("select  * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc" +
                    " inner join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu on ccu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME where tc.CONSTRAINT_TYPE = N'PRIMARY KEY' AND tc.TABLE_NAME = N'"
                    + firstTable + "' AND tc.TABLE_SCHEMA = N'" + firstSchema + "' ", con);
                var PrimaryReader = CommandPrimaryKey.ExecuteReader();
                List<string> CollectionPrimaryKeys = new List<string>();
                while (PrimaryReader.Read())
                {
                    CollectionPrimaryKeys.Add(PrimaryReader["COLUMN_NAME"].ToString());
                }

                var CommandComputed = new SqlCommand("select name from sys.columns where object_id= object_id( '" + firstSchema + "." + firstTable + "') And(is_computed =1 OR is_identity=1)", con);
                var ComputedReader = CommandComputed.ExecuteReader();
                List<string> CollectionComputedKeys = new List<string>();
                while (ComputedReader.Read())
                {
                    CollectionComputedKeys.Add(ComputedReader["name"].ToString());
                }

                //find foreign keys 
                var CommandForeignKey = new SqlCommand("SELECT \r\n " +
                                                       "   f.name AS ForeignKeyName,\r\n   " +
                                                       " OBJECT_NAME(f.parent_object_id) AS TableName,\r\n " +
                                                       "   COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,\r\n " +
                                                       "   OBJECT_NAME (f.referenced_object_id) AS ReferencedTableName\r\nFROM \r\n " +
                                                       "   sys.foreign_keys AS f\r\nINNER JOIN \r\n    sys.foreign_key_columns AS fc \r\n  " +
                                                       "  ON f.object_id = fc.constraint_object_id\r\nWHERE \r\n " +
                                                       $"   OBJECT_NAME(f.parent_object_id) = '{firstTable}'\r\n", con);
                var ForeignKeyReader = CommandForeignKey.ExecuteReader();
                List<string> CollectionForeignKeys = new List<string>();
                while (ForeignKeyReader.Read())
                {
                    CollectionForeignKeys.Add(ForeignKeyReader["TableName"] + "," + ForeignKeyReader["ReferencedTableName"] + "," + ForeignKeyReader["ColumnName"]);
                }

                var command = new SqlCommand("select  * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME =N'" + firstTable + "'  AND TABLE_SCHEMA =N'" + firstSchema + "'", con);
                var Columnreader = command.ExecuteReader();
                while (Columnreader.Read())
                {
                    ColumnInfoDto columnInfoModel = new ColumnInfoDto()
                    {
                        Name = Columnreader["COLUMN_NAME"].ToString(),
                        TypeColumn = Columnreader["DATA_TYPE"].ToString(),
                        IsNullable = Columnreader["IS_NULLABLE"].ToString() == "YES",
                        IsPrimaryKey = CollectionPrimaryKeys.Any(col => col.Equals(Columnreader["COLUMN_NAME"])),
                        IsComputed = CollectionComputedKeys.Any(col => col.Equals(Columnreader["COLUMN_NAME"])),
                        IsForeignKey = CollectionForeignKeys.Any(col => col.Split(',')[2].Equals(Columnreader["COLUMN_NAME"])),
                        TableForeignKeyName = CollectionForeignKeys.FirstOrDefault(col => col.Split(',')[2].Equals(Columnreader["COLUMN_NAME"]))?.Split(',')[1] ?? ""

                    };
                    databaseModels.Add(columnInfoModel);

                }
            }

            return databaseModels;

        }

        public async Task<ErrorOr<Dictionary<string, FakeDataTypesDto>>> AddFakeDataAsync(FakeDataToTableDto model)
        {
            Dictionary<string, FakeDataTypesDto> GetColumnFakeValue = new();
            string connectionString = model.Database.ConnectionString;
            //var currentTable = model.Tables.FirstOrDefault();
            foreach (var currentTable in model.Tables)
            {
                var columnsResult = await GetTableColumnAsync(new GetColumnByTableDto
                {
                    Database = model.Database,
                    Table = currentTable
                });

                if (columnsResult.IsError)
                    return columnsResult.FirstError;

                var columns = columnsResult.Value.Where(c => !c.IsComputed).ToList();
                var columnNames = string.Join(',', columns.Select(c =>$"[{c.Name}]" ));
                var parameterNames = string.Join(',', columns.Select(c => "@" + c.Name));

                for (int i = 0; i < model.Count; i++)
                {
                    using var connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();

                    string insertQuery = $"INSERT INTO [{currentTable.TableName}] ({columnNames}) VALUES ({parameterNames})";

                    using var command = new SqlCommand(insertQuery, connection);

                    foreach (var column in columnsResult.Value.Where(_ => !_.IsComputed))
                    {


                        object value = null;

                        if (column.IsForeignKey)
                        {
                            value = await GetRandomForeignKeyValueAsync(connection, column.TableForeignKeyName);
                            if (value == null)
                                return Error.NotFound(TableMessage.NotFound_Row);
                        }
                        else
                        {
                            value = GenerateFake(ConvertSqlToClr(column.TypeColumn));
                            GetColumnFakeValue[column.Name] = ConvertSqlToClr(column.TypeColumn);
                        }

                        command.Parameters.AddWithValue("@" + column.Name, value ?? DBNull.Value);
                    }

                    try
                    {
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }


            return GetColumnFakeValue;
        }


        public async Task<ErrorOr<bool>> AddFakeDataAsync(FakeDataToCustomColumnsDto model)
        {
            string connectionString = model.Database.ConnectionString;
            var currentTable = model.Tables;

            var columnsResult = await GetTableColumnAsync(new GetColumnByTableDto
            {
                Database = model.Database,
                Table = currentTable
            });

            if (columnsResult.IsError)
                return columnsResult.FirstError;

            var columns = columnsResult.Value.Where(c => !c.IsComputed).ToList();
            var columnNames = string.Join(',', columns.Select(c => c.Name));
            var parameterNames = string.Join(',', columns.Select(c => "@" + c.Name));

            for (int i = 0; i < model.Count; i++)
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                string insertQuery = $"INSERT INTO [{currentTable.TableName}] ({columnNames}) VALUES ({parameterNames})";

                using var command = new SqlCommand(insertQuery, connection);

                foreach (var column in columnsResult.Value)
                {
                    object value = null;

                    if (column.IsForeignKey)
                    {
                        value = await GetRandomForeignKeyValueAsync(connection, column.TableForeignKeyName);
                        if (value == null)
                            return Error.NotFound(TableMessage.NotFound_Row);
                    }

                    else if (model.Columns.Any(_ => _.Name.Equals(column.Name)))
                    {
                        value = GenerateFake((model.Columns.Where(_ => _.Name.Equals(column.Name)).Select(_ => _.FakeDataTypesDto).FirstOrDefault()));
                    }

                    else
                    {
                        value = GenerateFake(ConvertSqlToClr(column.TypeColumn));

                    }

                    command.Parameters.AddWithValue("@" + column.Name, value ?? DBNull.Value);


                }

                try
                {
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return true;
        }

        public FakeDataTypesDto ConvertSqlToClr(string dataType)
        {
            dataType = dataType.ToLowerInvariant();

            var map = new Dictionary<string, FakeDataTypesDto>
            {
                ["int"] = FakeDataTypesDto.Digit,
                ["smallint"] = FakeDataTypesDto.Digit,
                ["tinyint"] = FakeDataTypesDto.Digit,
                ["bigint"] = FakeDataTypesDto.Digit,
                ["decimal"] = FakeDataTypesDto.Digit,
                ["numeric"] = FakeDataTypesDto.Digit,
                ["float"] = FakeDataTypesDto.Digit,
                ["real"] = FakeDataTypesDto.Digit,
                ["bit"] = FakeDataTypesDto.@bool,

                ["char"] = FakeDataTypesDto.String,
                ["nchar"] = FakeDataTypesDto.String,
                ["varchar"] = FakeDataTypesDto.String,
                ["nvarchar"] = FakeDataTypesDto.String,
                ["text"] = FakeDataTypesDto.String,
                ["ntext"] = FakeDataTypesDto.String,

                ["datetime"] = FakeDataTypesDto.Date,
                ["datetime2"] = FakeDataTypesDto.Date,
                ["smalldatetime"] = FakeDataTypesDto.Date,
                ["date"] = FakeDataTypesDto.Date,
                ["time"] = FakeDataTypesDto.Time,

                ["binary"] = FakeDataTypesDto.Pictrue,
                ["image"] = FakeDataTypesDto.Pictrue,
                ["varbinary"] = FakeDataTypesDto.Pictrue,

                ["uniqueidentifier"] = FakeDataTypesDto.String,
                ["money"] = FakeDataTypesDto.DigitDecimal,
                ["smallmoney"] = FakeDataTypesDto.DigitDecimal
            };

            return map.TryGetValue(dataType, out var result) ? result : FakeDataTypesDto.None;
        }

        public object GenerateFake(FakeDataTypesDto type)
        {

            {
                return type switch
                {
                    FakeDataTypesDto.Name =>
                        FakeValue.Names[Random.Shared.Next(FakeValue.Names.Count)],

                    FakeDataTypesDto.String =>
                        Guid.NewGuid().ToString(),

                    FakeDataTypesDto.City =>
                        FakeValue.Cities[Random.Shared.Next(FakeValue.Cities.Count)],

                    FakeDataTypesDto.Phone =>
                        FakeValue.PhoneNumbers[Random.Shared.Next(FakeValue.PhoneNumbers.Count)],

                    FakeDataTypesDto.Digit =>
                        Random.Shared.Next(0, 1000),

                    FakeDataTypesDto.DigitLong =>
                        Random.Shared.NextInt64(0, 1000000000).ToString(),

                    FakeDataTypesDto.DigitDecimal =>
                        (Random.Shared.NextDouble() * 1000).ToString("F2"),

                    FakeDataTypesDto.DigitFloat =>
                        ((float)Random.Shared.NextDouble() * 1000).ToString("F2"),

                    FakeDataTypesDto.@bool =>
                        (Random.Shared.Next(0, 2) == 1).ToString(), // "True" or "False"

                          //.ToShamsi()
                    FakeDataTypesDto.Date =>
                        DateTime.Now.AddDays(Random.Shared.Next(-365, 0)),

                    FakeDataTypesDto.Time =>
                        DateTime.Today.AddSeconds(Random.Shared.Next(0, 86400)).ToString("HH:mm:ss"),

                    FakeDataTypesDto.Pictrue =>
                        "https://picsum.photos/200?random=" + Random.Shared.Next(1, 1000),

                    FakeDataTypesDto.Automatic =>
                        null,

                    FakeDataTypesDto.uniqString =>
                        Guid.NewGuid().ToString(),
                    FakeDataTypesDto.Computed =>
                        null,

                    FakeDataTypesDto.None =>
                        string.Empty,

                    _ =>
                        throw new ArgumentException("Unsupported fake data type", nameof(type))
                };
            }

        }



        public async Task<object?> GetRandomForeignKeyValueAsync(SqlConnection connection, string foreignTable)
        {
            string primaryKeyQuery = $@"
        SELECT KCU.COLUMN_NAME AS ColumnName
        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
        JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
            ON TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME
        WHERE TC.TABLE_NAME = '{foreignTable}' AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY';";

            using var primaryKeyCmd = new SqlCommand(primaryKeyQuery, connection);
            using var reader = await primaryKeyCmd.ExecuteReaderAsync();

            if (!reader.Read())
                return null;

            string primaryKeyColumn = reader["ColumnName"].ToString();

            await reader.CloseAsync();

            string randomRecordQuery = $"SELECT TOP 1 [{primaryKeyColumn}] AS PrimaryColumnName FROM [{foreignTable}] ORDER BY NEWID();";

            using var randomCmd = new SqlCommand(randomRecordQuery, connection);
            var result = await randomCmd.ExecuteScalarAsync();

            return result;
        }

    }
}
