namespace EnterpriseAssistant.Core.Models;

using System;

/// <summary>
/// Represents the result of a POC workflow operation.
/// </summary>
public sealed class PocResponse
{
    public string Id { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Customer { get; init; } = string.Empty;

    public string BusinessJustification { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string RequestedBy { get; init; } = string.Empty;

    public DateTime RequestedDate { get; init; }
}