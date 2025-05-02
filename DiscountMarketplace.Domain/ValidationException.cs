using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMarketplace.Domain
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; }

        public ValidationException()
        {
            Errors = new Dictionary<string, string>();
        }

        public ValidationException(Dictionary<string, string> errors)
        {
            Errors = errors ?? new Dictionary<string, string>();
        }

        public void Add(string field, string message)
        {
            if (!Errors.ContainsKey(field))
                Errors[field] = message;
        }

        public bool HasErrors => Errors.Count > 0;
    }
}
