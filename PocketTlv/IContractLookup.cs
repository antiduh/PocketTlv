namespace PocketTlv
{
    public interface IContractLookup
    {
        bool Contains( int contractId );

        ITlvContract Get( int contractId );

        bool TryGet( int contractId, out ITlvContract contract );
    }
}