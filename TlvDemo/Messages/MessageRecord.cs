﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    public class MessageRecord : ITlvContract
    {
        public string Name { get; set; }

        public ITlvContract Message { get; set; }

        int ITlvContract.ContractId => 3;

        void ITlvContract.Parse( ITlvParseContext parseContext )
        {
            this.Name = parseContext.ParseChild<StringTag>( 1 );
            this.Message = parseContext.ParseSubContract( 2 );
        }

        void ITlvContract.Save( ITlvSaveContext saveContract )
        {
            saveContract.Save( 1, new StringTag( this.Name ) );
            saveContract.Save( 2, this.Message );
        }
    }
}