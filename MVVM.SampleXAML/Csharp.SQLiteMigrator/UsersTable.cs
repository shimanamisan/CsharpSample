using FluentMigrator;

namespace Csharp.SQLiteMigrator
{
    [CustomMigration(author: "H.Nishihara", branchNumber: 12, year: 2023, month: 7, day: 19, hour: 17, minute: 29)]
    public sealed class UsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("name_hiragana").AsString().NotNullable()
                .WithColumn("age").AsInt32().NotNullable()
                .WithColumn("birthdate").AsDate().NotNullable()
                .WithColumn("gender").AsString().NotNullable()
                .WithColumn("email").AsString().NotNullable()
                .WithColumn("phone_number").AsString().NotNullable()
                .WithColumn("postal_code").AsString().NotNullable()
                .WithColumn("address").AsString().NotNullable()
                .WithColumn("company_name").AsString().Nullable()
                .WithColumn("credit_card_number").AsString().NotNullable()
                .WithColumn("expiration_date").AsDate().NotNullable()
                .WithColumn("my_number").AsString().NotNullable()
                .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        public override void Down()
        {
            Delete.Table("product");
        }
    }
}
