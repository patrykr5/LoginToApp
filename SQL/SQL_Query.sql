CREATE DATABASE Loginapp;
BEGIN TRANSACTION;

CREATE TABLE Loginapp_users (
  ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  Login VARCHAR(15) NOT NULL,
  Password VARCHAR(255) NOT NULL
);
BEGIN TRANSACTION;

-- Useful
exec sp_columns Loginapp_users;
SELECT * FROM sys.database_principals;
select * from sys.all_objects where type_desc = 'user_table';
select * from Loginapp_users;
exec usp_Auth user1;
-- end

INSERT INTO Loginapp_users (Login, Password) VALUES ('user1', '6ec2822e18df025a7fd4200af421ea41'); --Password = md5 hash from string Xmxy4Z9Goe
BEGIN TRANSACTION;

-- Creating user to application credentials (connectionString) // I granted privileges from user from SSMS GUI.
use LoginApp
GO
CREATE USER app_la for login app_la with DEFAULT_SCHEMA = dbo
go;
-- end

CREATE PROCEDURE usp_Auth @Login varchar(15)
  AS
  SELECT Password
    FROM Loginapp_users WHERE Login = @Login;
COMMIT;

CREATE TABLE LoginApp_users_login_history (
  ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  Login VARCHAR(15) NOT NULL,
  Login_Time DATETIME2 DEFAULT GETDATE()
)
BEGIN TRAN;
COMMIT;

ALTER PROCEDURE usp_Auth @Login varchar(15)
  AS
  INSERT INTO LoginApp_users_login_history (Login) VALUES (@Login);
  SELECT Password
    FROM Loginapp_users WHERE Login = @Login;
BEGIN TRANSACTION;
COMMIT;