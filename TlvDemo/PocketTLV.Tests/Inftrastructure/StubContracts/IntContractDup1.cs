using System;
using PocketTLV.Primitives;

namespace PocketTLV.Tests.Inftrastructure.StubContracts
{
    /// <summary>
    /// A stub contract that has a contract ID that conflicts with <see cref="IntContract1"/>.
    /// </summary>
    public class IntContractDup1 : ITlvContract
    {
        public const int Id = 1;

        public IntContractDup1()
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