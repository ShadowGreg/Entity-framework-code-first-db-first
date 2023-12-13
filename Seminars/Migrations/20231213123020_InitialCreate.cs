using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Seminars.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pk", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    send_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_sent = table.Column<bool>(type: "bit", nullable: false),
                    UserFromId = table.Column<int>(type: "int", nullable: true),
                    UserToId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pk", x => x.MessageId);
                    table.ForeignKey(
                        name: "massage_from_user_fk",
                        column: x => x.UserFromId,
                        principalTable: "users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "massage_to_user_fk",
                        column: x => x.UserToId,
                        principalTable: "users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_messages_UserFromId",
                table: "messages",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_UserToId",
                table: "messages",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_users_full_name",
                table: "users",
                column: "full_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
