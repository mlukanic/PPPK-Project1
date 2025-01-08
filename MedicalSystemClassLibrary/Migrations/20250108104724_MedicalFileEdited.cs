using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class MedicalFileEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "MedicalFiles",
                newName: "ObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "MedicalFiles",
                newName: "FilePath");
        }
    }
}
