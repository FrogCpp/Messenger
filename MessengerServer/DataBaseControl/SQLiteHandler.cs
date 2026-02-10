using Shared.Source;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MessengerServer
{



    internal class SQLiteHandler
    {
        public static string PATH
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                else
                {
                    string userName = Environment.UserName;  
                    return Path.Combine("/var/lib", userName, "myappname");
                }
            }
        }
        public class UserTable
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Bio {  get; set; }
            public UInt64 Suid { get; set; }
            public string AvatarPath { get; set; }


            public JN_Author ParseTo_JN()
            {
                return new JN_Author(
                    Name,
                    Surname,
                    Bio,
                    Suid,
                    GetAvatar(AvatarPath)
                );
            }

            public UserTable TryParseToTable(string name, string surname, string bio, ulong suid, ImageSource avatar)
            {
                var a = new UserTable();
                a.Name = name;
                a.Surname = surname;
                a.Bio = bio;
                a.Suid = suid;
                a.AvatarPath = "";

                return a;
            }

            private ImageSource GetAvatar(string Path)
            {
                return ImageSource.FromFile(Path);
            }

            private string SetAvatar(ImageSource avatar)
            {
                string a = "";
                return a;
            }
        }


        public SQLiteHandler(string dataBaseName)
        {
            Console.WriteLine(PATH);

            Directory.CreateDirectory(PATH + "\\MessengerDataBase");
            Console.WriteLine(PATH + "\\MessengerDataBase");
        }
    }
}
