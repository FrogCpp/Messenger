using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AB
{
    public class ClientContorlHub(IPAddress domain, UInt16 port, Action<byte[]> processor)
    {
        private readonly IPAddress _domain = domain;
        private readonly UInt16 _port = port;
        private readonly Action<byte[]> _processor = processor;

        private async Task<byte[]> RecivingMessageAsync()
        {

        }

        private async Task SendMessageAsync(byte[] content)
        {

        }
    }
}
