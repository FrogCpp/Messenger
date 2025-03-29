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
                execute("CREATE TABLE IF NOT EXISTS Users (UserName STRING, UserLogin STRING, UserPasword STRING, MemberChats STRING)");
            }
        }

        private string executeScalar(string commandS, string param="nonQuery")
        {
            string answer = "";
            using (var connection = new SqliteConnection("Data Source=testDatabase.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandS;
                object ans = command.ExecuteScalar();
                answer = ans.ToString();
            }
            return answer;
        }
        private string[] executeReader(string commandS)
        {
            string[] answer;
            using (var connection = new SqliteConnection("Data Source=testDatabase.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandS;
                using (var reader = command.ExecuteReader())
                {
                    answer = new string[reader.FieldCount];
                    int i = 0;
                    while (reader.Read())
                    {
                        answer[i] = reader.GetString(i++);
                    }
                }
                connection.Close();
            }
            return answer;
        }
        private void executeNonQuery(string commandS)
        {
            using (var connection = new SqliteConnection("Data Source=testDatabase.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandS;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
