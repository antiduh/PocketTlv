using System;
using PocketTlv.ClassLib;

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

        public bool Equals( CarrierRecord other )
        {
            if( other is null )
            {
                return false;
            }

            return other.Value == this.Value && other.Child.Equals( this.Child );
        }

        public override bool Equals( object obj )
        {
            return Equals( obj as CarrierRecord );
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode( this.Value, this.Child );
        }

        // ----- ITlvContract implementation ------

        int ITlvContract.ContractId => StubTlvs.Carrier;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            this.Value = parse.Tag<IntTag>( 0 );
            this.Child = parse.Contract( 1 );
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            save.Tag( 0, new IntTag( this.Value ) );
            save.Contract( 1, this.Child );
        }
    }
}