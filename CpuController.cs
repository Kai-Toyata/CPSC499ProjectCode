using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CpuController : ControllerBase
{
    private readonly SimService _simService;
    private readonly LimitService _limitService;

    public CpuController(SimService simService, LimitService limitService){
        _simService = simService;
        _limitService = limitService;
    }

    [HttpGet]
    public IActionResult GetCpuUsage(){
        return Ok(_simService.GetCpuUsage());
    }

    [HttpPost]
    public IActionResult AddCpuUsage([FromQuery] int amount){
        return Ok(_simService.GetCpuUsage());
    }
}