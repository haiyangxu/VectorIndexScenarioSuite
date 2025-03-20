using Newtonsoft.Json;

namespace VectorIndexScenarioSuite.filter
{
    internal class EmbeddingWithAmazonLabelDocument : EmbeddingOnlyDocument
    {
        [JsonProperty(PropertyName = "brand")]
        private string Brand { get; }

        //ratting 
        [JsonProperty(PropertyName = "rating")]
        private string Rating { get; }

        // category
        [JsonProperty(PropertyName = "category")]
        private string[] Category { get; }

        public EmbeddingWithAmazonLabelDocument(string id, float[] embedding, string brand, string rating, string[] category)
            : base(id, embedding) // Call the base class constructor
        {
            Brand = brand;
            Rating = rating;
            Category = category;
        }

        public EmbeddingWithAmazonLabelDocument(string id, float[] embedding, Dictionary<string, object> amazonLabel)
            : base(id, embedding) // Call the base class constructor
        {
            Brand = amazonLabel["brand"].ToString();
            Rating = amazonLabel["rating"].ToString();
            Category = ((List<object>)amazonLabel["category"]).Select(x => x.ToString()).ToArray();
        }
    }
}
