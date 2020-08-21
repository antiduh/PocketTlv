using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace TlvDemo.TlvApi
{
    public class StreamConverter
    {
        private readonly Stream stream;

        private byte[] buffer;

        public StreamConverter( Stream stream )
        {
            this.stream = stream;
            this.buffer = new byte[10];
        }

        public void WriteShortLE( short value )
        {
            stream.WriteByte( (byte)( value >> 0 ) );
            stream.WriteByte( (byte)( value >> 8 ) );
        }

        public bool ReadShortLE( out short value )
        {
            if( ReadHarder( this.buffer, 0, 2 ) == false )
            {
                value = 0;
                return false;
            }

            value = DataConverter.ReadShortLE( this.buffer, 0 );
            return true;
        }

        public bool ReadIntLE( out int value )
        {
            if( ReadHarder( this.buffer, 0, 4 ) == false )
            {
                value = 0;
                return false;
            }

            value = DataConverter.ReadIntLE( this.buffer, 0 );
            return true;
        }

        public bool ReadHarder( byte[] buffer, int start, int length )
        {
            int remaining = length;
            int position = start;

            int readLen;

            while( remaining > 0 )
            {
                readLen = this.stream.Read( buffer, position, remaining );

                if( readLen == 0 )
                {
                    return false;
                }

                remaining -= readLen;
                position += readLen;
            }

            return true;
        }
    }
}