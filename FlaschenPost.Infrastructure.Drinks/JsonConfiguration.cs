using System.Text.Json;

namespace FlaschenPost.Infrastructure.Drinks
{
    internal static class JsonConfiguration
    {
        public static JsonSerializerOptions GetOptions()
            => new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
    }
}

