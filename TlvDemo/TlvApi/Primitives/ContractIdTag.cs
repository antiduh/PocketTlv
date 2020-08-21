using System;

namespace TlvDemo.TlvApi
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

        public int ContractId { get; set; }

        public static implicit operator int( ContractIdTag tag )
        {
            return tag.ContractId;
        }

        // --- ITag implementation ---

        int ITag.FieldId { get; set; }

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