using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    public class PersonRecord : ITlvContract
    {
        public const int ContractId = 1;

        public string Name { get; set; }

        public int Age { get; set; }

        public AddressRecord Address { get; set; }

        int ITlvContract.ContractId => PersonRecord.ContractId;

        void ITlvContract.Parse( ITlvParseContext context )
        {
            this.Name = context.ParseChild<StringTag>( 1 );
            this.Age = context.ParseChild<IntTag>( 2 );
            this.Address = context.ParseSubContract<AddressRecord>( 3 );
        }

        void ITlvContract.Save( ITlvSaveContext context )
        {
            context.Save( 1, new StringTag( this.Name ) );
            context.Save( 2, new IntTag( this.Age ) );
            context.Save( 3, this.Address );
        }
    }
}
