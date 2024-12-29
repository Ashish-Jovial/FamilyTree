using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.FamilyTree.Migrations
{
    /// <inheritdoc />
    public partial class FamilyTreeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuperAdmins",
                columns: table => new
                {
                    SuperAdminID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmins", x => x.SuperAdminID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PanCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AadharCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoterCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentIdCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByAdminID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "DeletionRequests",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedByAdminID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedBySuperAdminID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletionRequests", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_DeletionRequests_SuperAdmins_VerifiedBySuperAdminID",
                        column: x => x.VerifiedBySuperAdminID,
                        principalTable: "SuperAdmins",
                        principalColumn: "SuperAdminID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletionRequests_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    FamilyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeadOfFamilyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentFamilyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GovFamilyID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.FamilyID);
                    table.ForeignKey(
                        name: "FK_Families_Families_ParentFamilyID",
                        column: x => x.ParentFamilyID,
                        principalTable: "Families",
                        principalColumn: "FamilyID");
                    table.ForeignKey(
                        name: "FK_Families_Users_HeadOfFamilyID",
                        column: x => x.HeadOfFamilyID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "UserPersonalDetails",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPersonalDetails", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserPersonalDetails_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfessionalDetails",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnualIncome = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfessionalDetails", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserProfessionalDetails_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyHeadChangeRequests",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedByID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProposedHeadID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousHeadID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeathNotification = table.Column<bool>(type: "bit", nullable: false),
                    DeceasedMemberID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeathReportedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyHeadChangeRequests", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_FamilyHeadChangeRequests_Families_FamilyID",
                        column: x => x.FamilyID,
                        principalTable: "Families",
                        principalColumn: "FamilyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyHeadChangeRequests_Users_PreviousHeadID",
                        column: x => x.PreviousHeadID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyHeadChangeRequests_Users_ProposedHeadID",
                        column: x => x.ProposedHeadID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyHeadChangeRequests_Users_RequestedByID",
                        column: x => x.RequestedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    FamilyMemberID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.FamilyMemberID);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Families_FamilyID",
                        column: x => x.FamilyID,
                        principalTable: "Families",
                        principalColumn: "FamilyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilySettings",
                columns: table => new
                {
                    SettingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilySettings", x => x.SettingID);
                    table.ForeignKey(
                        name: "FK_FamilySettings_Families_FamilyID",
                        column: x => x.FamilyID,
                        principalTable: "Families",
                        principalColumn: "FamilyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletionRequests_UserID",
                table: "DeletionRequests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DeletionRequests_VerifiedBySuperAdminID",
                table: "DeletionRequests",
                column: "VerifiedBySuperAdminID");

            migrationBuilder.CreateIndex(
                name: "IX_Families_HeadOfFamilyID",
                table: "Families",
                column: "HeadOfFamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ParentFamilyID",
                table: "Families",
                column: "ParentFamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyHeadChangeRequests_FamilyID",
                table: "FamilyHeadChangeRequests",
                column: "FamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyHeadChangeRequests_PreviousHeadID",
                table: "FamilyHeadChangeRequests",
                column: "PreviousHeadID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyHeadChangeRequests_ProposedHeadID",
                table: "FamilyHeadChangeRequests",
                column: "ProposedHeadID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyHeadChangeRequests_RequestedByID",
                table: "FamilyHeadChangeRequests",
                column: "RequestedByID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_FamilyID",
                table: "FamilyMembers",
                column: "FamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_UserID",
                table: "FamilyMembers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilySettings_FamilyID",
                table: "FamilySettings",
                column: "FamilyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletionRequests");

            migrationBuilder.DropTable(
                name: "FamilyHeadChangeRequests");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "FamilySettings");

            migrationBuilder.DropTable(
                name: "UserPersonalDetails");

            migrationBuilder.DropTable(
                name: "UserProfessionalDetails");

            migrationBuilder.DropTable(
                name: "SuperAdmins");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
