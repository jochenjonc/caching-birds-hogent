/* Create database */ 
IF (db_id(N'CachingBirds') IS NULL)
	BEGIN
        CREATE DATABASE [CachingBirds]
    END
GO

/* Switch to the newly created database */
USE [CachingBirds]
GO

/* Create a login and user with read/write access */
/*   Users are typically mapped to logins, as OP's question implies, so make sure an appropriate login exists. */
IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = 'Cuckoo')
	BEGIN
		/* Syntax for SQL server login.  See BOL for domain logins, etc. */
		CREATE LOGIN [Cuckoo] WITH PASSWORD = 'T1mekeeper'
	END
GO

/*   Create the user for the specified login. */
IF NOT EXISTS(SELECT principal_id FROM sys.database_principals WHERE name = 'Cuckoo')
	BEGIN
		CREATE USER [Cuckoo]
		FOR LOGIN [Cuckoo]
		WITH DEFAULT_SCHEMA=[dbo] 
	END
GO

/*   Assign the right roles */
IF EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = 'Cuckoo')
	BEGIN
		ALTER ROLE [db_datareader] ADD MEMBER [Cuckoo]
		ALTER ROLE [db_datawriter] ADD MEMBER [Cuckoo]
	END
GO
