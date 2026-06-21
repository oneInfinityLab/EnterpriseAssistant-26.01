using EnterpriseAssistant.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using System.Security.Claims;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

var azureAdSection = builder.Configuration.GetSection("AzureAd");
var useEntraId = !string.IsNullOrWhiteSpace(azureAdSection["Instance"])
    && !string.IsNullOrWhiteSpace(azureAdSection["TenantId"])
    && !string.IsNullOrWhiteSpace(azureAdSection["ClientId"]);

if (useEntraId)
{
    builder.Services.AddAuthentication("OpenIdConnect")
        .AddMicrosoftIdentityWebApp(azureAdSection);
}
else
{
    builder.Services.AddAuthentication(DemoAuthenticationHandler.SchemeName)
        .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>(
            DemoAuthenticationHandler.SchemeName,
            _ => { });
}

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
builder.Services.AddSingleton<DashboardService>();
builder.Services.AddSingleton<ActivityFeedService>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUi",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Business Logic: Register available workflow plugins for discovery.
var pluginRegistry = app.Services.GetRequiredService<EnterpriseAssistant.Web.Services.PluginRegistry>();
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.KnowledgeSearchPlugin>());
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.IssuePlugin>());
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.PocPlugin>());
pluginRegistry.RegisterPlugin(app.Services.GetRequiredService<EnterpriseAssistant.Plugins.WeekendExclusionPlugin>());

// Business Logic: Add authentication and authorization middleware.
// Middleware order matters: authentication before authorization before endpoint mapping.
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowUi");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");
app.Run();

public partial class Program { }

internal sealed class DemoAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Demo";

    public DemoAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "demo-user"),
            new Claim(ClaimTypes.Name, "Demo User"),
            new Claim(ClaimTypes.Email, "demo@local")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName));
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
