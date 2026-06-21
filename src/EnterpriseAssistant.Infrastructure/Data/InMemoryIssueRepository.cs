namespace EnterpriseAssistant.Infrastructure.Data;

using EnterpriseAssistant.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Business Logic: In-memory storage for issue workflow entities.
/// Stores issues for the current application runtime only.
/// </summary>
public sealed class InMemoryIssueRepository
{
    private readonly List<IssueResponse> _issues = new();

    public IssueResponse CreateIssue(IssueRequest request, string createdBy)
    {
        var issue = new IssueResponse
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = "Open",
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };

        _issues.Add(issue);
        return issue;
    }

    public IssueResponse? GetIssueById(string id)
    {
        return _issues.FirstOrDefault(i => i.Id == id);
    }

    public IReadOnlyList<IssueResponse> GetIssuesByCreator(string createdBy)
    {
        return _issues.Where(i => i.CreatedBy == createdBy).ToList();
    }
}
