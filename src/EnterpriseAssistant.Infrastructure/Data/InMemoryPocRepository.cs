namespace EnterpriseAssistant.Infrastructure.Data;

using EnterpriseAssistant.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Business Logic: In-memory storage for proof of concept requests.
/// Stores POC requests for the current application runtime only.
/// </summary>
public sealed class InMemoryPocRepository
{
    private readonly List<PocResponse> _pocs = new();

    public PocResponse CreatePoc(PocRequest request, string requestedBy)
    {
        var poc = new PocResponse
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Customer = request.Customer,
            BusinessJustification = request.BusinessJustification,
            Status = "Submitted",
            RequestedBy = requestedBy,
            RequestedDate = DateTime.UtcNow
        };

        _pocs.Add(poc);
        return poc;
    }

    public PocResponse? GetPocById(string id)
    {
        return _pocs.FirstOrDefault(p => p.Id == id);
    }

    public IReadOnlyList<PocResponse> GetPocsByRequester(string requestedBy)
    {
        return _pocs.Where(p => p.RequestedBy == requestedBy).ToList();
    }
    /// <summary>
    /// Business Logic:
    /// Returns the total number of proof
    /// of concept requests currently stored.
    /// </summary>
    public int GetPocCount()
    {
        return _pocs.Count;
    }
}
