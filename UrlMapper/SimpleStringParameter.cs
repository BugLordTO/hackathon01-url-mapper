using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameter : ISimpleStringParameter
    {
        public bool? IsMatch { get; set; }
        public string Pattern { get; set; }
        public IEnumerable<string> Routes { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        public SimpleStringParameter(IEnumerable<string> Routes, string Pattern)
        {
            this.Parameters = new Dictionary<string, string>();
            this.Routes = Routes;
            this.Pattern = Pattern;
        }

        public bool IsMatched(string textToCompare)
        {
            if (Pattern == null || textToCompare == null)
            {
                IsMatch = false;
                return false;
            }
            else if (Pattern == textToCompare && string.IsNullOrEmpty(textToCompare))
            {
                IsMatch = true;
                return true;
            }
            else if (string.IsNullOrEmpty(textToCompare))
            {
                IsMatch = false;
                return false;
            }

            if (Routes.Count() == 1 && textToCompare.IndexOf('{') == textToCompare.IndexOf('}') && textToCompare.IndexOf('}') == -1)
            {
                IsMatch = true;
                return true;
            }

            var text = textToCompare;
            var ParameterName = string.Empty;
            foreach (var route in this.Routes)
            {
                var isParameter = route.StartsWith("{") && route.EndsWith("}");
                if (isParameter && string.IsNullOrEmpty(text))
                {
                    if (!AddParameter(ParameterName, string.Empty))
                    {
                        IsMatch = false;
                        return false;
                    }
                }
                else if (isParameter)
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
                    if (!AddParameter(ParameterName, Paramval))
                    {
                        IsMatch = false;
                        return false;
                    }
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
                if (!AddParameter(ParameterName, text))
                {
                    IsMatch = false;
                    return false;
                };
            }
            else if (string.IsNullOrEmpty(ParameterName) && !string.IsNullOrEmpty(text))
            {
                IsMatch = false;
                return false;
            }
            IsMatch = true;
            return true;
        }

        public void ExtractVariables(string target, IDictionary<string, string> dicToStoreResults)
        {
            if ((IsMatch == null && IsMatched(target)) || IsMatch.Value)
            {
                foreach (var item in Parameters)
                {
                    dicToStoreResults.Add(item);
                }
            }
        }

        private bool AddParameter(string key, string param)
        {
            if (!Parameters.ContainsKey(key))
            {
                Parameters.Add(key, param);
                return true;
            }
            return false;
        }
    }
}