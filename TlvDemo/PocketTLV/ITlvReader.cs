using PocketTLV.Primitives;

namespace PocketTLV
{
    public interface ITlvReader
    {
        ITlvContract ReadContract();
        T ReadContract<T>() where T : ITlvContract, new();
        ITag ReadTag();
        T ReadTag<T>() where T : ITag;
        void RegisterContract<T>() where T : ITlvContract, new();
    }
}