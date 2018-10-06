using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameter : ISimpleStringParameter
    {
        public IEnumerable<string> Routes { get; }

        public SimpleStringParameter(IEnumerable<string> Routes)
        {
            this.Routes = Routes;
        }

        public void ExtractVariables(string target, IDictionary<string, string> dicToStoreResults)
        {
            throw new System.NotImplementedException();
        }

        public bool IsMatched(string textToCompare)
        {
            throw new System.NotImplementedException();
        }
    }
}