namespace NPAInspectionWriter.Caching
{
    public static class CachingAgent
    {
        private static IAsyncCacheProvider _provider;
        public static IAsyncCacheProvider Provider
        {
            get
            {
                if( _provider == null )
                    _provider = new SimpleCacheProvider();
                return _provider;
            }
            set { _provider = value; }
        }

        public static bool IsProviderAvailable { get { return Provider != null; } }
    }
}
