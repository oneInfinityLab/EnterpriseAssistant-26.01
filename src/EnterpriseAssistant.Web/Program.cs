// App startup
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EnterpriseAssistant.Infrastructure.Configuration.AzureOpenAIOptions>(
    builder.Configuration.GetSection("AzureOpenAI"));

builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.AI.KernelFactory>();
builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.AI.AzureOpenAIService>();
builder.Services.AddSingleton<EnterpriseAssistant.Core.Interfaces.IAssistantOrchestrator, EnterpriseAssistant.Web.Services.AssistantOrchestrator>();
builder.Services.AddSingleton<EnterpriseAssistant.Core.Interfaces.IKnowledgeProvider, EnterpriseAssistant.Infrastructure.Knowledge.MockKnowledgeProvider>();
builder.Services.AddSingleton<EnterpriseAssistant.Plugins.KnowledgeSearchPlugin>();
builder.Services.AddSingleton<EnterpriseAssistant.Core.Interfaces.IConversationMemoryService, EnterpriseAssistant.Infrastructure.Persistence.InMemoryConversationService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "Enterprise Assistant V1");

app.Run();

public partial class Program { }