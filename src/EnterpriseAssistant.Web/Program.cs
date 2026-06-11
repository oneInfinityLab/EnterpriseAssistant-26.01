// App startup
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/",()=>"Enterprise Assistant V1");
app.Run();