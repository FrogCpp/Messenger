using Microsoft.Data.Sqlite;
using System.Runtime.InteropServices;
using static JabNet.USC;

namespace MessengerServer
{
    internal static class SQLiteHandler
    {
        public class Column
        {
            public Column(string name, Column.Types type, int? lenght = null)
            {
                Name = name;
                Type = type;
                Lenght = lenght;
                if (Type == Types.VARCHAR && lenght == null)
                {
                    throw new ArgumentException("less argument for VARCHAR type");
                }
            }
            public readonly string Name;
            public readonly Types Type;
            public readonly int? Lenght;

            public enum Types
            {
                TEXT,
                INTEGER,
                FLOAT,
                VARCHAR,
                DATE,
                TIME,
                DATETIME,
            }
        }

        public static readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string ImageSourcePATH = Path.Combine(PATH, "massiveUserData");

        public static readonly SqliteConnection connection = new($"Data Source={Path.Combine(PATH, "userDataStorage.db")}");
        private static readonly List<string> _tableList = new();
        public static List<string> TableList { get => _tableList.Skip(1).ToList(); }

        public static async void InitAsync()
        {
            connection.Open();

            var checkExists = connection.CreateCommand();
            checkExists.CommandText =
                """
                SELECT name FROM sqlite_master WHERE type='table'
                """;

            using (var reader = await checkExists.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    _tableList.Add(reader.GetString(reader.GetOrdinal("name")));
                }
            }
        }

        public static async Task<bool> RemoveTableAsync(string name)
        {
            var Cmd = connection.CreateCommand();
            Cmd.CommandText =
                $"""
                DROP TABLE {name}
                """;
            try
            {
                await Cmd.ExecuteNonQueryAsync();
                _tableList.Remove(name);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> CreateTableAsync(string tableName, Column[] columns)
        {
            if (columns.Length == 0) return false;

            string args = "ID INTEGER PRIMARY KEY AUTOINCREMENT, ";
            foreach (var arg in columns)
            {
                if (arg.Type != Column.Types.VARCHAR)
                {
                    args += $"{arg.Name} {arg.Type.ToString()}, ";
                }
                else
                {
                    args += $"{arg.Name} {arg.Type.ToString()}({arg.Lenght}), ";
                }
            }
            args = args[..^2];


            var Cmd = connection.CreateCommand();
            Cmd.CommandText =
                $"""
                CREATE TABLE {tableName} ({args})
                """;

            try
            {
                await Cmd.ExecuteNonQueryAsync();
                _tableList.Add(tableName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> RewriteValueAsync<T>(string tableName, string column, T val, UInt32 stringID)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                $"""
                UPDATE {tableName} SET {column} = {val} WHERE ID = {stringID}
                """;

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<UInt32?> InsertRowAsync(string tableName)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"INSERT INTO {tableName} DEFAULT VALUES; SELECT last_insert_rowid();";
            var newId = await cmd.ExecuteScalarAsync();
            return newId != null ? Convert.ToUInt32(newId) : (UInt32?)null;
        }

        public static async Task RemoveRowAsync(string tableName, UInt32 id)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM {tableName} WHERE id = {id};";
        }

        public static async Task<UInt32?> SaerchIdByValueAsync<T>(string tableName, string columnName, T value)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT ID FROM {tableName} WHERE {columnName} = {value}";
            var newId = await cmd.ExecuteScalarAsync();
            return newId != null ? Convert.ToUInt32(newId) : (UInt32?)null;
        }

        public static async Task DebugPrintAsync() // вайбкод (исправлю)
        {
            var cmdTables = connection.CreateCommand();
            cmdTables.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";

            using (var readerTables = await cmdTables.ExecuteReaderAsync())
            {
                while (await readerTables.ReadAsync())
                {
                    string tableName = readerTables.GetString(0);
                    Console.WriteLine($"\n=== Таблица: {tableName} ===");

                    var cmdPragma = connection.CreateCommand();
                    cmdPragma.CommandText = $"PRAGMA table_info(\"{tableName}\");";

                    using (var readerColumns = await cmdPragma.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Колонки:");
                        while (await readerColumns.ReadAsync())
                        {
                            string columnName = readerColumns.GetString(readerColumns.GetOrdinal("name"));
                            string columnType = readerColumns.GetString(readerColumns.GetOrdinal("type"));
                            Console.WriteLine($"  - {columnName} ({columnType})");
                        }
                    }
                    var cmdData = connection.CreateCommand();
                    cmdData.CommandText = $"SELECT * FROM \"{tableName}\" LIMIT 50;";

                    using (var readerData = await cmdData.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Содержимое (первые 50 строк):");
                        for (int i = 0; i < readerData.FieldCount; i++)
                        {
                            Console.Write(readerData.GetName(i) + "\t");
                        }
                        Console.WriteLine();

                        while (await readerData.ReadAsync())
                        {
                            for (int i = 0; i < readerData.FieldCount; i++)
                            {
                                Console.Write(readerData.GetValue(i)?.ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }
}