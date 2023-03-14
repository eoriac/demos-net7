using FluentMigrator;

namespace DemoSesion3.Migrations
{
    [Migration(2023130300001)]
    public class InitialTables_2023130300001 : Migration
    {
        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("Games");
        }

        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("Email").AsString(50).NotNullable()
                .WithColumn("AvatarUrl").AsString(200).Nullable();

            Create.Table("Games")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("Description").AsString(200).Nullable()
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("Users", "Id");
        }
    }
}
