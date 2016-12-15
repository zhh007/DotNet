常用命令
Enable-Migrations
Add-Migration
Update-Database

//升级或降级到指定版本
Update-Database –TargetMigration: AddBlogUrl

//生成数据库更新脚本
Update-Database -Script -SourceMigration:$InitialDatabase -TargetMigration: initdb
