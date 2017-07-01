using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NPA.XF.Http
{
    public class JsonContent : ByteArrayContent
    {
        public JsonContent( JObject message, Encoding encoding = null ) : base( GetByteArray( message.ToString(), encoding ) )
        {
            Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( "application/json" )
            {
                CharSet = ( encoding ?? Encoding.UTF8 ).WebName
            };
        }

        public JsonContent( object message, Encoding encoding = null ) : base( GetByteArray( message, encoding ) )
        {
            Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( "application/json" )
            {
                CharSet = ( encoding ?? Encoding.UTF8 ).WebName
            };
        }

        static byte[] GetByteArray( object content, Encoding encoding = null )
        {
            return GetByteArray( JsonConvert.SerializeObject( content ).ToString() );
        }

        static byte[] GetByteArray( string content, Encoding encoding = null )
        {
            return ( encoding ?? Encoding.UTF8 ).GetBytes( content );
        }
    }
}
