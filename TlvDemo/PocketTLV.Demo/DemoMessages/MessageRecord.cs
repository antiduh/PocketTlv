using System;
using PocketTLV;
using PocketTLV.Primitives;

namespace TlvDemo.DemoMessages
{
    public class MessageRecord : ITlvContract
    {
        public string Name { get; set; }

        public ITlvContract Message { get; set; }

        // --- ITlvContract implementation ---

        int ITlvContract.ContractId => 3;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            this.Name = parse.Tag<StringTag>( 1 );
            this.Message = parse.Contract( 2 );
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            save.Tag( 1, new StringTag( this.Name ) );
            save.Contract( 2, this.Message );
        }
    }
}