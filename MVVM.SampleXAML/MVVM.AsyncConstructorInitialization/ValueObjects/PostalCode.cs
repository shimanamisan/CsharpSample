using MVVM.AsyncConstructorInitialization.Extensions;
using System;

namespace MVVM.AsyncConstructorInitialization.ValueObjects
{
    public sealed class PostalCode : ValueObject<PostalCode>
    {
        public string Value { get; }

        public string FormattedValue => Value.Format();

        public PostalCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                throw new ArgumentException("郵便番号が入力されていません");
            }

            Value = zipCode;
        }

        protected override bool EqualsCore(PostalCode other)
        {
            return Value == other.Value;
        }
    }
}
