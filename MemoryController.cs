using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MemoryController : ControllerBase
{
    private readonly SimService _simService;
    private readonly LimitService _limitService;

    public MemoryController(SimService simService, LimitService limitService){
        _simService = simService;
        _limitService = limitService;
    }

    [HttpGet("kb")]
    public IActionResult GetMemKb(){
        return Ok(_simService.GetMemUsage());
    }
    [HttpGet("mb")]
    public IActionResult GetMemMb(){
        return Ok(_simService.GetMemUsage()/1024);
    }
    [HttpGet("gb")]
    public IActionResult GetMemGb(){
        return Ok(_simService.GetMemUsage()/1024/1024);
    }

    [HttpPost("kb")]
    public IActionResult AddKilobyte([FromQuery] int amount)
    {
        return Ok(Add(amount,DataUnit.Kb));
    }
    [HttpPost("mb")]
    public IActionResult AddMegabyte([FromQuery] int amount)
    {
        return Ok(Add(amount,DataUnit.Mb));
    }
    [HttpPost("gb")]
    public IActionResult AddGigabyte([FromQuery] int amount)
    {
        return Ok(Add(amount,DataUnit.Gb));
    }

    private string Add(int amount, DataUnit ds){
        _simService.ComputeMem(amount,ds);
        long currentMemUsage = _simService.GetMemUsage();
        return "Current Memory Usage: " + (currentMemUsage / (int) ds )+ ds.ToString();

    }

}
