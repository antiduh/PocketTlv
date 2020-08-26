using System;
using PocketTLV.ClassLib;

namespace PocketTLV.Primitives
{
    public class ContractIdTag : ITag
    {
        public ContractIdTag()
        {
        }

        public ContractIdTag( int contractId )
        {
            this.ContractId = contractId;
        }

        public ContractIdTag( int fieldId, int contractId )
        {
            this.FieldId = fieldId;
            this.ContractId = contractId;
        }

        public int ContractId { get; set; }

        public override bool Equals( object other )
        {
            return Equals( other as ContractIdTag );
        }

        public bool Equals( ContractIdTag other )
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
                return this.ContractId == other.ContractId;
            }
        }

        public override int GetHashCode()
        {
            return this.ContractId.GetHashCode();
        }

        public static implicit operator int( ContractIdTag tag )
        {
            return tag.ContractId;
        }

        // --- ITag implementation ---

        public int FieldId { get; set; }

        WireType ITag.WireType => WireType.ContractId;

        int ITag.ComputeLength()
        {
            return 2;
        }

        void ITag.ReadValue( byte[] buffer, int position, int length )
        {
            this.ContractId = DataConverter.ReadShortLE( buffer, position );
        }

        void ITag.WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteShortLE( (short)this.ContractId, buffer, position );
        }
    }
}