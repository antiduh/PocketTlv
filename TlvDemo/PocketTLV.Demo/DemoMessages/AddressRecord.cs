using System;
using PocketTlv;
using PocketTlv.ClassLib;

namespace PocketTlv.Demo.Messages
{
    public class AddressRecord : ITlvContract
    {
        public AddressRecord()
        { }

        public int LotNumber { get; set; }

        public string StreetName { get; set; }

        public override bool Equals( object other )
        {
            return base.Equals( other );
        }

        public bool Equals( AddressRecord other )
        {
            if( ReferenceEquals( other, null ) )
            {
                return false;
            }
            else if( ReferenceEquals( other, this ) )
            {
                return true;
            }
            else
            {
                return
                    this.LotNumber == other.LotNumber &&
                    this.StreetName == other.StreetName;
            }
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode( this.LotNumber, this.StreetName );
        }

        // --- ITlvContract implementation ---

        int ITlvContract.ContractId => 2;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            this.LotNumber = parse.Tag<IntTag>( 0 );
            this.StreetName = parse.Tag<StringTag>( 1 );
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            save.Tag( 0, new IntTag( this.LotNumber ) );
            save.Tag( 1, new StringTag( this.StreetName ) );
        }
    }
}