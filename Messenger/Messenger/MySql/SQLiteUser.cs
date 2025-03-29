using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Messenger
{
    class SQLiteUser
    {
        public void Start()
        {
            using (var connection = new SqliteConnection("Data Source=testDatabase.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                //command.CommandText = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT)";
                command.CommandText = "CREATE TABLE IF NOT EXISTS Users (UserName STRING, UserLogin STRING, UserPasword STRING, MemberChats STRING)";
                command.ExecuteNonQuery();
            }
        }
    }
}
