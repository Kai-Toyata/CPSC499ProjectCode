public enum DataUnit{
    Kb = 1,
    Mb = 1024,
    Gb = 1024*1024

    
}

public class SimService
{
    static int currentMemoryUsage = 0;//bytes
    static int currentCpuPercentage = 0;

    //returns if the memory is overflowing
    public void ComputeMem(int memoryAmount, DataUnit memorySize)
    {
        currentMemoryUsage+=memoryAmount * (int) memorySize;
    }

    public bool HasMemoryRemaining(){
        if(currentMemoryUsage<Constants.MAX_MEMORY * (int)Constants.MAX_MEMORY_SIZE) return true;
        return false;
    }
    public bool HasCPURemaining(){
        if(currentCpuPercentage<100) return true;
        return false;
    }

    //returns true if the memory is overflowing
    public void ComputeCPU(int cpuAmount){
        currentCpuPercentage+=cpuAmount;
    }

    public void RemoveMemory(int memoryAmount, DataUnit memorySize){
        currentMemoryUsage=Math.Max(0,currentMemoryUsage - (memoryAmount * (int) memorySize));
    }

    public void RemoveCpuUsage(int cpuUsagePercent){
        currentCpuPercentage=Math.Max(0,currentCpuPercentage - cpuUsagePercent);
    }

    public int GetCpuUsage(){
        return currentCpuPercentage;
    }

    public int GetMemUsage(){
        return currentMemoryUsage;
    }
}

public class ResourceManagementService : BackgroundService
{
    private readonly SimService _simService;

    public ResourceManagementService(SimService simService)
    {
        _simService = simService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _simService.RemoveMemory(Constants.MEM_TO_REMOVE_AMOUNT,Constants.MEM_TO_REMOVE_UNIT);
            _simService.RemoveCpuUsage(Constants.CPU_TO_REMOVE_AMOUNT);
            await Task.Delay(Constants.RESOURCE_RELEASE_INTERVAL, stoppingToken);
        }
    }
}
