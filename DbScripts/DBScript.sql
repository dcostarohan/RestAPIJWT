IF EXISTS(SELECT Object_Id('dbo.Products'))
BEGIN 
	DROP TABLE Products 
END 

Create Table Products(
	ProductId Int Identity(1,1) Primary Key,
	Name Varchar(100) Not Null,
	Category Varchar(100),
	Color Varchar(20),
	UnitPrice Decimal Not Null,
	AvailableQuantity Int Not Null
)
INSERT INTO Products 
SELECT 'PName1','PCategory1','PColor',4,1 UNION ALL
SELECT 'PName2','PCategory2','PColor',4,1 UNION ALL
SELECT 'PName3','PCategory3','PColor',4,1 UNION ALL
SELECT 'PName4','PCategory4','PColor',4,1 
GO

IF EXISTS(SELECT Object_Id('dbo.UserInfo'))
BEGIN 
	DROP TABLE UserInfo 
END 
Create Table UserInfo
(
	UserId Int Identity(1,1) Not null Primary Key,
	FirstName Varchar(30) Not null,
	LastName Varchar(30) Not null,
	UserName Varchar(30) Not null,
	Email Varchar(50) Not null,
	Password Varchar(20) Not null,
	CreatedDate DateTime Default(GetDate()) Not Null
)
GO

Insert Into UserInfo(FirstName, LastName, UserName, Email, Password) 
Values ('Admin', 'Admin', 'Admin', 'Admin@abc.com', 'Admin')

