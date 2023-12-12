public static class Constants{
    //rate limiting method
    public static readonly LimitMethod LIMIT_METHOD = LimitMethod.FIXED_WINDOW;
    public static readonly TimeSpan LIMIT_UPDATE_DELAY = TimeSpan.FromMilliseconds(100);
    //system memory setting
    public static readonly int MAX_MEMORY = 1;
    public static readonly DataUnit MAX_MEMORY_SIZE = DataUnit.Gb;
    //for memory/cpu usage removal
    public static readonly int MEM_TO_REMOVE_AMOUNT = 10;
    public static readonly int CPU_TO_REMOVE_AMOUNT = 1;
    public static readonly TimeSpan RESOURCE_RELEASE_INTERVAL = TimeSpan.FromMilliseconds(100); //interval for memory and cpu to be removed
    public static readonly DataUnit MEM_TO_REMOVE_UNIT = DataUnit.Mb;

    // for rate limiting
    //window & sliding window
    public static readonly TimeSpan WINDOW_SIZE = TimeSpan.FromSeconds(10); 
    public static readonly int WINDOW_CAPACITY = 100;
    //bucket and leaky bucket
    public static readonly int BUCKET_CAPACITY = 100;
    public static readonly int BUCKET_FILL_RATE = 10; //tokens per second
    public static readonly int BUCKET_LEAK_RATE = 10; //tokens per second

    //status codes
    public static readonly int NO_CPU_CODE = 500;
    public static readonly int NO_MEM_CODE = 507;
    public static readonly int RATE_LIMITTED_CODE = 429;
}