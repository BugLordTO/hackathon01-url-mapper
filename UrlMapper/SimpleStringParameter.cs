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
                return ReturnIsMatch(false);
            else if (Pattern == textToCompare && string.IsNullOrEmpty(textToCompare))
                return ReturnIsMatch(true);
            else if (string.IsNullOrEmpty(textToCompare))
                return ReturnIsMatch(false);

            if (Routes.Count() == 1 && textToCompare.IndexOf('{') == textToCompare.IndexOf('}') && textToCompare.IndexOf('}') == -1)
                return ReturnIsMatch(true);

            var text = textToCompare;
            var ParameterName = string.Empty;
            foreach (var route in this.Routes)
            {
                var isParameter = route.StartsWith("{") && route.EndsWith("}");
                if (isParameter && string.IsNullOrEmpty(text))
                {
                    if (!AddParameter(ParameterName, string.Empty))
                        return ReturnIsMatch(false);
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
                    var ParameterValue = text.Substring(0, text.IndexOf(route));

                    if (!AddParameter(ParameterName, ParameterValue))
                        return ReturnIsMatch(false);

                    text = text.Substring(ParameterValue.Length + route.Length);
                    ParameterName = string.Empty;
                }
                else return ReturnIsMatch(false);
            }

            if (!string.IsNullOrEmpty(ParameterName) && !string.IsNullOrEmpty(text))
            {
                if (!AddParameter(ParameterName, text))
                    return ReturnIsMatch(false);
            }
            else if (string.IsNullOrEmpty(ParameterName) && !string.IsNullOrEmpty(text))
                return ReturnIsMatch(false);

            return ReturnIsMatch(true);
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

        private bool ReturnIsMatch(bool value)
        {
            IsMatch = value;
            return value;
        }
    }
}