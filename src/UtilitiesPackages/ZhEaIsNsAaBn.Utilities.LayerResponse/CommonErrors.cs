namespace ZhEaIsNsAaBn.Utilities;

/// <summary>
/// Provides reusable error instances for common cross-layer failures.
/// </summary>
public static class CommonErrors
{
    /// <summary>
    /// Represents a resource-not-found failure.
    /// </summary>
    public static readonly Error NotFound = new(nameof(NotFound), "Unknown", nameof(NotFound), nameof(NotFound));

    /// <summary>
    /// Represents a canceled-operation failure.
    /// </summary>
    public static readonly Error OperationHasBeenCanceled = new(nameof(OperationHasBeenCanceled), "Unknown",
                                                                "The operation has been canceled",
                                                                "The operation has been canceled");

    /// <summary>
    /// Represents an undocumented or unclassified failure.
    /// </summary>
    public static readonly Error UNDOCUMENTED = new(nameof(UNDOCUMENTED),
                                                    "Unknown", nameof(UNDOCUMENTED), nameof(UNDOCUMENTED));

    /// <summary>
    /// Represents a generic unknown failure.
    /// </summary>
    public static readonly Error UnknownError = new(nameof(UnknownError), "Unknown", "Unknown Error", "Unknown Error");
}
