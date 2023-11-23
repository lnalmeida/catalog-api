using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Migrations
{
    public partial class PopulateProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Products (ProductId, Name, Description, Price, Stock, Created, CategoryId )" +
                " values (UUID(), 'Coronita', 'Cerveja Corona 210ml', 7.00, 150, Now(),'4e4337a5-885e-11ee-8562-3a965696bc64');");

            migrationBuilder.Sql("insert into Products (ProductId, Name, Description, Price, Stock, Created, CategoryId )" +
                " values (UUID(), 'Heinekein lata', 'Cerveja Heinekein lata 350ml 210ml', 5.00, 150, Now(),'4e4337a5-885e-11ee-8562-3a965696bc64');");

            migrationBuilder.Sql("insert into Products (ProductId, Name, Description, Price, Stock, Created, CategoryId )" +
                " values (UUID(), 'Carré suíno', 'Carré suíno, Kg', 15.00, 1500, Now(),'4e4384a3-885e-11ee-8562-3a965696bc64');");

            migrationBuilder.Sql("insert into Products (ProductId, Name, Description, Price, Stock, Created, CategoryId )" +
                " values (UUID(), 'Alcatra bovina', 'Alcatra bovina corte, Kg', 30.00, 1500, Now(),'4e4384a3-885e-11ee-8562-3a965696bc64');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Products");
        }
    }
}
