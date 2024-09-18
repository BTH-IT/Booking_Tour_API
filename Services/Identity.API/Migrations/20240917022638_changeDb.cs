using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.API.Migrations
{
    /// <inheritdoc />
    public partial class changeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role_RoleId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleDetail_Permission_PermissionId",
                table: "RoleDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleDetail_Role_RoleId",
                table: "RoleDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Account_AccountId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleDetail",
                table: "RoleDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "RoleDetail",
                newName: "RoleDetails");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_User_AccountId",
                table: "Users",
                newName: "IX_Users_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleDetail_PermissionId",
                table: "RoleDetails",
                newName: "IX_RoleDetails_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_RoleId",
                table: "Accounts",
                newName: "IX_Accounts_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleDetails",
                table: "RoleDetails",
                columns: new[] { "RoleId", "PermissionId", "ActionName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Roles_RoleId",
                table: "Accounts",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDetails_Permissions_PermissionId",
                table: "RoleDetails",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDetails_Roles_RoleId",
                table: "RoleDetails",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accounts_AccountId",
                table: "Users",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Roles_RoleId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleDetails_Permissions_PermissionId",
                table: "RoleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleDetails_Roles_RoleId",
                table: "RoleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accounts_AccountId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleDetails",
                table: "RoleDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "RoleDetails",
                newName: "RoleDetail");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Users_AccountId",
                table: "User",
                newName: "IX_User_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleDetails_PermissionId",
                table: "RoleDetail",
                newName: "IX_RoleDetail_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_RoleId",
                table: "Account",
                newName: "IX_Account_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleDetail",
                table: "RoleDetail",
                columns: new[] { "RoleId", "PermissionId", "ActionName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role_RoleId",
                table: "Account",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDetail_Permission_PermissionId",
                table: "RoleDetail",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDetail_Role_RoleId",
                table: "RoleDetail",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Account_AccountId",
                table: "User",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
