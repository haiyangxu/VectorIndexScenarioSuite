using Newtonsoft.Json;

namespace VectorIndexScenarioSuite
{

    internal class EmbeddingDocumentBase
    {
        // Fixed seed for deterministic output
        private static readonly Random random = new Random(12345);

        static bool OneInAThousand()
        {
            return random.Next(1000) == 0;
        }

        static bool OneInAHundred()
        {
            return random.Next(100) == 0;
        }

        static bool OneInATen()
        {
            return random.Next(10) == 0;
        }


        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "embedding")]
        public float[] Embedding { get; }


        [JsonProperty(PropertyName = "ranking")]
        public uint Ranking { get; set; }

        [JsonProperty(PropertyName = "pct")]
        public uint Pct { get; set; }

        [JsonProperty(PropertyName = "thousand")]
        public bool thousand { get; set; }

        [JsonProperty(PropertyName = "hundred")]
        public bool hundred { get; set; }

        [JsonProperty(PropertyName = "ten")]
        public bool ten { get; set; }


        public EmbeddingDocumentBase(string id, float[] embedding)
        {
            this.Id = id;
            this.Embedding = embedding;
            this.Ranking = uint.Parse(id);
            this.Pct = uint.Parse(id) % 100;
            this.thousand = OneInAThousand();
            this.hundred = OneInAHundred();
            this.ten = OneInATen();
        }
    }
}
