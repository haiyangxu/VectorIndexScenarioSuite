using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
namespace VectorIndexScenarioSuite
{ 
    internal class MSMarcoEmbeddingOnlyScenario : BigANNBinaryEmbeddingOnlyScearioBase
    {
        protected override string BaseDataFile => "base";
        protected override string BinaryFileExt => "fbin";
        protected override string QueryFile => "query";
        protected override string GetGroundTruthFileName => "ground_truth";
        protected override string PartitionKeyPath => "/id";
        protected override string EmbeddingColumn => "embedding";
        protected override string EmbeddingPath => $"/{EmbeddingColumn}";
        protected override VectorDataType EmbeddingDataType => VectorDataType.Float32;
        protected override DistanceFunction EmbeddingDistanceFunction => DistanceFunction.DotProduct;
        protected override int EmbeddingDimensions => 768;
        protected override int MaxPhysicalPartitionCount => 56;
        protected override string RunName => "msmarco-embeddingonly-" + guid;

        public MSMarcoEmbeddingOnlyScenario(IConfiguration configurations) : 
            base(configurations, DefaultInitialAndFinalThroughput(configurations).Item1)
        {
        }

        public override void Setup()
        {
            this.ReplaceFinalThroughput(DefaultInitialAndFinalThroughput(this.Configurations).Item2);
        }
        protected override ContainerProperties GetContainerSpec(string containerName)
        {
            ContainerProperties properties = new ContainerProperties(id: containerName, partitionKeyPath: this.PartitionKeyPath)
            {
                VectorEmbeddingPolicy = new VectorEmbeddingPolicy(new Collection<Embedding>(new List<Embedding>()
                {
                    new Embedding()
                    {
                        Path = this.EmbeddingPath,
                        DataType = this.EmbeddingDataType,
                        DistanceFunction = this.EmbeddingDistanceFunction,
                        Dimensions = this.EmbeddingDimensions,
                    }
                })),
                IndexingPolicy = new IndexingPolicy()
                {
                    VectorIndexes = new()
                    {
                        new VectorIndexPath()
                        {
                            Path = this.EmbeddingPath,
                            Type = VectorIndexType.DiskANN,
                            QuantizationByteSize = 192,
                            IndexingSearchListSize = 100,

                        }
                    }
                }
            };

            properties.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = "/" });

            // Add EMBEDDING_PATH to excluded paths for scalar indexing.
            properties.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath { Path = this.EmbeddingPath + "/*" });

            return properties;
        }
        private static (int, int) DefaultInitialAndFinalThroughput(IConfiguration configurations)
        {
            // default throughput for MSMarcoEmbeddingOnlyScenario
            int sliceCount = Convert.ToInt32(configurations["AppSettings:scenario:sliceCount"]);
            switch (sliceCount)
            {
                case HUNDRED_THOUSAND:
                case ONE_MILLION:
                    return (6000, 10000);
                case TEN_MILLION:
                    return (12000, 20000);
                case ONE_HUNDRED_MILLION:
                    return (48000, 80000);
                default:
                    throw new ArgumentException("Invalid slice count.");
            }
        }
    }
}
