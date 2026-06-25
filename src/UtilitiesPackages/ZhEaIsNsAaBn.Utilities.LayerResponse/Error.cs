#region Usings

using System.Text.Json.Serialization;

#endregion

namespace ZhEaIsNsAaBn.Utilities;

/// <summary>
/// Represents a domain/application error that can be transferred between layers.
/// </summary>
public sealed record Error
{
    /// <summary>
    /// Sentinel error value that indicates the absence of an error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty, string.Empty, string.Empty, Severity.Unknown);

    /// <summary>
    /// Initializes a new instance of <see cref="Error"/>.
    /// </summary>
    /// <param name="code">Machine-friendly error code.</param>
    /// <param name="messageAr">Localized Arabic message.</param>
    /// <param name="message">Default or English message.</param>
    /// <param name="severity">Error severity. Defaults to <see cref="Severity.Medium"/> when null.</param>
    /// <param name="exception">Associated exception when available.</param>
    /// <param name="additionalData">Additional contextual data for troubleshooting.</param>
    public Error(
            string code, string messageAr, string message, Severity? severity = null, Exception? exception = null,
            IReadOnlyDictionary<string, object>? additionalData = null
        ) : this( code, null, messageAr, message, severity, exception, additionalData ) { }

    /// <summary>
    /// Initializes a new instance of <see cref="Error"/>.
    /// </summary>
    /// <param name="code">Machine-friendly error code.</param>
    /// <param name="type">Error category/type. Defaults to <c>General</c> when null.</param>
    /// <param name="messageAr">Localized Arabic message.</param>
    /// <param name="message">Default or English message.</param>
    /// <param name="severity">Error severity. Defaults to <see cref="Severity.Medium"/> when null.</param>
    /// <param name="exception">Associated exception when available.</param>
    /// <param name="additionalData">Additional contextual data for troubleshooting.</param>
    [ JsonConstructor ]
    public Error(
            string     code,             string? type, string messageAr, string message, Severity? severity = null,
            Exception? exception = null, IReadOnlyDictionary<string, object>? additionalData = null
        )
    {
        Code           = code;
        Type           = type ?? "General";
        MessageAR      = messageAr;
        Message        = message;
        Severity       = severity ?? Severity.Medium;
        Exception      = exception;
        AdditionalData = additionalData ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets the machine-friendly error code.
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// Gets the error category/type.
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// Gets the localized Arabic message.
    /// </summary>
    public string MessageAR { get; init; }

    /// <summary>
    /// Gets the default or English message.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets the severity level for this error.
    /// </summary>
    public Severity Severity { get; init; }

    /// <summary>
    /// Gets the associated exception.
    /// </summary>
    [ JsonIgnore ]
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets additional contextual data related to this error.
    /// </summary>
    [ JsonIgnore ]
    public IReadOnlyDictionary<string, object> AdditionalData { get; init; }

    /// <summary>
    /// Creates a standardized unknown-error instance from an exception.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="additionalData">Optional contextual key/value data.</param>
    /// <returns>An <see cref="Error"/> representing an unknown failure.</returns>
    public static Error Unknown( Exception exception, IReadOnlyDictionary<string, object>? additionalData = null ) =>
        new(nameof(Unknown), string.Empty, "حدث خطأ غير معروف", "An unknown error occurred", Severity.Unknown,
            exception, additionalData);

    /// <summary>
    /// Deconstructs the error into primitive values.
    /// </summary>
    /// <param name="code">The machine-friendly error code.</param>
    /// <param name="nameAR">The localized Arabic message.</param>
    /// <param name="nameEN">The default or English message.</param>
    /// <param name="severity">The severity level.</param>
    /// <param name="exception">The associated exception if present.</param>
    /// <param name="additionalData">Additional contextual key/value data.</param>
    public void Deconstruct(
            out string code, out string nameAR, out string nameEN, out Severity severity, out Exception? exception,
            out IReadOnlyDictionary<string, object> additionalData
        )
    {
        code           = Code;
        nameAR         = MessageAR;
        nameEN         = Message;
        severity       = Severity;
        exception      = Exception;
        additionalData = AdditionalData;
    }
}
