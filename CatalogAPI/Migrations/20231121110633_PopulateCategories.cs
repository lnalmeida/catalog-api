using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    public partial class PopulateCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Categories(CategoryId, CategoryName, CategoryImageUrl)" +
                "values (UUID(),'Bebidas', 'https://media.istockphoto.com/id/1371317396/pt/foto/assortment-of-hard-strong-alcoholic-drinks-and-spirits.jpg');");

            migrationBuilder.Sql("insert into Categories(CategoryId, CategoryName, CategoryImageUrl)" +
                " values (UUID(),'Carnes', 'https://media.istockphoto.com/id/174479270/pt/foto/frescos-ribeye-de-bifes-na-loja-de-carniceiro.jpg');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categories");
        }
    }
}
