// App startup
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EnterpriseAssistant.Infrastructure.Configuration.AzureOpenAIOptions>(
    builder.Configuration.GetSection("AzureOpenAI"));

builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.AI.KernelFactory>();
builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.AI.AzureOpenAIService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Enterprise Assistant V1");

app.Run();

public partial class Program { }