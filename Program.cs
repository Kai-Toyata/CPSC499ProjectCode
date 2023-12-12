var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
switch(Constants.LIMIT_METHOD){
    case LimitMethod.FIXED_WINDOW:
        builder.Services.AddSingleton<LimitService,FixedWindowService>();
        break;
    case LimitMethod.SLIDING_WINDOW:
        builder.Services.AddSingleton<LimitService,SlidingWindowService>();
        break;
    // case LimitMethod.TOKEN_BUCKET:
    //     builder.Services.AddSingleton<LimitService,FixedWindowService>();
    //     break;
    // case LimitMethod.LEAKY_BUCKET:
    //     builder.Services.AddSingleton<LimitService,FixedWindowService>();
    //     break;
}
// builder.Services.AddSingleton<LimitService,SlidingWindowService>();
builder.Services.AddHostedService<LimitManagementService>();
builder.Services.AddSingleton<SimService>();
builder.Services.AddHostedService<ResourceManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseMiddleware<RateLimitMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

