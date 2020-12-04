using System.Collections.Generic;
using System.Linq;
using NotificationCenter.Model.Exceptions.Imp;

namespace NotificationCenter.Model.ValueObjects
{
    public class NotificationTitle : ValueObject
    {
        private const int MAX_LENGHT = 50;
        public string Value { get; private set; }

        public NotificationTitle(string value)
        {
            value = value?.Trim() ?? string.Empty;
            if (value == string.Empty) throw new NotificationTitleEmptyException();

            if (value.Length > MAX_LENGHT)
            {
                value = $"{value.Take(MAX_LENGHT - 3)}...";
            }

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