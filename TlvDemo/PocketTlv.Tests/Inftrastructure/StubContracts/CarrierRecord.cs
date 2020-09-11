using System;

namespace PocketTlv.Tests.Inftrastructure.StubContracts
{
    public class CarrierRecord : ITlvContract
    {
        public CarrierRecord() { }

        public CarrierRecord( int value, ITlvContract child )
        {
            this.Value = value;
            this.Child = child;
        }

        public int Value { get; set; }

        public ITlvContract Child { get; set; }

        public int ContractId => StubTlvs.Carrier;

        public void Parse( ITlvParseContext parse )
        {
            this.Value = parse.Tag<IntTag>( 0 );
            this.Child = parse.Contract( 1 );
        }

        public void Save( ITlvSaveContext save )
        {
            save.Tag( 0, new IntTag( this.Value ) );
            save.Contract( 1, this.Child );
        }
    }
}