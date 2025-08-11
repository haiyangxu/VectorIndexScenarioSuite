using Newtonsoft.Json;

namespace VectorIndexScenarioSuite
{
    internal class EmbeddingDocumentBase<T>
    {
         [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "embedding")]
        public Array Embedding { get; }

        public EmbeddingDocumentBase(string id, T[] embedding)
        {
            this.Id = id;
            this.Embedding = ToJsonFriendlyArray(embedding);
        }

        private static Array ToJsonFriendlyArray(T[] embedding)
        {
            // byte[] and sbyte[] are serialized as base64 strings by default. Convert
            // them to an int array so that each element is represented as a number
            // in JSON instead of a base64 encoded string.
            if (typeof(T) == typeof(byte))
            {
                var bytes = (byte[])(object)embedding;
                return Array.ConvertAll(bytes, static b => (int)b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                var sbytes = (sbyte[])(object)embedding;
                return Array.ConvertAll(sbytes, static b => (int)b);
            }

            // For other numeric types (e.g. float), no conversion is required.
            return embedding;
        }
    }
}
