using System.Collections.Generic;
using NotificationCenter.Model.Exceptions.Imp;

namespace NotificationCenter.Model.ValueObjects
{
    public class UserId : ValueObject
    {
        public string Value { get; private set; }

        public UserId(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new UserIdEmptyException();
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