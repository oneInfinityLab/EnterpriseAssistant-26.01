// App startup
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Enterprise Assistant V1");

app.Run();

public partial class Program { }