using System;

namespace DataGrid_Sample.Enitities
{
    public sealed class UserEnitity
    {
        public int Id { get; }

        public string Name { get; }

        public int Age { get; }

        public string BirthDay { get; }

        public string Gender { get; }

        public string Email { get; }

        public string Tell { get; }

        public UserEnitity(int id,
                           string name,
                           string age,
                           string birthDay,
                           string gender,
                           string email,
                           string tell)
        {
            Id = id;
            Name = name;
            Age = Int32.Parse(age);
            BirthDay = birthDay;
            Gender = gender;
            Email = email;
            Tell = tell;
        }

    }
}
