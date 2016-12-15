常用命令
Enable-Migrations
Add-Migration
Update-Database
Get-Migrations

//升级或降级到指定版本
Update-Database –TargetMigration: AddBlogUrl

//生成数据库更新脚本
Update-Database -Script -SourceMigration:$InitialDatabase -TargetMigration: initdb

使用Migrate.exe
//目录
<project folder>\packages\EntityFramework.<version>\tools

//升级到最新版本
Migrate.exe MyMvcApplication.dll /startupConfigurationFile=”..\\web.config”

//升级到指定版本
Migrate.exe MyApp.exe /startupConfigurationFile=”MyApp.exe.config” /targetMigration=”AddTitle”

//指定目录
Migrate.exe MyApp.exe /startupConfigurationFile=”MyApp.exe.config” /startupDirectory=”c:\\MyApp”

//指定configuration
Migrate.exe MyAssembly CustomConfig /startupConfigurationFile=”..\\web.config”

//提供数据库链接字符串
Migrate.exe BlogDemo.dll /connectionString="Data Source=localhost;Initial Catalog=BlogDemo;Integrated Security=SSPI" /connectionProviderName="System.Data.SqlClient"




