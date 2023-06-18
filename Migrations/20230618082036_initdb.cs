using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using razorweb.models;

#nullable disable

namespace asprazor04.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Table 
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });
            // Insert Data
            // Fake Data: Bogus
            // migrationBuilder.InsertData(
            //     table: "articles",
            //     columns: new[] {"Title", "Created", "Content"},
            //     values: new object[] {
            //         "Bai viet 1",
            //         new DateTime(2021,8,20),
            //         "Noi dung 1"
            //     } 
            // );

            Randomizer.Seed = new Random(8675309);
            // khai báo biến để chứa các fake Bogus Data
            var fakerArticle = new Faker<Article>();
            // thiết LẬP LUẬT cho thuộc tính nào của đối tượng nào
            fakerArticle.RuleFor(a => a.Title, f => f.Lorem.Sentence(5,5));
            fakerArticle.RuleFor(a => a.Created, f => f.Date.Between(new DateTime(2021,1,11),new DateTime(2021, 7 , 30)));
            fakerArticle.RuleFor(a => a.Content, f => f.Lorem.Paragraphs(1,4));


            for(int i = 0 ; i < 150 ; i++){
                // PHÁT SINH NGẪU NHIÊN
                Article article = fakerArticle.Generate();
                migrationBuilder.InsertData(
                    table: "articles",
                    columns: new[] {"Title", "Created", "Content"},
                    values: new object[] {
                        article.Title,
                        article.Created,
                        article.Content
                    } 
                );
            }
            
            // migrationBuilder.InsertData(
            //     table: "articles",
            //     columns: new[] {"Title", "Created", "Content"},
            //     values: new object[] {
            //         "Bai viet 2",
            //         new DateTime(2022,2,27),
            //         "Noi dung 2"
            //     } 
            // );

            // migrationBuilder.InsertData(
            //     table: "articles",
            //     columns: new[] {"Title", "Created", "Content"},
            //     values: new object[] {
            //         "Bai viet 3",
            //         new DateTime(2002,5,23),
            //         "Noi dung 3"
            //     } 
            // );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
