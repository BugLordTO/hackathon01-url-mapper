using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameter : ISimpleStringParameter
    {
        public bool? IsMatch { get; set; }
        public IEnumerable<string> Routes { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        public SimpleStringParameter(IEnumerable<string> Routes)
        {
            this.Routes = Routes;
        }

        public bool IsMatched(string textToCompare)
        {
            var text = textToCompare;
            var ParameterName = string.Empty;
            foreach (var route in this.Routes)
            {
                var isParameter = route.StartsWith("{") && route.EndsWith("}");
                if (isParameter)
                {
                    ParameterName = route;
                }
                else if (!isParameter && text.StartsWith(route))
                {
                    text = text.Substring(route.Length);
                }
                else if (!isParameter && text.Contains(route))
                {
                    var Paramval = text.Substring(0, text.IndexOf(route));
                    Parameters.Add(ParameterName, Paramval);
                    text = text.Substring(Paramval.Length + route.Length);
                    ParameterName = string.Empty;
                }
                else
                {
                    IsMatch = false;
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(ParameterName) && !string.IsNullOrEmpty(text))
            {
                Parameters.Add(ParameterName, text);
            }
            else if (string.IsNullOrEmpty(ParameterName) && !string.IsNullOrEmpty(text))
            {
                IsMatch = false;
                return false;
            }
            else if (!string.IsNullOrEmpty(ParameterName) && string.IsNullOrEmpty(text))
            {
                IsMatch = false;
                return false;
            }
            IsMatch = true;
            return true;
        }

        public void ExtractVariables(string target, IDictionary<string, string> dicToStoreResults)
        {
            if (IsMatch == null)
            {
                if (IsMatched(target))
                    dicToStoreResults = Parameters;
            }
            else if (IsMatch.Value)
            {
                dicToStoreResults = Parameters;
            }
        }
    }
}