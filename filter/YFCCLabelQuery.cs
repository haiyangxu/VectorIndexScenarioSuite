namespace VectorIndexScenarioSuite.filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class YFCCQueryParser
    {
        // Function to create query_clause from query label
        public static List<string> FromQuery(string queryLabel)
        {
            var queryClause = new List<string>();
            var andClauses = queryLabel.Split('&');

            foreach (var token in andClauses)
            {
                queryClause.Add(token);
            }

            return queryClause;
        }

        // Function to create a WHERE statement from query_clause
        public static string ToWhereStatement(List<string> queryClause)
        {
            var whereClauses = new List<string>();

            foreach (var condition in queryClause)
            {
                    var parts = condition.Split('=');
                    var field = parts[0];
                    var value = parts[1];
                    if (field == "year")
                    {
                        whereClauses.Add($"c.year = \"{value}\"");
                    }
                    else if (field == "month")
                    {
                        whereClauses.Add($"c.month = \"{value}\"");
                    }
                    else if (field == "model")
                    {
                        whereClauses.Add($"c.model = \"{value}\"");
                    }
                    else if (field == "country")
                    {
                        whereClauses.Add($"c.country = \"{value}\"");
                    }                
            }

            return string.Join(" AND ", whereClauses);
        }
    }


}
