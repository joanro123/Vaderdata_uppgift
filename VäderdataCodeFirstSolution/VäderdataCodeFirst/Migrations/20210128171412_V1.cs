using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VäderdataCodeFirst.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Plats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Luftfuktighet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInfo", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherInfo");
        }
    }
}
