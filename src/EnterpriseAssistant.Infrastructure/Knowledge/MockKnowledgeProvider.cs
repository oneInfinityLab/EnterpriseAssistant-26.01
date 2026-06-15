namespace EnterpriseAssistant.Infrastructure.Knowledge;

using EnterpriseAssistant.Core.Interfaces;
using EnterpriseAssistant.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Business Logic: Mock implementation of the knowledge provider for enterprise document search.
/// Returns predefined enterprise documents for operations, innovation, change management, 
/// infrastructure, and procurement domains.
/// 
/// Supports basic keyword-based search by matching against document titles, categories, and content.
/// Returns all matching documents sorted by relevance.
/// 
/// Future enhancements:
/// - Integration with SharePoint or Dataverse for real document retrieval
/// - Vector database search for semantic similarity
/// - Azure AI Search for enterprise-scale knowledge management
/// </summary>
public sealed class MockKnowledgeProvider : IKnowledgeProvider
{
    private readonly List<KnowledgeDocument> _enterpriseDocuments;

    public MockKnowledgeProvider()
    {
        // Business Logic: Initialize the mock knowledge base with representative enterprise documents.
        // These serve as test fixtures for knowledge search functionality.
        _enterpriseDocuments = new List<KnowledgeDocument>
        {
            new KnowledgeDocument
            {
                Id = "doc-001",
                Title = "How to Raise an Issue",
                Category = "Operations",
                Content = "This guide explains the process for raising operational issues in the enterprise system."
            },
            new KnowledgeDocument
            {
                Id = "doc-002",
                Title = "How to Request a POC",
                Category = "Innovation",
                Content = "Instructions for submitting a Proof of Concept (POC) request for new technology evaluation."
            },
            new KnowledgeDocument
            {
                Id = "doc-003",
                Title = "Weekend Exclusion Process",
                Category = "Change Management",
                Content = "Guidelines for excluding changes during weekend periods to maintain system stability."
            },
            new KnowledgeDocument
            {
                Id = "doc-004",
                Title = "Azure VM Operations Guide",
                Category = "Infrastructure",
                Content = "Comprehensive guide for managing and operating Azure Virtual Machines in the enterprise."
            },
            new KnowledgeDocument
            {
                Id = "doc-005",
                Title = "Ariba Operations Guide",
                Category = "Procurement",
                Content = "Reference documentation for Ariba procurement and supply chain operations."
            }
        };
    }

    /// <summary>
    /// Business Logic: Search the mock knowledge base using keyword matching.
    /// Performs simple case-insensitive search across document titles, categories, and content.
    /// Returns matching documents in the order they are found.
    /// </summary>
    public Task<KnowledgeSearchResult> SearchAsync(string query)
    {
        // Business Logic: Validate the search query is not empty.
        if (string.IsNullOrWhiteSpace(query))
        {
            return Task.FromResult(new KnowledgeSearchResult
            {
                Query = query,
                Results = []
            });
        }

        // Business Logic: Perform case-insensitive keyword search across all document fields.
        // Match documents that contain the query string in title, category, or content.
        var searchResults = _enterpriseDocuments
            .Where(doc =>
                doc.Title.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
                doc.Category.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
                doc.Content.Contains(query, System.StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(new KnowledgeSearchResult
        {
            Query = query,
            Results = searchResults
        });
    }
}
