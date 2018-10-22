using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StudyMate.Data.Migrations
{
    public partial class RadicalChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QandA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "QandA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoiceOver",
                table: "QandA",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QandA");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "QandA");

            migrationBuilder.DropColumn(
                name: "VoiceOver",
                table: "QandA");
        }
    }
}
