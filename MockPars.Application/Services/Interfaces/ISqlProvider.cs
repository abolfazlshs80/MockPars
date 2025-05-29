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

namespace MockPars.Application.Services.Interfaces
{
    public interface ISqlProvider
    {
        Task<ErrorOr<IEnumerable<TableInfoDto>>> GetTablesAsync(ConnectionDatabaseDto model,
            CancellationToken cancellationToken);

        Task<List<ColumnInfoDto>> GetTableColumnAsync(GetColumnByTableDto model);
        Task<bool> AddFakeDataAsync(FakeDataToTableDto model);

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

        public async Task<List<ColumnInfoDto>> GetTableColumnAsync(GetColumnByTableDto model)
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
                        IsComputed = CollectionComputedKeys.Any(col => col.Equals(Columnreader["COLUMN_NAME"]))

                    };
                    databaseModels.Add(columnInfoModel);

                }
            }

            return databaseModels;

        }

        public async Task<bool> AddFakeDataAsync(FakeDataToTableDto model)
        {
            string connectionString = model.Database.ConnectionString;
            var currentTable = model.Tables.FirstOrDefault();
            var columns = await GetTableColumnAsync(new GetColumnByTableDto()
                { Database = model.Database, Table = currentTable });
            var columnName = string.Join(',', columns.Select(a => a.Name));
            var parameterName = string.Join(',', columns.Select(a => "@" + a.Name));
            string query = $"INSERT INTO [{currentTable.TableName}] ({columnName}) VALUES ({parameterName})";
            for (int i = 0; i < model.Count; i++)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        foreach (var item in columns)
                        {
                            object value = item.IsComputed ? null : GenerateFake(ConvertSqlToClr(item.TypeColumn));

                            command.Parameters.AddWithValue("@" + item.Name, value);
                        }

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"{rowsAffected} row(s) inserted.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
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
                ["bigint"] = FakeDataTypesDto.DigitLong,
                ["decimal"] = FakeDataTypesDto.DigitDecimal,
                ["numeric"] = FakeDataTypesDto.DigitDecimal,
                ["float"] = FakeDataTypesDto.DigitFloat,
                ["real"] = FakeDataTypesDto.DigitFloat,
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

        string GenerateFake(FakeDataTypesDto type)
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
                    Random.Shared.Next(0, 1000).ToString(),

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
