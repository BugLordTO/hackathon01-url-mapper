using System.Collections.Generic;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameterBuilder : ISimpleStringParameterBuilder
    {
        public ISimpleStringParameter Parse(string pattern)
        {
            var routes = new List<string>();
            var patternSplit = pattern.Split('{');
            var isFirst = true;
            foreach (var str in patternSplit)
            {
                var strSplit = str.Split('}');
                if (strSplit.Length == 1 || isFirst)
                    routes.Add(str);
                else if (strSplit.Length == 2)
                    routes.Add("{" + strSplit[0] + "}");
                else
                {
                    routes.Add("{" + strSplit[0] + "}");
                    routes.Add(strSplit.Skip(1).Aggregate((x, y) => x + "}" + y));
                }
                isFirst = false;
            }

            return new SimpleStringParameter(routes);
        }
    }
}