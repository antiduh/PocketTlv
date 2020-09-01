using System;
using PocketTLV.Primitives;

namespace PocketTLV.Tests.Inftrastructure.StubContracts
{
    public class IntContract1 : ITlvContract
    {
        public const int Id = 1;

        public IntContract1()
        {
        }

        public int ContractId => Id;

        public int Value { get; set; }

        public void Parse( ITlvParseContext parse )
        {
            this.Value = parse.Tag<IntTag>( 0 );
        }

        public void Save( ITlvSaveContext save )
        {
            save.Tag( 0, new IntTag( this.Value ) );
        }
    }
}