using System.Collections.Generic;
using NotificationCenter.Model.Exceptions.Imp;

namespace NotificationCenter.Model.ValueObjects
{
    public class Username : ValueObject
    {
        public string Value { get; private set; }

        public Username(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new UsernameEmptyException();
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}