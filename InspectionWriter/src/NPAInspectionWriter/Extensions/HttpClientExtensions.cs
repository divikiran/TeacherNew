using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPA.XF.Http;
using System.Diagnostics;

namespace NPAInspectionWriter.Extensions
{
    public static class HttpClientExtensions
    {
        static readonly HttpMethod patch_method = new HttpMethod( "PATCH" );

        public static async Task<JObject> GetJObjectAsync( this HttpClient client, string requestUri, object requestObject = null, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            try
            {
                var defaultResponse = "{\"message\":\"An unknown error has occured. Please try again. If this problem persists, please contact the closest Code Monkey.\"}";
                var response = await client.GetAsync( requestUri.AddQueryStringParameters( requestObject ), cancellationToken );
                string result = await response.Content.ReadAsStringAsync();
                if( !response.IsSuccessStatusCode )
                {
                    result = $"{{\"message\":\"{response.StatusCode} {result}\"}}";
                    Debug.WriteLine( $"Code {response.StatusCode}: {result}" );
                }

                //var result = await client.GetStringAsync( requestUri.AddQueryStringParameters( requestObject ) );
                Debug.WriteLine( $"Result: {result}" );
                if( string.IsNullOrWhiteSpace( result ) ) result = defaultResponse;
                return JObject.Parse( result );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return null;
            }
        }

        public static async Task<T> GetAsync<T>( this HttpClient client, string requestUri, object requestObject = null, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            //var result = await client.GetStringAsync( requestUri.AddQueryStringParameters( requestObject ) );
            try
            {
                var response = await client.GetAsync( requestUri.AddQueryStringParameters( requestObject ), cancellationToken );

                return JsonConvert.DeserializeObject<T>( await response.Content.ReadAsStringAsync() );
            }
            catch( Exception e )
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public static async Task<T> GetAsync<T>( this HttpClient client, string requestUri, T failedResponse, object requestObject = null, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            var result = await client.GetAsync( requestUri.AddQueryStringParameters( requestObject ), cancellationToken );
            if( !result.IsSuccessStatusCode )
                return failedResponse;

            try
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                Debug.WriteLine( jsonString );

                return JsonConvert.DeserializeObject<T>( jsonString );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return failedResponse;
            }
        }

        public static async Task<T> GetResultAsync<T>( this HttpClient client, string resultKey, string requestUri, object requestObject = null, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            try
            {
                return ( T )Convert.ChangeType( await GetResultAsync( client, resultKey, requestUri, requestObject, cancellationToken ), typeof( T ) );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return default( T );
            }
        }

        public static async Task<string> GetResultAsync( this HttpClient client, string resultKey, string requestUri, object requestObject = null, CancellationToken canellationToken = default( CancellationToken ) )
        {
            try
            {
                Debug.WriteLine( $"Making Get Request for key: {resultKey}" );
                var response = await client.GetJObjectAsync( requestUri, requestObject, canellationToken );
                return response[ resultKey ].ToString();
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return null;
            }
        }

        public static async Task<HttpResponseMessage> PostJsonObjectAsync( this HttpClient client, string requestUri, dynamic jsonObj, CancellationToken canellationToken = default(CancellationToken) )
        {
            try
            {
                var postBody = JsonConvert.SerializeObject( jsonObj );
                var content = new StringContent( postBody, Encoding.UTF8, "application/json" );
                return await client.PostAsync( requestUri, content, canellationToken );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                throw;
            }
        }

        /// <summary>
        /// Posts dictionary of values
        /// </summary>
        /// <typeparam name="TReturn">Return Type</typeparam>
        /// <typeparam name="TKey">Dictionary Key Type</typeparam>
        /// <typeparam name="TValue">Dictionary Value Type</typeparam>
        /// <param name="client">HttpClient</param>
        /// <param name="requestUri">Uri to request from</param>
        /// <param name="payload">Form values to request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessWebException">Throws <see cref="UnauthorizedAccessWebException"/> if the username and/or password are invalid</exception>
        public static Task<TReturn> PostForObjectAsync<TReturn,TKey,TValue>( this HttpClient client, string requestUri, IDictionary<TKey,TValue> payload, CancellationToken cancellationToken = default(CancellationToken) )
        {
            return PostForObjectAsync<TReturn>( client, requestUri, payload as object, cancellationToken );
        }

        public static Task<T> PostForObjectAsync<T>( this HttpClient client, string requestUri, object payload, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            return client.PostForObjectAsync<T>( new Uri( requestUri ), payload, cancellationToken );
        }

        public static async Task<T> PostForObjectAsync<T>( this HttpClient client, Uri requestUri, object payload, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            try
            {
                var message = await client.PostAsync( requestUri, new JsonContent( payload ), cancellationToken );
                if( message.StatusCode == HttpStatusCode.Unauthorized )
                    throw new UnauthorizedAccessWebException();

                return JsonConvert.DeserializeObject<T>( await message.Content.ReadAsStringAsync() );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                throw;
            }
        }

        public static Task<HttpResponseMessage> PostAsync( this HttpClient client, string requestUri, object payload, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            PostAsync( client, new Uri( requestUri ), payload, cancellationToken );

        public static Task<HttpResponseMessage> PostAsync( this HttpClient client, Uri requestUri, object payload, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            client.PostAsync( requestUri, new JsonContent( payload ), cancellationToken );

        public static async Task<string> GetStringAsync( this HttpClient client, string requestUri, CancellationToken cancellationToken )
        {
            var response = await client.GetAsync( requestUri, cancellationToken );
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<HttpResponseMessage> PatchAsync( this HttpClient client, string address, HttpContent content ) =>
            await client.SendAsync( new HttpRequestMessage( patch_method, address ) { Content = content } );

        public static async Task<HttpResponseMessage> PatchAsync( this HttpClient client, string address, string content ) =>
            await client.SendAsync( new HttpRequestMessage( patch_method, address ) { Content = new StringContent( content ) } );

        public static async Task<HttpResponseMessage> PatchAsync<T>( this HttpClient client, string address, T content ) where T : class =>
            await client.SendAsync( new HttpRequestMessage( patch_method, address ) { Content = new JsonContent( content ) } );

        public static async Task<HttpResponseMessage> PatchAsync( this HttpClient client, string address, JObject content ) =>
            await client.SendAsync( new HttpRequestMessage( patch_method, address ) { Content = new JsonContent( content ) } );
    }
}
