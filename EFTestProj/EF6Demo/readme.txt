��������
Enable-Migrations
Add-Migration
Update-Database
Get-Migrations

//�����򽵼���ָ���汾
Update-Database �CTargetMigration: AddBlogUrl

//�������ݿ���½ű�
Update-Database -Script -SourceMigration:$InitialDatabase -TargetMigration: initdb

ʹ��Migrate.exe
//Ŀ¼
<project folder>\packages\EntityFramework.<version>\tools

//���������°汾
Migrate.exe MyMvcApplication.dll /startupConfigurationFile=��..\\web.config��

//������ָ���汾
Migrate.exe MyApp.exe /startupConfigurationFile=��MyApp.exe.config�� /targetMigration=��AddTitle��

//ָ��Ŀ¼
Migrate.exe MyApp.exe /startupConfigurationFile=��MyApp.exe.config�� /startupDirectory=��c:\\MyApp��

//ָ��configuration
Migrate.exe MyAssembly CustomConfig /startupConfigurationFile=��..\\web.config��

//�ṩ���ݿ������ַ���
Migrate.exe BlogDemo.dll /connectionString="Data Source=localhost;Initial Catalog=BlogDemo;Integrated Security=SSPI" /connectionProviderName="System.Data.SqlClient"




