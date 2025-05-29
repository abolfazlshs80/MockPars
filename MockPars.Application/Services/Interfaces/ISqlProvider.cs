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

        Task<ErrorOr<List<ColumnInfoDto>>> GetTableColumnAsync(GetColumnByTableDto model);
        Task<ErrorOr<bool>> AddFakeDataAsync(FakeDataToTableDto model);

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



                //var CommandGetPrimaryKey = new SqlCommand("\tSELECT \r\n " +
                //                                          "   KCU.COLUMN_NAME as ColumnName, \r\n  " +
                //                                          "  TC.CONSTRAINT_NAME\r\nFROM \r\n " +
                //                                          "   INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC\r\nJOIN \r\n  " +
                //                                          "  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU\r\n  " +
                //                                          "  ON TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME\r\nWHERE" +
                //                                          " \r\n " +
                //                                          $"   TC.TABLE_NAME = '{firstTable}'\r\n " +
                //                                          "   AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY';\r\n", con);
                //var GetPrimaryKeyReader = CommandGetPrimaryKey.ExecuteReader();
                //List<string> CollectionGetPrimaryKeys = new List<string>();
                //while (GetPrimaryKeyReader.Read())
                //{
                //    CollectionGetPrimaryKeys.Add(GetPrimaryKeyReader["ColumnName"].ToString());
                //}





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

        public async Task<ErrorOr<bool>> AddFakeDataAsync(FakeDataToTableDto model)
        {
            string connectionString = model.Database.ConnectionString;
            var currentTable = model.Tables.FirstOrDefault();
            var columns = await GetTableColumnAsync(new GetColumnByTableDto()
            { Database = model.Database, Table = currentTable });
            var columnName = string.Join(',', columns.Value.Where(a => !a.IsPrimaryKey).Select(a => a.Name));
            var parameterName = string.Join(',', columns.Value.Where(a => !a.IsPrimaryKey).Select(a => "@" + a.Name));

            for (int i = 0; i < model.Count; i++)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {


                    string query = $"INSERT INTO [{currentTable.TableName}] ({columnName}) VALUES ({parameterName})";
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            foreach (var item in columns.Value)
                            {
                                object value = null;
                                //if (item.IsPrimaryKey)
                                //    value = ConvertSqlToClr(item.TypeColumn) == FakeDataTypesDto.Digit ?0:null;
                                if (item.IsForeignKey)
                                {

                                    var CommandGetPrimaryKey = new SqlCommand("\tSELECT \r\n " +
                                                                              "   KCU.COLUMN_NAME as ColumnName, \r\n  " +
                                                                              "  TC.CONSTRAINT_NAME\r\nFROM \r\n " +
                                                                              "   INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC\r\nJOIN \r\n  " +
                                                                              "  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU\r\n  " +
                                                                              "  ON TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME\r\nWHERE" +
                                                                              " \r\n " +
                                                                              $"   TC.TABLE_NAME = '{item.TableForeignKeyName}'\r\n " +
                                                                              "   AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY';\r\n", connection);
                                    var GetPrimaryKeyReader = CommandGetPrimaryKey.ExecuteReader();
                                    //  List<string> CollectionGetPrimaryKeys = new List<string>();
                                    while (GetPrimaryKeyReader.Read())
                                    {
                                        var CommandGetPrimaryKeyRecord = new SqlCommand($"select top(1) [{GetPrimaryKeyReader["ColumnName"]}] as PrimaryColumnName from [{item.TableForeignKeyName}] ORDER BY NEWID()", connection);
                                        var GetPrimaryKeyRecordReader = CommandGetPrimaryKeyRecord.ExecuteReader();
                                        while (GetPrimaryKeyRecordReader.Read())
                                        {
                                            value = GetPrimaryKeyRecordReader["PrimaryColumnName"].ToString();

                                        }
                                        if (GetPrimaryKeyRecordReader.HasRows == false)
                                        {
                                            return Error.NotFound(TableMessage.NotFound_Row);
                                        }
                                        //   CollectionGetPrimaryKeys.Add(GetPrimaryKeyReader["ColumnName"].ToString());
                                    }


                                }
                                else
                                    value = item.IsComputed ? null : GenerateFake(ConvertSqlToClr(item.TypeColumn));

                                if (!item.IsPrimaryKey)
                                    command.Parameters.AddWithValue("@" + item.Name, value);
                            }



                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"{rowsAffected} row(s) inserted.");



                        }

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }






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

        object GenerateFake(FakeDataTypesDto type)
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

                    FakeDataTypesDto.Date =>
                        DateTime.Now.AddDays(Random.Shared.Next(-365, 0)).ToShamsi(),

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
    }
}
