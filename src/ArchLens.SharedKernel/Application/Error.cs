using System.Diagnostics.CodeAnalysis;

namespace ArchLens.SharedKernel.Application;

[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "A null value was provided.");
    public static readonly Error NotFound = new("Error.NotFound", "The requested resource was not found.");
    public static readonly Error Conflict = new("Error.Conflict", "A conflict occurred.");
    public static readonly Error Validation = new("Error.Validation", "A validation error occurred.");
}
