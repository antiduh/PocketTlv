using System;

namespace TlvDemo.TlvApi
{
    public interface ITlvContract
    {
        int ContractId { get; }

        void Parse( ITlvParseContext parseContext );

        void Save( ITlvSaveContext saveContext );
    }
}