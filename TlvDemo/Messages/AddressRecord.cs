using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    public class AddressRecord : ITlvContract
    {
        public AddressRecord()
        { }

        int ITlvContract.ContractId => 2;

        public int LotNumber { get; set; }

        public string StreetName { get; set; }

        void ITlvContract.Parse( ITlvParseContext context )
        {
            this.LotNumber = context.ParseChild<IntTag>( 0 );
            this.StreetName = context.ParseChild<StringTag>( 1 );
        }

        void ITlvContract.Save( ITlvSaveContext context )
        {
            context.Save( 0, new IntTag( this.LotNumber ) );
            context.Save( 1, new StringTag( this.StreetName ) );
        }
    }
}
