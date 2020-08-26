using System;
using TlvDemo.ClassLib;
using TlvDemo.TlvApi;

namespace TlvDemo
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

        int ITlvContract.ContractId => PersonRecord.ContractId;

        void ITlvContract.Parse( ITlvParseContext context )
        {
            this.Name = context.ParseTag<StringTag>( 1 );
            this.Age = context.ParseTag<IntTag>( 2 );
            this.Address = context.ParseSubContract<AddressRecord>( 3 );
        }

        void ITlvContract.Save( ITlvSaveContext context )
        {
            context.Save( 1, new StringTag( this.Name ) );
            context.Save( 2, new IntTag( this.Age ) );
            context.Save( 3, this.Address );
        }
    }
}