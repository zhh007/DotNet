��������
Enable-Migrations
Add-Migration
Update-Database

//�����򽵼���ָ���汾
Update-Database �CTargetMigration: AddBlogUrl

//�������ݿ���½ű�
Update-Database -Script -SourceMigration:$InitialDatabase -TargetMigration: initdb
