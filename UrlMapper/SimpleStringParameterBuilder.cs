using System.Collections.Generic;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameterBuilder : ISimpleStringParameterBuilder
    {
        public ISimpleStringParameter Parse(string pattern)
        {
            var routes = new List<string>();

            if (string.IsNullOrEmpty(pattern))
                return new SimpleStringParameter(routes, pattern);

            if (pattern.IndexOf('{') == pattern.IndexOf('}') && pattern.IndexOf('}') == -1)
            {
                routes.Add(pattern.TrimEnd('/'));
                return new SimpleStringParameter(routes, pattern);
            }

            var patternSplit = pattern.Split('{');
            var isFirst = true;
            foreach (var str in patternSplit)
            {
                var strSplit = str.Split('}');
                if (strSplit.Length == 1 || isFirst)
                    routes.Add(str);
                else if (strSplit.Length > 1) { 
                //    routes.Add("{" + strSplit[0] + "}");
                //if (!string.IsNullOrEmpty(strSplit[1]))
                //    routes.Add(strSplit[1]);
                //else
                //{
                    routes.Add("{" + strSplit[0] + "}");
                    var strSkip = string.Join("}", strSplit.Skip(1));
                    if (!string.IsNullOrEmpty(strSkip))
                        routes.Add(strSkip);
                    //routes.Add(strSplit.Skip(1).Aggregate((x, y) => x + "}" + y));
                }
                isFirst = false;
            }

            return new SimpleStringParameter(routes, pattern);
        }
    }
}