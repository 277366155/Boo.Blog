using AliOSS.WebApi.Service;
using Aliyun.OSS;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(builder.Configuration.GetSection("AliCloudOSSOptions").Get<AliCloudOSSOptions>());
builder.Services.AddScoped(sp =>
{
    var ossOpt = sp.GetService<AliCloudOSSOptions>();
    return new OssClient(ossOpt?.Endpoint, ossOpt?.AccessKeyId, ossOpt?.AccessKeySecret);
});
builder.Services.AddScoped<AliCloudOSSService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
