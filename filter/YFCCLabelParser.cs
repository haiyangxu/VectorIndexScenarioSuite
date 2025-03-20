namespace VectorIndexScenarioSuite.filter
{
    internal static class YFCCLabelParser
    {
        public static Dictionary<string, object> ParseLineToJson(string line)
        {
            var result = new Dictionary<string, object>();
            var parts = line.Split(',');
            result["year"] = string.Empty;
            result["month"] = string.Empty;
            result["model"] = string.Empty;
            result["country"] = string.Empty;
            foreach (var part in parts)
            {
                var keyValue = part.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0];
                    var value = keyValue[1];

                    if (key == "year")
                    {
                        result["year"] = value;
                    }
                    else if (key == "month")
                    {
                        result["month"] = value;
                    }
                    else if (key == "model")
                    {
                        result["model"] = value;
                    }
                    else if (key == "country")
                    {
                        result["country"] = value;
                    }
                }
            }

            return result;
        }
    }
}
