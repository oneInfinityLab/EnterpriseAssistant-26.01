namespace EnterpriseAssistant.Core.Interfaces;

using System.Threading.Tasks;
using EnterpriseAssistant.Core.Models;

public interface IKnowledgeProvider
{
    Task<KnowledgeSearchResult> SearchAsync(string query);
}
