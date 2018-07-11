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
    }
}
