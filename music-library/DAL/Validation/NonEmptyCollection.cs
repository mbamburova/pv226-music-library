using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DAL.Validation
{
    public class NonEmptyCollection : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var collection = value as ICollection;
            return collection?.Count > 0;
        }
    }
}