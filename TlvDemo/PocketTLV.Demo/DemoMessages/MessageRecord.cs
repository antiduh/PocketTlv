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

        void ITlvContract.Parse( ITlvParseContext parseContext )
        {
            this.Name = parseContext.ParseTag<StringTag>( 1 );
            this.Message = parseContext.ParseSubContract( 2 );
        }

        void ITlvContract.Save( ITlvSaveContext saveContract )
        {
            saveContract.Save( 1, new StringTag( this.Name ) );
            saveContract.Save( 2, this.Message );
        }
    }
}