namespace GeneralStoreAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Product_SKU", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "Product_SKU");
            AddForeignKey("dbo.Products", "Product_SKU", "dbo.Products", "SKU");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Product_SKU", "dbo.Products");
            DropIndex("dbo.Products", new[] { "Product_SKU" });
            DropColumn("dbo.Products", "Product_SKU");
        }
    }
}
