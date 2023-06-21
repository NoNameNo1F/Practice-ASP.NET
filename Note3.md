REQUIRED: 
    dotnet tool uninstall --global dotnet-aspnet-codegenerator
    dotnet tool install --global dotnet-aspnet-codegenerator
    dotnet tool uninstall --global dotnet-ef
    dotnet tool install --global dotnet-ef
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer

    REQUIRED THIS PACKAGE
    *dotnet add package Microsoft.EntityFrameworkCore.Tools*

CRUD: dotnet aspnet-codegenerator razorpage -m {namespace}.{Model} -dc {namespace}.{database context} -outDir
vd dotnet aspnet-codegenerator razorpage -m razorweb.models.Article -dc razorweb.models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries


để dùng DisplayName trong lớp model =>> using System.ComponentModel;


!!! *lƯU Ý*
@* @page "/blog/chinhsuabaiviet/int?:{id}" *@ 
@page "/blog/chinhsuabaiviet/{id:int}"

DÒNG 1 SẼ RETURN LẠI LỖI SAU:
        An unhandled exception occurred while processing the request.
        RoutePatternException: The literal section 'int?:' is invalid. Literal sections cannot contain the '?' character.
*DÒNG 2 ĐỂ KHẮC PHỤC*