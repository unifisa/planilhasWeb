IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'sa')
BEGIN
    CREATE LOGIN [sa] 
      FROM WINDOWS WITH DEFAULT_DATABASE=[master], 
      DEFAULT_LANGUAGE=[us_english]
END
GO
CREATE USER [ContosoUniversityUser] 
  FOR LOGIN [sa]
GO
EXEC sp_addrolemember 'db_owner', 'ContosoUniversityUser'
GO