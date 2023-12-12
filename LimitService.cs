
public enum LimitMethod{
    TOKEN_BUCKET,
    LEAKY_BUCKET,
    FIXED_WINDOW,
    SLIDING_WINDOW
}

public abstract class LimitService{

    public abstract void AddRequest();
    public abstract void Update();
    public abstract bool CanRequest();

}

public class SlidingWindowService : LimitService{
    private Queue<DateTime> requests = new Queue<DateTime>();
    public override void AddRequest(){
        requests.Enqueue(DateTime.Now);
    }
    public override void Update(){
        for(int i=0;i<requests.Count;i++){
            TimeSpan ts = requests.Peek() - DateTime.Now;
            if(ts>Constants.WINDOW_SIZE) requests.Dequeue();
            else break;
        }
    }

    public override bool CanRequest(){
        if(requests.Count>=Constants.WINDOW_CAPACITY) return false;
        return true;
    }

}
public class FixedWindowService : LimitService{
    private int requestCount = 0;
    private DateTime lastWindowStart = DateTime.Now;
    public override void AddRequest(){
        requestCount++;
    }
    public override void Update(){
        // Console.WriteLine(DateTime.Now - lastWindowStart);
        if(DateTime.Now - lastWindowStart >= Constants.WINDOW_SIZE){
            lastWindowStart = DateTime.Now;
            requestCount=0;    
        }
    }

    public override bool CanRequest()
    {
        if(requestCount>=Constants.WINDOW_CAPACITY) return false;
        return true;
    }

}
public class TokenBucketService : LimitService{
    private int currentTokensInBucket = Constants.BUCKET_CAPACITY;
    public override void AddRequest(){
        currentTokensInBucket++;
    }
    public override void Update(){//adds request to the bucket
        if(currentTokensInBucket<Constants.BUCKET_CAPACITY) currentTokensInBucket++;
    }

    public override bool CanRequest()
    {
        if(currentTokensInBucket>0) return true;
        return false;
    }

}
public class LeakyBucketService : LimitService{
    private static int requestNumber = 0;
    private int lastFulfilledRequest = 0;
    private bool allowRequest = false;
    private int currentTokensInBucket = 0;
    public override void AddRequest(){
        lastFulfilledRequest++;
        allowRequest=false;
    }

    public int GetRequestNumber(){
        requestNumber++;
        return requestNumber;
    }
    public override void Update(){//leaks the bucket (aka fulfills a request)
        if(currentTokensInBucket>0) {
            currentTokensInBucket--;
            allowRequest=true;
        }
    }

    public bool IsMyTurn(int requestNumber){
        if(lastFulfilledRequest==requestNumber-1 && allowRequest) return true;
        return false;
    }

    public override bool CanRequest()
    {
        if(currentTokensInBucket>=Constants.BUCKET_CAPACITY) return false;
        return true;
    }

}

public class LimitManagementService : BackgroundService
{
    private readonly LimitService _limitService;

    public LimitManagementService(LimitService limitService)
    {
        _limitService = limitService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _limitService.Update();
            await Task.Delay(Constants.LIMIT_UPDATE_DELAY, stoppingToken);
        }
    }
}