using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/test", async context =>
{
    Console.WriteLine(JsonSerializer.Serialize(context.Request.Headers));
    await Console.Out.WriteLineAsync("X-Forwarded-For��Ϣ��" + JsonSerializer.Serialize(context.Request.Headers["X-Forwarded-For"]));
    await Console.Out.WriteLineAsync("FORWARDED_FOR��Ϣ��" + JsonSerializer.Serialize(context.Request.Headers["FORWARDED_FOR"]));
    context.Response.Headers.Add("content-type","application/json");
    await context.Response.WriteAsync("X-Forwarded-For��Ϣ��" + JsonSerializer.Serialize(context.Request.Headers["X-Forwarded-For"])
                                                        + "\r\nFORWARDED_FOR��Ϣ��" + JsonSerializer.Serialize(context.Request.Headers["FORWARDED_FOR"]));
});

app.Run();
