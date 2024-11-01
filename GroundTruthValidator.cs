namespace VectorIndexScenarioSuite
{
    internal enum GroundTruthFileType
    {
        Binary
    }

    internal class GroundTruthValidator
    {
        private IDictionary<string, List<IdWithSimilarityScore>> groundTruth;
        private int groundTruthKValue;

        public GroundTruthValidator(GroundTruthFileType fileType, string filePath)
        {
            this.groundTruth = new Dictionary<string, List<IdWithSimilarityScore>>();
            LoadGroundTruthData(fileType, filePath);
        }

        public float ComputeRecall(int queryKValue, IDictionary<string, List<IdWithSimilarityScore>> queryResults)
        {
            if (this.groundTruthKValue < queryKValue)
            {
                throw new ArgumentException("Query K value is greater than the ground truth K value");
            }

            float recall = 0;
            HashSet<string> groundTruthIdsForQuery = new HashSet<string>();
            HashSet<string> resultIdsForQuery = new HashSet<string>();
            List<double> similarityScoreFromQuery = new List<double>();

            int cumulativeTruePositive = 0;
            var queryIds = queryResults.Keys;
            foreach (string queryId in queryResults.Keys)
            {
                groundTruthIdsForQuery.Clear();
                resultIdsForQuery.Clear();

                for (int i = 0; i < queryKValue; i++)
                {
                    resultIdsForQuery.Add(queryResults[queryId][i].Id);
                }

                for (int i = 0; i < queryKValue; i++)
                {
                    similarityScoreFromQuery.Add(queryResults[queryId][i].SimilarityScore);
                }

                /* Compute valid ground truth ids for the query 
                 * Handle scenario where multiple vectors have the same similarity score as the kth vector
                 */
                int tieBreaker = queryKValue - 1;
                while (tieBreaker < this.groundTruth[queryId].Count &&
                    this.groundTruth[queryId][tieBreaker].SimilarityScore == this.groundTruth[queryId][queryKValue - 1].SimilarityScore)
                {
                    tieBreaker++;
                }
                for (int i = 0; i < tieBreaker; i++)
                {
                    groundTruthIdsForQuery.Add(this.groundTruth[queryId][i].Id);
                }

                //Print the ground truth ids for the query
                Console.WriteLine();
                Console.WriteLine($"Ground truth ids and distances for query {queryId}: ");
                int idx = 0;
                foreach (string id in groundTruthIdsForQuery)
                {
                    Console.Write(id + "->" + this.groundTruth[queryId][idx].SimilarityScore + ", ");
                    idx++;
                }

                //Print the result ids for the query
                Console.WriteLine();
                Console.WriteLine($"Result ids for query {queryId}: ");
                int idx2 = 0;
                foreach (string id in resultIdsForQuery)
                {
                    Console.Write(id + "->" + similarityScoreFromQuery[idx2] + ", ");
                    idx2++;
                }

                int truePositive = 0;
                foreach (string queryResultId in resultIdsForQuery)
                {
                    if (groundTruthIdsForQuery.Contains(queryResultId))
                    {
                        cumulativeTruePositive++;
                        truePositive++;
                    }
                }
            }

            Console.WriteLine($"Recall Stats: " +
                $"Cumulative True Positive: {cumulativeTruePositive}, " +
                $"NumQueries: {queryResults.Count}, KValue: {queryKValue}, GroundTruthKValue: {groundTruthKValue}");

            float averageHitsAcrossQueries = ((cumulativeTruePositive * 1.0f) / queryResults.Count);
            recall = (averageHitsAcrossQueries / queryKValue) * 100.0f;
            return recall;
        }

        private void LoadGroundTruthData(GroundTruthFileType fileType, string filePath)
        {
            switch (fileType)
            {
                case GroundTruthFileType.Binary:
                    LoadGroundTruthDataFromBinaryFile(filePath).Wait();
                    break;
                default:
                    throw new ArgumentException("Invalid GroundTruthFileType: ", nameof(fileType));
            }
        }
        private async Task LoadGroundTruthDataFromBinaryFile(string filePath)
        {
            await foreach ((int vectorId, int[] groundTruthNeighborIds, float[] groundTruthNeighborDistances) in BigANNBinaryFormat.GetGroundTruthDataAsync(filePath))
            {
                List<IdWithSimilarityScore> idWithSimilarityScores = new List<IdWithSimilarityScore>();
                for (int i = 0; i < groundTruthNeighborIds.Length; i++)
                {
                    idWithSimilarityScores.Add(new IdWithSimilarityScore(groundTruthNeighborIds[i].ToString(), groundTruthNeighborDistances[i]));
                }

                if (vectorId == 0)
                {
                    this.groundTruthKValue = groundTruthNeighborIds.Length;
                }
                else if (this.groundTruthKValue != groundTruthNeighborIds.Length)
                {
                    Console.WriteLine($"Ground truth K value mismatch for vectorId: {vectorId}");
                }

                this.groundTruth.Add(vectorId.ToString(), idWithSimilarityScores);
            }
        }
    }
}
