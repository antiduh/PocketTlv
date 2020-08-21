using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TlvDemo.TlvApi;

namespace TlvDemo.TlvApi
{
    public interface ITlvContract
    {
        int ContractId { get; }

        void Parse( ITlvParseContext parseContext );

        void Save( ITlvSaveContext saveContext );
    }
}
