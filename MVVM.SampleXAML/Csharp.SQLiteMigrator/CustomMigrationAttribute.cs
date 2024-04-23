namespace Csharp.SQLiteMigrator
{
    public class CustomMigrationAttribute : FluentMigrator.MigrationAttribute
    {
        public string Author { get; private set; }

        public CustomMigrationAttribute(int branchNumber, int year, int month, int day, int hour, int minute, string author) 
            : base(CalculateValue(branchNumber, year, month, day, hour, minute))
        {
            Author = author;
        }

        private static long CalculateValue(int branchNumber, int year, int month, int day, int hour, int minute)
        {
            return branchNumber * 1000000000000L + year * 100000000L + month * 1000000L + day * 10000L + hour * 100L + minute;
        }
    }
}
