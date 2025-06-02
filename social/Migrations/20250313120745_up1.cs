using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace social.Migrations
{
    public partial class up1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    accountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    peerId = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    fullname = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    picturename = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    birthDay = table.Column<string>(nullable: true),
                    registerDate = table.Column<string>(nullable: true),
                    authCode = table.Column<int>(nullable: false),
                    activeAll = table.Column<bool>(nullable: false),
                    activeUser = table.Column<bool>(nullable: false),
                    role = table.Column<string>(nullable: true),
                    danger = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.accountId);
                });

            migrationBuilder.CreateTable(
                name: "Callus",
                columns: table => new
                {
                    callusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    time = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Callus", x => x.callusId);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sharefiles",
                columns: table => new
                {
                    shareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    filename = table.Column<string>(nullable: true),
                    prefix = table.Column<string>(nullable: true),
                    dt = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    accountId = table.Column<int>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sharefiles", x => x.shareId);
                });

            migrationBuilder.CreateTable(
                name: "VisitorHits",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAddress = table.Column<string>(nullable: true),
                    DateTime = table.Column<string>(nullable: true),
                    VisitHit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorHits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AccountJoins",
                columns: table => new
                {
                    accjoinId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    follower = table.Column<bool>(nullable: false),
                    following = table.Column<bool>(nullable: false),
                    follower2 = table.Column<bool>(nullable: false),
                    following2 = table.Column<bool>(nullable: false),
                    fullfollow = table.Column<bool>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    username2 = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: false),
                    request = table.Column<bool>(nullable: false),
                    ignore = table.Column<bool>(nullable: false),
                    send = table.Column<bool>(nullable: false),
                    dt = table.Column<DateTime>(nullable: false),
                    accountId2 = table.Column<int>(nullable: false),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountJoins", x => x.accjoinId);
                    table.ForeignKey(
                        name: "FK_AccountJoins_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatFiles",
                columns: table => new
                {
                    cfId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    filename = table.Column<string>(nullable: true),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatFiles", x => x.cfId);
                    table.ForeignKey(
                        name: "FK_ChatFiles_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    chatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(nullable: true),
                    text = table.Column<string>(nullable: true),
                    pic = table.Column<string>(nullable: true),
                    voice = table.Column<string>(nullable: true),
                    video = table.Column<string>(nullable: true),
                    fileName = table.Column<string>(nullable: true),
                    sendAt = table.Column<DateTime>(nullable: false),
                    count = table.Column<int>(nullable: true),
                    accountId = table.Column<int>(nullable: false),
                    accountId2 = table.Column<int>(nullable: false),
                    active = table.Column<bool>(nullable: false),
                    active1 = table.Column<bool>(nullable: false),
                    active2 = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.chatId);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    groupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    active = table.Column<bool>(nullable: false),
                    danger = table.Column<bool>(nullable: false),
                    report = table.Column<int>(nullable: false),
                    countSee = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    like = table.Column<int>(nullable: false),
                    subject = table.Column<string>(nullable: true),
                    activetext = table.Column<bool>(nullable: false),
                    managerId = table.Column<int>(nullable: false),
                    password = table.Column<string>(nullable: true),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.groupId);
                    table.ForeignKey(
                        name: "FK_Groups_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UpdateAlls",
                columns: table => new
                {
                    uaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postUp = table.Column<bool>(nullable: false),
                    questUp = table.Column<bool>(nullable: false),
                    converUp = table.Column<bool>(nullable: false),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateAlls", x => x.uaId);
                    table.ForeignKey(
                        name: "FK_UpdateAlls_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wcams",
                columns: table => new
                {
                    wcamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    peerId = table.Column<string>(nullable: true),
                    accountId1 = table.Column<int>(nullable: false),
                    accountId2 = table.Column<int>(nullable: false),
                    username1 = table.Column<string>(nullable: true),
                    username2 = table.Column<string>(nullable: true),
                    calluser1 = table.Column<bool>(nullable: false),
                    calluser2 = table.Column<bool>(nullable: false),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wcams", x => x.wcamId);
                    table.ForeignKey(
                        name: "FK_Wcams_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    storyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    filename = table.Column<string>(nullable: true),
                    prefix = table.Column<string>(nullable: true),
                    dt = table.Column<DateTime>(nullable: false),
                    active = table.Column<bool>(nullable: false),
                    accountId = table.Column<int>(nullable: false),
                    accjoinId = table.Column<int>(nullable: false),
                    accountJoinaccjoinId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.storyId);
                    table.ForeignKey(
                        name: "FK_Stories_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stories_AccountJoins_accountJoinaccjoinId",
                        column: x => x.accountJoinaccjoinId,
                        principalTable: "AccountJoins",
                        principalColumn: "accjoinId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Peoplese",
                columns: table => new
                {
                    pId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(nullable: true),
                    gropId = table.Column<int>(nullable: false),
                    accountId = table.Column<int>(nullable: false),
                    groupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peoplese", x => x.pId);
                    table.ForeignKey(
                        name: "FK_Peoplese_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Peoplese_Groups_groupId",
                        column: x => x.groupId,
                        principalTable: "Groups",
                        principalColumn: "groupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    converId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    active = table.Column<bool>(nullable: false),
                    activetext = table.Column<bool>(nullable: false),
                    danger = table.Column<bool>(nullable: false),
                    filename = table.Column<string>(nullable: true),
                    voice = table.Column<string>(nullable: true),
                    pic = table.Column<string>(nullable: true),
                    video = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    text = table.Column<string>(nullable: true),
                    sendAt = table.Column<DateTime>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    accountId = table.Column<int>(nullable: false),
                    gropId = table.Column<int>(nullable: false),
                    UpdateAlluaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.converId);
                    table.ForeignKey(
                        name: "FK_Conversations_UpdateAlls_UpdateAlluaId",
                        column: x => x.UpdateAlluaId,
                        principalTable: "UpdateAlls",
                        principalColumn: "uaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostMs",
                columns: table => new
                {
                    postId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject = table.Column<string>(nullable: true),
                    countSee = table.Column<int>(nullable: false),
                    like = table.Column<int>(nullable: false),
                    textAll = table.Column<string>(nullable: true),
                    pic = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: false),
                    danger = table.Column<bool>(nullable: false),
                    report = table.Column<int>(nullable: false),
                    prefix = table.Column<string>(nullable: true),
                    accountId = table.Column<int>(nullable: false),
                    UpdateAlluaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMs", x => x.postId);
                    table.ForeignKey(
                        name: "FK_PostMs_UpdateAlls_UpdateAlluaId",
                        column: x => x.UpdateAlluaId,
                        principalTable: "UpdateAlls",
                        principalColumn: "uaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostMs_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Qustions",
                columns: table => new
                {
                    qId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject = table.Column<string>(nullable: true),
                    textAll = table.Column<string>(nullable: true),
                    countSee = table.Column<int>(nullable: false),
                    like = table.Column<int>(nullable: false),
                    active = table.Column<bool>(nullable: false),
                    danger = table.Column<bool>(nullable: false),
                    accountId = table.Column<int>(nullable: false),
                    UpdateAlluaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qustions", x => x.qId);
                    table.ForeignKey(
                        name: "FK_Qustions_UpdateAlls_UpdateAlluaId",
                        column: x => x.UpdateAlluaId,
                        principalTable: "UpdateAlls",
                        principalColumn: "uaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Qustions_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentPosts",
                columns: table => new
                {
                    cpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    accountId = table.Column<int>(nullable: false),
                    postId = table.Column<int>(nullable: false),
                    postMpostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPosts", x => x.cpId);
                    table.ForeignKey(
                        name: "FK_CommentPosts_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentPosts_PostMs_postMpostId",
                        column: x => x.postMpostId,
                        principalTable: "PostMs",
                        principalColumn: "postId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentExteras",
                columns: table => new
                {
                    ceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    like = table.Column<bool>(nullable: false),
                    see = table.Column<bool>(nullable: false),
                    report = table.Column<bool>(nullable: false),
                    accountId = table.Column<int>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    cpId = table.Column<int>(nullable: false),
                    commentPostcpId = table.Column<int>(nullable: true),
                    postId = table.Column<int>(nullable: false),
                    postMpostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentExteras", x => x.ceId);
                    table.ForeignKey(
                        name: "FK_CommentExteras_CommentPosts_commentPostcpId",
                        column: x => x.commentPostcpId,
                        principalTable: "CommentPosts",
                        principalColumn: "cpId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentExteras_PostMs_postMpostId",
                        column: x => x.postMpostId,
                        principalTable: "PostMs",
                        principalColumn: "postId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountJoins_accountId",
                table: "AccountJoins",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatFiles_accountId",
                table: "ChatFiles",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_accountId",
                table: "ChatMessages",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentExteras_commentPostcpId",
                table: "CommentExteras",
                column: "commentPostcpId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentExteras_postMpostId",
                table: "CommentExteras",
                column: "postMpostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPosts_accountId",
                table: "CommentPosts",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPosts_postMpostId",
                table: "CommentPosts",
                column: "postMpostId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UpdateAlluaId",
                table: "Conversations",
                column: "UpdateAlluaId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_accountId",
                table: "Conversations",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_accountId",
                table: "Groups",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Peoplese_accountId",
                table: "Peoplese",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Peoplese_groupId",
                table: "Peoplese",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMs_UpdateAlluaId",
                table: "PostMs",
                column: "UpdateAlluaId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMs_accountId",
                table: "PostMs",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Qustions_UpdateAlluaId",
                table: "Qustions",
                column: "UpdateAlluaId");

            migrationBuilder.CreateIndex(
                name: "IX_Qustions_accountId",
                table: "Qustions",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_accountId",
                table: "Stories",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_accountJoinaccjoinId",
                table: "Stories",
                column: "accountJoinaccjoinId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateAlls_accountId",
                table: "UpdateAlls",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Wcams_accountId",
                table: "Wcams",
                column: "accountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Callus");

            migrationBuilder.DropTable(
                name: "ChatFiles");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "CommentExteras");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Peoplese");

            migrationBuilder.DropTable(
                name: "provinces");

            migrationBuilder.DropTable(
                name: "Qustions");

            migrationBuilder.DropTable(
                name: "Sharefiles");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "VisitorHits");

            migrationBuilder.DropTable(
                name: "Wcams");

            migrationBuilder.DropTable(
                name: "CommentPosts");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "AccountJoins");

            migrationBuilder.DropTable(
                name: "PostMs");

            migrationBuilder.DropTable(
                name: "UpdateAlls");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
