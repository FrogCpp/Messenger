using AVcontrol;
using Shared.Source.tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AC
{
    public partial class NetDriverCore
    {
        public async Task SendMassiveMesage(Socket sock, string pathToFile)
        {
            string fileName = Path.GetFileName(pathToFile);

            FileInfo fileInfo = new FileInfo(pathToFile);
            long fileSize = fileInfo.Length;

            int part = 1024 * 1024 * 32;                                          // размер одного пакета в среднем

            while (fileSize % part != 0) part--;

            int piceCount =  (int)(fileSize / part);

            var configMessage = new Message(null, ToBinary.Utf16(fileName), piceCount);

            Guid mainGuid = configMessage.msgsuid;

            var firstAns = await SendReqMessageAsync(sock, configMessage);
            if (firstAns != null)
            {
                Dictionary<Message, Task> sending = new();


                using (FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[part];
                    int sn = 0;

                    while (fs.Read(buffer, 0, buffer.Length) > 0)
                    {
                        var msg = new Message(mainGuid, buffer, sn);
                        sending.Add(msg, SendReqMessageAsync(sock, msg));
                        sn++;
                    }
                }

                while (sending.Count > 0)
                {
                    var completed = await Task.WhenAny(sending.Values);

                    var kvp = sending.FirstOrDefault(x => x.Value == completed);
                    if (completed == null && kvp.Key != null)
                    {
                        if (sending.Remove(kvp.Key))
                            sending.Add(kvp.Key, SendReqMessageAsync(sock, kvp.Key));
                        else
                            DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Error, "code is dead 0__o", LOGFOLDER));
                    }
                }
            }
            else
            {
                DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Warning, "the other party is not responding", LOGFOLDER));
            }

        }          
    }
}
