using Newtonsoft.Json;

namespace VectorIndexScenarioSuite
{
    internal class EmbeddingDocumentBase<T>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "embedding")]
        public int[] Embedding { get; }


        [JsonProperty(PropertyName = "ranking")]
        public uint Ranking { get; set; }

        [JsonProperty(PropertyName = "pct")]
        public uint Pct { get; set; }

        public EmbeddingDocumentBase(string id, T[] embedding)
        {
            this.Id = id;
            this.Embedding = Array.ConvertAll(embedding, item => Convert.ToInt32(item));
            this.Ranking = uint.Parse(id);
            this.Pct = uint.Parse(id) % 100;
        }
    }
}
