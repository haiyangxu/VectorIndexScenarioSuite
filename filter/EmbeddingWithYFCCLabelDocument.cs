using Newtonsoft.Json;

namespace VectorIndexScenarioSuite.filter
{
    internal class EmbeddingWithYFCCLabelDocument : EmbeddingOnlyDocument
    {
        [JsonProperty(PropertyName = "year")]
        private string Year { get; }

        [JsonProperty(PropertyName = "month")]
        private string Month { get; }

        [JsonProperty(PropertyName = "model")]
        private string Model { get; }

        [JsonProperty(PropertyName = "country")]
        private string Country { get; }

        [JsonConstructor]
        public EmbeddingWithYFCCLabelDocument(string id, float[] embedding, string year, string month, string model, string country)
            : base(id, embedding) // Call the base class constructor
        {
            Year = year;
            Month = month;
            Model = model;
            Country = country;
        }

        public EmbeddingWithYFCCLabelDocument(string id, float[] embedding, Dictionary<string, object> yfccLabel)
            : base(id, embedding) // Call the base class constructor
        {
            Year = yfccLabel["year"].ToString();
            Month = yfccLabel["month"].ToString();
            Model = yfccLabel["model"].ToString();
            Country = yfccLabel["country"].ToString();
        }
    }
}
