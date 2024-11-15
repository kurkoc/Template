using System.Text.Json;
using System.Text.Json.Serialization;

namespace TemplateSolution.Infrastructure.Json;

public class NumberToStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out long longValue))
            {
                return longValue.ToString();
            }
            else if (reader.TryGetInt32(out int intValue))
            {
                return intValue.ToString();
            }
            else if (reader.TryGetDecimal(out decimal decimalValue))
            {
                return decimalValue.ToString();
            }
            else if (reader.TryGetDouble(out double doubleValue))
            {
                return doubleValue.ToString();
            }
        }

        return JsonSerializer.Deserialize<string>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}