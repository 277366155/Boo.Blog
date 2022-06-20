using Boo.Blog.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("any",
        builder =>
        {
            builder.AllowAnyMethod(); 
            builder.AllowAnyHeader(); 
            builder.AllowAnyOrigin();
            //builder.AllowCredentials();
        });
});

var app = builder.Build();

app.UseMiddleware<SerilogHandlerMiddleware>();
app.UseCors("any");
//app.UseMiddleware<CorsMiddleware>();
app.UseHealthChecks("/health");
app.Run();
