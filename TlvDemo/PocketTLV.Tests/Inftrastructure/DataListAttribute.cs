using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PocketTLV.Tests.Inftrastructure
{
    public class DataListAttribute : Attribute, ITestDataSource
    {
        private readonly object[] values;

        public DataListAttribute( params object[] values )
        {
            this.values = values;
        }

        public IEnumerable<object[]> GetData( MethodInfo methodInfo )
        {
            foreach( object value in values )
            {
                yield return new[] { value };
            }
        }

        public string GetDisplayName( MethodInfo methodInfo, object[] data )
        {
            return methodInfo.Name + " (" + data[0] + ")";
        }
    }
}