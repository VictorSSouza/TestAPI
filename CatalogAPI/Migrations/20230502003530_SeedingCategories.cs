using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    public partial class SeedingCategories : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categories(Name,ImageUrl) Values('Bebidas','bebidas.jpg')");
            mb.Sql("Insert into Categories(Name,ImageUrl) Values('Lanches','lanches.jpg')");
            mb.Sql("Insert into Categories(Name,ImageUrl) Values('Sobremesas','sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categories");
        }
    }
}
