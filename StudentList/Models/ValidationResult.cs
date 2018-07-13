using System.Collections.Generic;
using System.Linq;

namespace StudentList.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Errors = new Dictionary<string, IEnumerable<string>>();
        }

        public bool IsValid => !this.Errors.Any();

        public Dictionary<string, IEnumerable<string>> Errors { get; private set; }

        public string GetErrorMessages(string key)
        {
            return string.Join(".", this.Errors.Where(e => e.Key == key).SelectMany(e => e.Value));
        }
    }
}
