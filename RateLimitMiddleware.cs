using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly LimitService _limitService;
    private readonly SimService _simService;

    public RateLimitMiddleware(RequestDelegate next,LimitService limitService, SimService simService)
    {
        _next = next;
        _limitService = limitService;
        _simService = simService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request is a POST request
        if (context.Request.Method == "POST")
        {
            if(Constants.LIMIT_METHOD==LimitMethod.LEAKY_BUCKET){
                if(!_limitService.CanRequest()){
                    context.Response.StatusCode = Constants.RATE_LIMITTED_CODE;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Rate Limit Exceeded");
                    return;
                }
                LeakyBucketService leakService = (LeakyBucketService) _limitService;
                int requestNumber = leakService.GetRequestNumber();
                while(!leakService.IsMyTurn(requestNumber)){}
                leakService.AddRequest();

            }
            else{
                //check rate limiting
                if(Constants.LIMIT_METHOD==LimitMethod.FIXED_WINDOW ||
                    Constants.LIMIT_METHOD == LimitMethod.SLIDING_WINDOW) _limitService.Update();
                if(!_limitService.CanRequest()){
                    context.Response.StatusCode = Constants.RATE_LIMITTED_CODE;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Rate Limit Exceeded");
                    return;
                }
                _limitService.AddRequest();
                //check memory & cpu usage
                if(!_simService.HasCPURemaining()){
                    context.Response.StatusCode = Constants.NO_CPU_CODE;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Out of CPU");
                    return;
                }
                if(!_simService.HasMemoryRemaining()){
                    context.Response.StatusCode = Constants.NO_MEM_CODE;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Out of Memory");
                    return;
                }
            }
        }

        // Continue processing the request pipeline
        await _next(context);
    }
}
