using System;
using TlvDemo.TlvApi;

namespace TlvDemo.Messages
{
    public class ClientFrame
    {
        public string Exchange { get; set; }

        public string RoutingKey { get; set; }

        public string MessageName { get; set; }

        public string Guid { get; set; }

        public ITag Message { get; set; }
    }
}