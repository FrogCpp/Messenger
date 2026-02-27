using Microsoft.Data.Sqlite;
using System.Runtime.InteropServices;
using static JabNet.USC;

namespace MessengerServer
{
    internal static class SQLiteHandler
    {
        abstract public class IColumn(string name, IColumn.Types type)
        {
            public readonly string Name = name;
            public readonly Types Type = type;

            public enum Types
            {
                TEXT,
                INTEGER,
                FLOAT,
                VARCHAR, // добавить ограничение в n? символов.
                DATE,
                TIME,
                DATETIME,
            }
        }

        public static readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string ImageSourcePATH = Path.Combine(PATH, "massiveUserData");

        public static readonly SqliteConnection connection = new($"Data Source={Path.Combine(PATH, "userDataStorage.db")}");
        private static readonly List<string> _tableList = new();

        public static async void Init()
        {
            Console.WriteLine(PATH);

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

        public static async Task<bool> RemoveTable(string name)
        {
            var Cmd = connection.CreateCommand();
            Cmd.CommandText =
                $"""
                DROP TABLE IF EXISTS {name}
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

        public static async Task<bool> CreateTable(string tableName, IColumn[] columns)
        {
            if (columns.Length == 0) return false;

            string args = "ID INTEGER PRIMARY KEY AUTOINCREMENT, ";
            foreach (var arg in columns)
            {
                args += $"{arg.Name} {arg.Type.ToString()}, ";
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

        public static async Task<bool> InsertVal<T>(string tableName, string column, T val, UInt32 stringID)
        {
            var Cmd = connection.CreateCommand();
            Cmd.CommandText =
                $"""
                UPDATE {tableName} SET {column} = {val} WHERE ID = {stringID}
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
    }
}