*install các package sau: *

dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

*tạo Dorcker/docker-compose.yml*
version: "3.7"                    

services:                         
  xtlab-mssql:
    image: "mcr.microsoft.com/mssql/server:2017-latest"
    container_name: sqlserver-noname
    restart: always
    hostname: mssql
    environment: 
      SA_PASSWORD: Password123 #Thiết lập password
      ACCEPT_EULA: 'Y'
      # Express: 

    volumes:
      - mssqlvolume:/var/opt/mssql/data # thư mục lưu DB
      # - ./bk:/var/opt/mssql/backup
    ports:
      - "1433:1433"     
                        
volumes:                                
    mssqlvolume:        
      name: sqlserver-noname-vl
                    
run: docker up -d

*trong appsettings.json*
InitialCatalog: tên cơ sỡ dữ liệu

"ConnectionStrings": {
    "MyBlogContext": "Data Source=localhost,1433; Initial Catalog=razorwebdb; User ID=SA;Password=Password123; TrustServerCertificate=True;"
  }

*thêm config vào Program.cs*
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>(options =>{
    string connectString = builder.Configuration.GetConnectionString("MyBlogContext");
    options.UseSqlServer(connectString);
});

*dotnet ef  database drop -f*
*dotnet ef database update*
*dotnet ef migrations list*
*dotnet ef migrations add initdb*
*Thêm EF Faker Data Generate*
 dotnet add package Bogus --version 34.0.2   

 dotnet new page -o Pages -n Contact -p:n ValidationExample.Pages   


 1. Models Folder
    - Khai báo lớp namespace: razorweb.models[tùy ý đặt]
    -lớp public: [ABC] model biểu diễn lớp abc.. được sử dụng trong DbContext
        trong lớp có các properties ID, Name , ,....
    - thiết lập các Attributes,
    [Key] Khóa chính
    [StringLength(255)] độ dài max 255
    [Required] bắt buộc
    [Column(TypeName = "nvarchar")] lưu ở trên sql server kiểu dữ liệu gì *using System.ComponentModel.DataAnnotations.Schema;*
    [DataType(DataType.Date)] Kiểu dữ liệu ngày
    [Column(TypeName = "ntext")]
    Nơi khai báo model (Article or MyBlogContext)

    - thiết lập tên trên sql server thì có thể dùng [Table("posts")] <==> public class Article
    - tiếp theo là phải tạo thêm 1 DbContext biểu diễu cấu trúc của cơ sở dữ liệu*cũng khai *cũng khai báo trong namespace razorweb.models*
    *Models/MyBlogContext.cs*

    - với khai báo từ dòng 81 và 95 thì cơ sở dữ liệu tạo ra sẽ có bảng là Article, chứa các dòng tương ứng với cả model chứa các kiểu dữ liệu trong Article
    - chỉ rõ Contructor dành cho MyBlogContext, khởi tạo kèm với lớp cơ sở của DbContext

    - khai báo đúng để còn đăng kí thành 1 dv trong DRI của ứng dụng
    ***
            using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;
            using Microsoft.EntityFrameworkCore;

            namespace razorweb.models{
                *Kế thừa từ lớp DbContext*
                public class MyBlogContext : DbContext{

                    public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options){
                        //.......
                    }

                    protected override void OnConfiguring(DbContextOptionsBuilder builder)
                    {
                        base.OnConfiguring(builder);
                    }
                    protected override void OnModelCreating(ModelBuilder modelBuilder){
                        base.OnModelCreating(modelBuilder);
                    }
                    *Tập hợp chứa các phần tử Article*
                    public DbSet<Article> articles {get; set; }
                }
            }
    ***





    *Models/Article.cs*
    ***
            using System;
            using System.ComponentModel.DataAnnotations;
            using System.ComponentModel.DataAnnotations.Schema;

            namespace razorweb.models
            {
                // [Table("post")]
                public class Article {
                    [Key]
                    public int Id{ get; set;}

                    [StringLength(255)]
                    [Required]
                    [Column(TypeName="nvarchar")]
                    public string Title{ get; set;}
                    [DataType(DataType.Date)]
                    [Required]
                    public DateTime Created{ get; set;}
                    [Column(TypeName ="ntext")]
                    public string Content { get; set;}

                }
            }
***


    2.Khai báo vào appsettings.json
    3. Đọc các giá trị thiết lập [2] bằng cách thết lập trong Program.cs thông qua IConfiguration


    4. Migrations sau khi exec: dotnet ef migrations intidb[có thể thay đổi] thì sẽ xuất

        *dotnet ef migrations add initdb*
    - khởi tạo migrations đàu tiên , là ảnh chụp ctcsdl hiện tại
    *dotnet ef migrations list*
   
    *Thêm EF Faker Data Generate*

 File initdb.cs (file gốc )
        Dùng để tạo csdl , cấu trúc cho csdl
         *dotnet ef database update*

    khi dùng migrations của initdb
         trong initbd có phương thức UP: dùng để tạo ra bảng có tên [Article] và cấu trúc bảng gồm cột gì,
            sau đó chèn 1 số csdl

        xóa;  *dotnet ef  database drop -f*

        ******
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
        ******



        dùng Bogus để phát sinh , những dữ liệu giả định (mẫu)




        trong View: Index.cshtml.cs
            private readonly MyBlogContext myBlogContext;
            public IndexModel(ILogger<IndexModel> logger, MyBlogContext _myContext)
            {
                _logger = logger;
                myBlogContext = _myContext;
            }
            public void OnGet()
            {
                var posts = (from a in myBlogContext.articles
                            orderby a.Created descending
                            select a).ToList();
                ViewData["posts"] = posts;
            }

        trong View: Index.cshtml
            @using razorweb.models
            @{
                ViewData["Title"] = "Home page";

                var posts = ViewData["posts"] as List<Article>;
            }
            <ol>
                @foreach(var post in posts){
                    <li> @post.Title </li>
                }
            </ol>
