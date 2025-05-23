﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Messenger
{
    class SQLiteUser(string name, string login, string password)
    {
        private string _userName = name;
        private string _userLogin = login;
        private string _userPassword = password;
        public bool CreatTable(string tableName="Users", string command="(UserName STRING, UserLogin STRING, UserPassword STRING, MemberChats STRING)")
        {
            string table = executeScalar($"SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'");
            command = "CREATE TABLE" + ' ' + tableName + ' ' + command;
            if (int.Parse(table) == 0)
            {
                executeNonQuery(command);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddUser()
        {
            string table = executeScalar($"SELECT COUNT(*) FROM Users WHERE UserName = '{_userName}' AND UserLogin = '{_userLogin}' AND UserPassword = '{_userPassword}'");
            string command = $"INSERT INTO Users (UserName, UserLogin, UserPassword) VALUES ('{_userName}', '{_userLogin}', '{_userPassword}')";
            if (int.Parse(table) == 0)
            {
                executeNonQuery(command);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeletUser()
        {
            string table = executeScalar($"SELECT COUNT(*) FROM Users WHERE UserName = '{_userName}' AND UserLogin = '{_userLogin}' AND UserPassword = '{_userPassword}'");
            string command = $"DELETE FROM Users WHERE UserName = '{_userName}' AND UserLogin = '{_userLogin}' AND UserPassword = '{_userPassword}'";
            if (int.Parse(table) != 0)
            {
                executeNonQuery(command);
            }
        }

        public bool ChangeUserSettings(string newName="*", string newLogin="*", string newPassword="*")
        {
            newName = (newName == "*" ? _userName : newName);
            newLogin = (newLogin == "*" ? _userLogin : newLogin);
            newPassword = (newPassword == "*" ? _userPassword : newPassword);
            string table = executeScalar($"SELECT COUNT(*) FROM Users WHERE UserName = '{_userName}' AND UserLogin = '{_userLogin}' AND UserPassword = '{_userPassword}'");
            string command = $"UPDATE Users SET UserName = '{newName}' AND UserLogin = '{newLogin}' AND UserPassword = '{newPassword}' WHERE UserName = '{_userName}' AND UserLogin = '{_userLogin}' AND UserPassword = '{_userPassword}'";
            if (int.Parse(table) == 1)
            {
                executeNonQuery(command);
                _userName = newName;
                _userLogin = newLogin;
                _userPassword = newPassword;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] GetUserChat()
        {
            string command = $"SELECT MemberChats FROM Users WHERE UserName = '{_userName}' AND UserLogin = '{_userLogin}' AND UserPassword = '{_userPassword}'";
            return executeReader(command);
        }


        public void KillAll() // системная функция которая убивает все. Егор не юзай ее пж
        {
            string command = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%'";
            string[] a = executeReader(command);
            foreach (string b in a)
            {
                command = $"DROP TABLE {b}";
                executeNonQuery(command);
            }
            command = $"SELECT COUNT(*) FROM sqlite_master WHERE type = 'table'";
            Console.WriteLine(executeScalar(command));
        }

        public void DrawDataBase() // тоже системная функция, тебе егор не нужна
        {
            string command = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%'";
            string[] a = executeReader(command);
            foreach (string b in a)
            {
                Console.WriteLine(b + ':');
                command = $"SELECT UserName , UserLogin , UserPassword FROM {b}";
                foreach (string c in executeReader(command))
                {
                    Console.WriteLine(c);
                }
            }
        }


        /*
         * Нужно изменить данные (добавить, удалить, обновить)? → ExecuteNonQuery.
         * Нужно получить набор данных (много строк/колонок)? → ExecuteReader.
         * Нужно одно значение (количество, сумма и т.д.)? → ExecuteScalar.
         */

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
            string[] answer = new string[0];
            using (var connection = new SqliteConnection("Data Source=testDatabase.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandS;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        List<string> values = new List<string>(0);
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                for (int i = 0; i < reader.FieldCount; i++) {
                                    values.Add(reader.GetString(i).ToString());
                                }
                            }
                        }
                        answer = values.ToArray();
                        return answer;
                    }
                    else
                    {
                        answer = new string[0];
                    }
                }
                connection.Close();
                return answer;
            }
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
