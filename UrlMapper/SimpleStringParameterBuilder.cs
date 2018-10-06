using System.Collections.Generic;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameterBuilder : ISimpleStringParameterBuilder
    {
        public ISimpleStringParameter Parse(string pattern)
        {
            // TODO: Need to implement this method.
            var routes = new List<string>();
            var patternSplit = pattern.Split('{');
            foreach(var str in patternSplit)
            {
                var strSplit = str.Split('}');
                if(strSplit.Length == 1)
                        routes.Add(str);
                else if(strSplit.Length == 2)
                        routes.Add("{" + strSplit[0]  + "}");
                else{
                    routes.Add("{" + strSplit[0]  + "}");
                    routes.Add( strSplit.Skip(1).Aggregate((x,y)=> x + "}" + y));
                }
            }
            
            return new SimpleStringParameter(routes);
        }
    }
}