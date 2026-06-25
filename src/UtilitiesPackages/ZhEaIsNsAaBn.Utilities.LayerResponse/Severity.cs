#region Usings

using System.Text.Json.Serialization;

#endregion

namespace ZhEaIsNsAaBn.Utilities;

/// <summary>
/// Represents an error severity level value object.
/// </summary>
[ JsonConverter( typeof( SeverityJsonConverter ) ) ]
public sealed record Severity
{
    /// <summary>
    /// High severity level.
    /// </summary>
    public static readonly Severity High = new(nameof(High));

    /// <summary>
    /// Low severity level.
    /// </summary>
    public static readonly Severity Low = new(nameof(Low));

    /// <summary>
    /// Medium severity level.
    /// </summary>
    public static readonly Severity Medium = new(nameof(Medium));

    /// <summary>
    /// Unknown or unspecified severity level.
    /// </summary>
    public static readonly Severity Unknown = new(string.Empty);

    [ JsonConstructor ]
    private Severity( string value ) => Value = value;

    /// <summary>
    /// Gets the raw severity value used for serialization.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Deconstructs the severity into its raw value.
    /// </summary>
    /// <param name="Value">The raw severity value.</param>
    public void Deconstruct( out string Value ) { Value = this.Value; }

    /// <summary>
    /// Creates a <see cref="Severity"/> from a raw value.
    /// </summary>
    /// <param name="value">Raw severity value.</param>
    /// <returns><see cref="Unknown"/> when value is null or empty; otherwise a new severity instance.</returns>
    public static Severity From( string? value ) => string.IsNullOrEmpty( value ) ? Unknown : new Severity( value );
}