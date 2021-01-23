namespace GeneralStoreAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Transaction_Id", c => c.Int());
            CreateIndex("dbo.Transactions", "Transaction_Id");
            AddForeignKey("dbo.Transactions", "Transaction_Id", "dbo.Transactions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Transaction_Id", "dbo.Transactions");
            DropIndex("dbo.Transactions", new[] { "Transaction_Id" });
            DropColumn("dbo.Transactions", "Transaction_Id");
        }
    }
}
