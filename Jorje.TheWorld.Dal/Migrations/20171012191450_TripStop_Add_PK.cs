using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Jorje.TheWorld.Dal.Migrations
{
    public partial class TripStop_Add_PK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TripStop",
                table: "TripStop");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TripStop",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripStop",
                table: "TripStop",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TripStop_TripId",
                table: "TripStop",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TripStop",
                table: "TripStop");

            migrationBuilder.DropIndex(
                name: "IX_TripStop_TripId",
                table: "TripStop");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TripStop");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripStop",
                table: "TripStop",
                columns: new[] { "TripId", "StopId" });
        }
    }
}
