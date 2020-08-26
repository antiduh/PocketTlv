using System;
using System.Runtime.InteropServices;

namespace PocketTLV.ClassLib
{
    /// <summary>
    /// Converts common data types from their native type to their big-endian and little-endian
    /// representations suitable for serialization.
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Writes a 16-bit signed integer to the given byte array in little-endian format. 2 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteShortLE( short value, byte[] data, int start )
        {
            WriteUShortLE( (ushort)value, data, start );
        }

        /// <summary>
        /// Reads a 16-bit signed integer from a byte array storing the value in
        /// little-endian format. 2 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static short ReadShortLE( byte[] data, int start )
        {
            return (short)ReadUShortLE( data, start );
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer to the given byte array in little-endian format. 2 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteUShortLE( ushort value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 2 );

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer from a byte array storing the value in little-endian
        /// format. 2 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static ushort ReadUShortLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 2 );

            ushort value;

            value = data[start + 0];
            value |= (ushort)( data[start + 1] << 8 );

            return value;
        }

        /// Writes a 32-bit signed integer to the given byte array in little-endian format. 4 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteIntLE( int value, byte[] data, int start )
        {
            WriteUIntLE( (uint)value, data, start );
        }

        /// <summary>
        /// Reads a 32-bit signed integer from a byte array storing the value in little-endian
        /// format. 4 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static int ReadIntLE( byte[] data, int start )
        {
            return (int)ReadUIntLE( data, start );
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer to the given byte array in little-endian format. 4 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteUIntLE( uint value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 4 );

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
            data[start + 2] = (byte)( value >> 16 );
            data[start + 3] = (byte)( value >> 24 );
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer from a byte array storing the value in little-endian
        /// format. 4 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static uint ReadUIntLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 4 );

            uint value;

            value = data[start + 0];
            value |= (uint)data[start + 1] << 8;
            value |= (uint)data[start + 2] << 16;
            value |= (uint)data[start + 3] << 24;

            return value;
        }

        /// <summary>
        /// Writes a 64-bit signed integer to the given byte array in little-endian format. 8 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteLongLE( long value, byte[] data, int start )
        {
            WriteULongLE( (ulong)value, data, start );
        }

        /// <summary>
        /// Reads a 64-bit signed integer from a byte array storing the value in little-endian
        /// format. 8 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static long ReadLongLE( byte[] data, int start )
        {
            return (long)ReadULongLE( data, start );
        }

        /// <summary>
        /// Writes a 64-bit unsigned integer to the given byte array in little-endian format. 8 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteULongLE( ulong value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 8 );

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
            data[start + 2] = (byte)( value >> 16 );
            data[start + 3] = (byte)( value >> 24 );

            data[start + 4] = (byte)( value >> 32 );
            data[start + 5] = (byte)( value >> 40 );
            data[start + 6] = (byte)( value >> 48 );
            data[start + 7] = (byte)( value >> 56 );
        }

        /// <summary>
        /// Reads a 64-bit signed integer from a byte array storing the value in little-endian
        /// format. 8 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static ulong ReadULongLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 8 );
            ulong value;

            value = data[start + 0];
            value |= (ulong)data[start + 1] << 8;
            value |= (ulong)data[start + 2] << 16;
            value |= (ulong)data[start + 3] << 24;

            value |= (ulong)data[start + 4] << 32;
            value |= (ulong)data[start + 5] << 40;
            value |= (ulong)data[start + 6] << 48;
            value |= (ulong)data[start + 7] << 56;

            return value;
        }

        /// <summary>
        /// Writes the 128-bit decimal value to the given byte array in little-endian format. 16
        /// bytes are written to the array.
        /// </summary>
        /// <remarks>
        /// The native, portable serialization format of the decimal type as specified by Microsoft
        /// is to return four 32-bit signed integers. This implementation simply serializes decimals
        /// by serializing those 4 32-bit integers in little endian format as if they were any other
        /// 32-bit signed integer.
        /// </remarks>
        /// <param name="value">The decimal to serialize.</param>
        /// <param name="data">The byte array to store the decimal's value in.</param>
        /// <param name="start">The first index in the byte array to write to.</param>
        public static void WriteDecimalLE( decimal value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 16 );

            int[] decimalBits = decimal.GetBits( value );

            for( int i = 0; i < 4; i++ )
            {
                WriteIntLE( decimalBits[i], data, start + i * 4 );
            }
        }

        /// <summary>
        /// Reads a 128-bit decimal value from a byte array storing the value in little endian
        /// format. 16 bytes are read from the array.
        /// </summary>
        /// <remarks>
        /// The native, portable serialization format of the decimal type as specified by Microsoft
        /// is to return four 32-bit signed integers. This implementation simply deserializes
        /// decimals by deserializing those 4 32-bit integers in little endian format as if they were
        /// any integer.
        /// </remarks>
        /// <param name="value">The decimal to deserialize.</param>
        /// <param name="data">The byte array to read the decimal's value from.</param>
        /// <param name="start">The first index in the byte array to read from.</param>
        public static decimal ReadDecimalLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 16 );

            int[] decimalBits = new int[4];

            for( int i = 0; i < 4; i++ )
            {
                decimalBits[i] = ReadIntLE( data, start + i * 4 );
            }

            return new decimal( decimalBits );
        }

        /// <summary>
        /// Writes the 64-bit double floating-point value to the given byte array in little-endian format. 8
        /// bytes are written to the array.
        /// </summary>
        /// <param name="value">The decimal to serialize.</param>
        /// <param name="data">The byte array to store the decimal's value in.</param>
        /// <param name="start">The first index in the byte array to write to.</param>
        public static void WriteDoubleLE( double value, byte[] buffer, int start )
        {
            // It's important to state that doing this gives us a double's bytes in the native word order.
            // By calling WriteLongLE, we guarantee we write the native-word-ordered bytes in the
            // little endian format.
            var toy = new DoubleToy() { DoubleValue = value };

            WriteLongLE( toy.LongValue, buffer, start );
        }

        /// <summary>
        /// Reads a 64-bit double floating-point value from a byte array storing the value in little endian
        /// format. 8 bytes are read from the array.
        /// </summary>
        /// <param name="value">The decimal to deserialize.</param>
        /// <param name="data">The byte array to read the decimal's value from.</param>
        /// <param name="start">The first index in the byte array to read from.</param>
        public static double ReadDoubleLE( byte[] buffer, int start )
        {
            CheckReadSize( buffer, start, 8 );
            var toy = new DoubleToy() { LongValue = ReadLongLE( buffer, start ) };

            return toy.DoubleValue;
        }

        private static void CheckWriteSize( byte[] data, int start, int neededSize )
        {
            if( data.Length - start < neededSize )
            {
                throw new ArgumentException( "The provided array is not large enough to read the value." );
            }
        }

        private static void CheckReadSize( byte[] data, int start, int neededSize )
        {
            if( data.Length - start < neededSize )
            {
                throw new ArgumentException( "The provided array is not large enough to contain the value to read." );
            }
        }

        [StructLayout( LayoutKind.Explicit )]
        private struct DoubleToy
        {
            [FieldOffset( 0 )]
            public double DoubleValue;

            [FieldOffset( 0 )]
            public long LongValue;
        }
    }
}