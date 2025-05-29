using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RubberProductionManagement.Migrations
{
    /// <inheritdoc />
    public partial class huhu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkAssignments_Employees_EmployeeId",
                table: "WorkAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "WorkAssignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAssignments_Employees_EmployeeId",
                table: "WorkAssignments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkAssignments_Employees_EmployeeId",
                table: "WorkAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "WorkAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAssignments_Employees_EmployeeId",
                table: "WorkAssignments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
