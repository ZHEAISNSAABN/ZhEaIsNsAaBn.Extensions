using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZhEaIsNsAaBn.Utilities;

/// <summary>
/// Serializes <see cref="Severity"/> as its raw <see cref="Severity.Value"/> string.
/// </summary>
internal sealed class SeverityJsonConverter : JsonConverter<Severity>
{
    /// <inheritdoc />
    public override Severity Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        if ( reader.TokenType == JsonTokenType.Null )
        {
            return Severity.Unknown;
        }

        if ( reader.TokenType != JsonTokenType.String )
        {
            throw new JsonException( "Severity must be a JSON string." );
        }

        return Severity.From( reader.GetString() );
    }

    /// <inheritdoc />
    public override void Write( Utf8JsonWriter writer, Severity value, JsonSerializerOptions options )
        => writer.WriteStringValue( value?.Value ?? string.Empty );
}
