// App startup
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Business Logic: Configure Microsoft Entra ID authentication.
// Loads AzureAd settings from configuration and sets up OpenID Connect authentication.
builder.Services.AddAuthentication("OpenIdConnect")
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Business Logic: Configure authorization for the application.
// Ensures endpoints are protected by authentication when using [Authorize] attribute.
builder.Services.AddAuthorization();

builder.Services.Configure<EnterpriseAssistant.Infrastructure.Configuration.AzureOpenAIOptions>(
    builder.Configuration.GetSection("AzureOpenAI"));

builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.AI.KernelFactory>();
builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.AI.AzureOpenAIService>();
builder.Services.AddSingleton<EnterpriseAssistant.Core.Interfaces.IAssistantOrchestrator, EnterpriseAssistant.Web.Services.AssistantOrchestrator>();
builder.Services.AddSingleton<EnterpriseAssistant.Core.Interfaces.IKnowledgeProvider, EnterpriseAssistant.Infrastructure.Knowledge.MockKnowledgeProvider>();
builder.Services.AddSingleton<EnterpriseAssistant.Plugins.KnowledgeSearchPlugin>();
builder.Services.AddSingleton<EnterpriseAssistant.Core.Interfaces.IConversationMemoryService, EnterpriseAssistant.Infrastructure.Persistence.InMemoryConversationService>();
builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.Authentication.IUserContextService, EnterpriseAssistant.Infrastructure.Authentication.UserContextService>();

builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.Data.InMemoryIssueRepository>();
builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.Data.InMemoryPocRepository>();
builder.Services.AddSingleton<EnterpriseAssistant.Infrastructure.Data.InMemoryWeekendExclusionRepository>();
builder.Services.AddSingleton<EnterpriseAssistant.Plugins.IssuePlugin>();
builder.Services.AddSingleton<EnterpriseAssistant.Plugins.PocPlugin>();
builder.Services.AddSingleton<EnterpriseAssistant.Plugins.WeekendExclusionPlugin>();
builder.Services.AddSingleton<EnterpriseAssistant.Web.Services.PluginRegistry>();

builder.Services.AddControllers();

var app = builder.Build();

// Business Logic: Register available workflow plugins for discovery.
var pluginRegistry = app.Services.GetRequiredService<EnterpriseAssistant.Web.Services.PluginRegistry>();
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.KnowledgeSearchPlugin>());
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.IssuePlugin>());
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.PocPlugin>());
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.WeekendExclusionPlugin>());

// Business Logic: Add authentication and authorization middleware.
// Middleware order matters: authentication before authorization before endpoint mapping.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Enterprise Assistant V1");

app.Run();

public partial class Program { }