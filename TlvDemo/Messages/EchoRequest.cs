﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlvDemo.Messages
{
    public class EchoRequest
    {
        public EchoRequest()
        {
        }

        public EchoRequest( string text )
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}
