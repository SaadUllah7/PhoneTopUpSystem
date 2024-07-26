using Microsoft.EntityFrameworkCore;
using PhoneTopUpSystem.API;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneTopUpSystem.API", Version = "v1" });
});


builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ITopUpService, TopUpService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();


builder.Services.AddDbContext<TopUpDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneTopUpSystem.API v1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
