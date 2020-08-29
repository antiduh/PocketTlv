﻿using System;
using PocketTLV.Primitives;

namespace PocketTLV.Tests.Inftrastructure.StubContracts
{
    public class IntContract1 : ITlvContract
    {
        public const int Id = 1;

        public IntContract1()
        {
        }

        public int ContractId => Id;

        public int Value { get; set; }

        public void Parse( ITlvParseContext parseContext )
        {
            this.Value = parseContext.ParseTag<IntTag>( 0 );
        }

        public void Save( ITlvSaveContext saveContext )
        {
            saveContext.Save( 0, new IntTag( 0 ) );
        }
    }
}