using Newtonsoft.Json;

namespace VectorIndexScenarioSuite
{
    internal class EmbeddingDocumentBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "embedding")]
        public float[] Embedding { get; }


        [JsonProperty(PropertyName = "ranking")]
        public uint Ranking { get; set; }

        [JsonProperty(PropertyName = "pct")]
        public uint Pct { get; set; }

        public EmbeddingDocumentBase(string id, float[] embedding)
        {
            this.Id = id;
            this.Embedding = embedding;
            this.Ranking = uint.Parse(id);
            this.Pct = uint.Parse(id) % 100;
        }
    }
}
