using System;
using System.Runtime.Serialization;

namespace TlvDemo.TlvApi
{
    public class UnknownWireTypeException : Exception
    {
        public UnknownWireTypeException()
        {
        }

        public UnknownWireTypeException( string message ) : base( message )
        {
        }

        public UnknownWireTypeException( string message, Exception innerException ) : base( message, innerException )
        {
        }

        public UnknownWireTypeException( string message, int wireTypeId ) : base( message )
        {
            this.WireTypeId = wireTypeId;
        }

        public UnknownWireTypeException( string message, int wireTypeId, Exception innerException ) : base( message, innerException )
        {
            this.WireTypeId = wireTypeId;
        }

        protected UnknownWireTypeException( SerializationInfo info, StreamingContext context ) : base( info, context )
        {
            this.WireTypeId = info.GetInt32( "wireTypeId" );
        }

        public int WireTypeId { get; private set; }

        public override void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            if( info == null )
            {
                throw new ArgumentNullException( nameof( info ) );
            }

            base.GetObjectData( info, context );

            info.AddValue( "wireTypeId", this.WireTypeId, typeof( int ) );
        }
    }
}