using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.MultiWindowSample.ValueObjects;

namespace MVVM.MultiWindowSample.Entitirs
{
    public partial class UserEntity : ObservableObject
    {
        public long Id { get; }
        public string Name { get; }
        public string NameHiragana { get; }
        public long Age { get; }
        public string Birthdate { get; }
        public string Gender { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public PostalCode PostalCode { get; }
        public string Address { get; }
        public string CompanyName { get; }
        public string CreditCardNumber { get; }
        public string ExpirationDate { get; }
        public string MyNumber { get; }
        public DisplayDate CreatedAt { get; }
        public string UpdatedAt { get; }

        [ObservableProperty]
        public bool _isPrint = true;

        public UserEntity(
            long id,
            string name,
            string name_hiragana,
            long age,
            string birthdate,
            string gender,
            string email,
            string phone_number,
            string postal_code,
            string address,
            string company_name,
            string credit_card_number,
            string expiration_date,
            string my_number,
            string created_at,
            string updated_at)

        {
            Id = id;
            Name = name;
            NameHiragana = name_hiragana;
            Age = age;
            Birthdate = birthdate;
            Gender = gender;
            Email = email;
            PhoneNumber = phone_number;
            PostalCode = new PostalCode(postal_code);
            Address = address;
            CompanyName = company_name;
            CreditCardNumber = credit_card_number;
            ExpirationDate = expiration_date;
            MyNumber = my_number;
            CreatedAt = new DisplayDate(created_at);
            UpdatedAt = updated_at;
        }
    }
}
