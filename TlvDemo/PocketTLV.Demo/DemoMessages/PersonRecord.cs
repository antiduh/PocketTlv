using System;
using PocketTLV;
using PocketTLV.ClassLib;
using PocketTLV.Primitives;

namespace TlvDemo.DemoMessages
{
    public class PersonRecord : ITlvContract
    {
        public const int ContractId = 1;

        public string Name { get; set; }

        public int Age { get; set; }

        public AddressRecord Address { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as PersonRecord );
        }

        public bool Equals( PersonRecord other )
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
                    this.Name == other.Name &&
                    this.Age == other.Age &&
                    this.Address.Equals( other.Address );
            }
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode( this.Name, this.Age, this.Address );
        }

        // --- ITlvContract implementation ---

        int ITlvContract.ContractId => ContractId;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            this.Name = parse.ParseTag<StringTag>( 1 );
            this.Age = parse.ParseTag<IntTag>( 2 );
            this.Address = parse.ParseSubContract<AddressRecord>( 3 );
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            save.Save( 1, new StringTag( this.Name ) );
            save.Save( 2, new IntTag( this.Age ) );
            save.Save( 3, this.Address );
        }
    }
}