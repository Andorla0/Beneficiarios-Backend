using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Beneficiarios.API.Serialization;

/// <summary>
/// Converts DateOnly values to and from JSON format using the "yyyy-MM-dd" format.
/// </summary>
public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    /// <summary>
    /// The standard date format used for serialization and deserialization.
    /// </summary>
    private const string Format = "yyyy-MM-dd";

    /// <summary>
    /// Deserializes a JSON string to a DateOnly object.
    /// </summary>
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Extract the string value from the JSON reader
        var dateString = reader.GetString();

        // Return default value if the string is null or empty
        if (string.IsNullOrWhiteSpace(dateString))
        {
            return default;
        }

        // Attempt to parse using the exact format
        if (DateOnly.TryParseExact(dateString, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }

        // Fallback: parse with flexible format if exact parsing fails
        return DateOnly.Parse(dateString, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Serializes a DateOnly object to a JSON string.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
}
