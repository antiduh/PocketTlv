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

        public int FieldId { get; set; }

        public WireType WireType => WireType.ContractId;

        public int ContractId { get; set; }

        public int ComputeLength()
        {
            return 4;
        }

        public void ReadValue( byte[] buffer, int position, int length )
        {
            this.ContractId = DataConverter.ReadIntLE( buffer, position );
        }

        public void WriteValue( byte[] buffer, int position )
        {
            DataConverter.WriteIntLE( this.ContractId, buffer, position );
        }

        public static implicit operator int( ContractIdTag tag )
        {
            return tag.ContractId;
        }
    }
}