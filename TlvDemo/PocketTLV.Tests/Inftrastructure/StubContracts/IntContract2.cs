using System;
using PocketTLV.Primitives;

namespace PocketTLV.Tests.Inftrastructure.StubContracts
{
    public class IntContract2 : ITlvContract
    {
        public const int Id = 2;

        public IntContract2()
        {
        }

        public int ContractId => Id;

        public int Value { get; set; }

        public void Parse( ITlvParseContext parseContext )
        {
            this.Value = parseContext.ParseTag<IntTag>( 0 );
        }

        public void Save( ITlvSaveContext saveContext )
        {
            saveContext.Save( 0, new IntTag( 0 ) );
        }
    }
}