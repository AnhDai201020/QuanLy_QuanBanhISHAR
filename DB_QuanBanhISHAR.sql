CREATE DATABASE [QL_QuanBanhISHAR]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QL_QuanBanhISHAR', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\QuanLyQuanIshar\QL_QuanBanhISHAR.mdf' , SIZE = 4096KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QL_QuanBanhISHAR_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\QuanLyQuanIshar\QL_QuanBanhISHAR_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET COMPATIBILITY_LEVEL = 110
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET ARITHABORT OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET  READ_WRITE 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET RECOVERY FULL 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET  MULTI_USER 
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QL_QuanBanhISHAR] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [QL_QuanBanhISHAR]
GO
IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [QL_QuanBanhISHAR] MODIFY FILEGROUP [PRIMARY] DEFAULT
GO
USE [QL_QuanBanhISHAR]
GO
--------------------------------------------------------------------------------------------------------------------------------------------------------------------
--TABLE
CREATE TABLE TABLEFOOD
(
	ID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(100) not null default N'ChuaDatTen',
	STATUS NVARCHAR(100) not null default N'Trong' -- TRONG || CO NGUOI
)
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
--ACCOUNT
CREATE TABLE ACCOUNT
(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) not null default N'Kter',
	PassWord Nvarchar(100) not null default 0,
	Type int not null default 0 --1 admin || 0 staff
)
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
--FOODCATEGORY
Create table FOODCATEGORY
(
	ID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(100) not null default N'ChuaDatTen'
)
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
--FOOD
Create table FOOD
(
	ID INT IDENTITY PRIMARY KEY,
	NAME NVARCHAR(100) default N'ChuaDatTen',
	idCategory int not null,
	prire float not null default 0
)
GO
alter table FOOD
	ADD CONSTRAINT fk_FOODCATEGOGY_FOOD
	FOREIGN KEY (idCategory)
	REFERENCES dbo.FOODCATEGORY(ID);
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
--BILL
create table BILL
(
	ID INT IDENTITY PRIMARY KEY,
	DateCheckIn date not null default GETDATE(),
	DateCheckOut date,
	idTable int not null,
	status int not null default 0 --1da thanh toan|| 0 chua thanh toan

	FOREIGN KEY (idTable) REFERENCES dbo.TABLEFOOD(ID)
)
go
alter table dbo.BILL
ADD discount int
go

update dbo.BILL set discount = 0
go

Alter table dbo.BILL add totalPrice float
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
--BILLINFO
create table BILLINFO
(
	ID INT IDENTITY PRIMARY KEY,
	idBill int not null,
	idFood int not null,
	count int not null default 0
)
go
alter table BILLINFO
	ADD CONSTRAINT fk_BILL_BILLINFO
	FOREIGN KEY (idBill)
	REFERENCES dbo.BILL(ID);
go
alter table BILLINFO
	ADD CONSTRAINT fk_FOOD_BILLINFO
	FOREIGN KEY (idFood)
	REFERENCES dbo.FOOD(ID);
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Thêm Account
Use QL_QuanBanhISHAR
Insert into dbo.ACCOUNT (UserName, DisplayName, PassWord, Type) values (N'QL1', N'QuanLy01', N'0', 1)
Insert into dbo.ACCOUNT (UserName, DisplayName, PassWord, Type) values (N'NV1', N'NhanVien01', N'0', 0)
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Thêm Bàn
Declare @i int =0
while @i <= 15
begin
	insert dbo.TABLEFOOD( NAME) values (N'Bàn '+ cast(@i as nvarchar(100)))
	SET @i = @i + 1
end
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Thêm Category
INSERT dbo.FOODCATEGORY (NAME) values ( N'Chocolate')
go
INSERT dbo.FOODCATEGORY (NAME) values ( N'Matcha')
go
INSERT dbo.FOODCATEGORY (NAME) values ( N'Pudding')
go
INSERT dbo.FOODCATEGORY (NAME) values ( N'Bread')
go
INSERT dbo.FOODCATEGORY (NAME) values ( N'Cake')
go
--------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Thêm Món Ăn
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Chocolate Viên',1,15000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Chocolate Viên - Cheese',1,15000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Flan-Cafe',3,7000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Flan-Matcha',2,12000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Flan-ĐB',3,16000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Crepe Matcha',2,55000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Cookie Matcha Hạnh Nhân',2,45000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh cuộn Matcha',2,12000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Mì Nhân Thịt',4,15000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Mì Nhân Đậu Xanh',4,10000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Mì Nhân Khoai Môn',4,13000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Paparoti',4,12000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Kem Mini',5,70000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Kem Size L',5,260000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Bánh Kem Fruit',3,450000)
go
INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'Kem Matcha',2,36000)
go

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Stored Procedures
create proc USP_GetListAccountByUserName
@userName nvarchar(100)
AS
Begin
	SELECT * FROM dbo.ACCOUNT where UserName = @userName
end
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_Login
@username nvarchar(100), @password nvarchar(100)
as
begin
	Select * from dbo.Account where UserName = @username and PassWord = @password
end
go
--------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_GetTableList
as select * from dbo.TABLEFOOD
go
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_InsertBill
@idTable int
as
begin
	insert dbo.BILL (DateCheckIn, DateCheckOut, idTable, status, discount) values ( GETDATE(), NULL, @idTable , 0, 0)
end
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExitsBillInfo int;
	declare @foodcount int = 1;
	select @isExitsBillInfo = id, @foodcount = b.count from dbo.BILLINFO as b where idBill = @idBill and idFood = @idFood
	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodcount + @count
		if(@newCount > 0)
			update dbo.BILLINFO set count = @foodcount + @count where idFood = @idFood
		else
			delete dbo.BILLINFO where idBill = @idBill and idFood = @idFood
	end
	else
	begin
		INSERT dbo.BILLINFO (idBill, idFood, count) values ( @idBill, @idFood, @count)
	end
end
go
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create PROC USP_SwitchTable
@idTable1 int, @idTable2 int
as
begin
	declare @idFirstBill int
	declare @idSeconrdBill int

	declare @isFirstTableEmty int = 1
	declare @isSeconrdTableEmty int = 1
	SELECT @idFirstBill = id FROM dbo.BILL WHERE idTable = @idTable1 AND status = 0
	SELECT @idSeconrdBill = id FROM dbo.BILL WHERE idTable = @idTable2 AND status = 0

	if(@idFirstBill is null)
	begin
		INSERT dbo.BILL (DateCheckIn, DateCheckOut, idTable, status) values ( GETDATE(), NULL, @idTable1,0)
		select @idFirstBill = Max(id) from dbo.BILL where idTable = @idTable1 AND status = 0
	end

	select @isFirstTableEmty = count(*) from dbo.BILLINFO where idBill = @idFirstBill
	
	if(@idSeconrdBill is null)
	begin
		INSERT dbo.BILL (DateCheckIn, DateCheckOut, idTable, status) values ( GETDATE(), NULL, @idTable2,0)
		select @idSeconrdBill = Max(id) from dbo.BILL where idTable = @idTable2 AND status = 0
	end

	select @isSeconrdTableEmty = count(*) from dbo.BILLINFO where idBill = @idSeconrdBill

	select id INTO IDBillInfoTable  from dbo.BILLINFO where idBill = @idSeconrdBill

	update dbo.BILLINFO set idBill = @idSeconrdBill where idBill = @idFirstBill
	update dbo.BILLINFO set idBill = @idFirstBill where id IN (select * from IDBillInfoTable)

	Drop Table IDBillInfoTable

	if(@isFirstTableEmty = 0)
		update dbo.TABLEFOOD set status = N'Trong' where id = @idTable2
	if(@isSeconrdTableEmty = 0)
		update dbo.TABLEFOOD set status = N'Trong' where id = @idTable1
end
go
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_GetListBillByDate
@checkin date, @checkOut date
as
begin
	select t.NAME as [Tên Bàn], b.totalPrice as [Tổng Tiền], DateCheckIn as [Ngày Vào], DateCheckOut as [Ngày Ra], discount as [Giảm Giá] 
	from dbo.BILL as b, dbo.TABLEFOOD  as t
	where DateCheckIn >= @checkin and DateCheckOut <= @checkOut and b.status = 1 
	and t.ID = b.idTable 
end
go
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create PROC USP_UpdateAccount
@userName nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0

	select @isRightPass = count(*) from dbo.ACCOUNT where UserName = @userName and PassWord = @password
	 if (@isRightPass = 1)
	 begin
		if(@newPassword = null or @newPassword = '')
			begin
				update dbo.ACCOUNT set DisplayName = @displayName where UserName = @userName
			end
			else
				update dbo.ACCOUNT set DisplayName = @displayName, PassWord = @newPassword where UserName = @userName
	 end
end
go
--------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_GetListBillByDateAndPage
@checkin date, @checkOut date, @page int
as
begin
	declare @pageRows int = 10
	declare @selectRows int = @pageRows
	declare @exceptRows int = (@page - 1) * @pageRows

	;with BillShow as (select b.ID, t.NAME as [Tên Bàn], b.totalPrice as [Tổng Tiền], DateCheckIn as [Ngày Vào], DateCheckOut as [Ngày Ra], discount as [Giảm Giá] 
	from dbo.BILL as b, dbo.TABLEFOOD  as t
	where DateCheckIn >= @checkin and DateCheckOut <= @checkOut and b.status = 1 
	and t.ID = b.idTable )

	select TOP (@selectRows) * from BillShow where id not in (select TOP (@exceptRows) id from BillShow)
end
go
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Proc USP_GetNumBillByDate
@checkin date, @checkOut date
as
begin
	select count(*)
	from dbo.BILL as b, dbo.TABLEFOOD  as t
	where DateCheckIn >= @checkin and DateCheckOut <= @checkOut and b.status = 1 
	and t.ID = b.idTable 
end
go
--------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Trigger
Create TRIGGER UTG_UpdateBillInfo
on dbo.BILLINFO for INSERT, UPDATE
as
begin
	declare @idBill int

	select @idBill = idBill from Inserted

	declare @idTable int

	select @idTable = idTable from dbo.BILL where id = @idBill and status = 0

	declare @count int
	select @count = count(*) from dbo.BILLINFO where idBill = @idBill

	if(@count >0)
		UPDATE dbo.TABLEFOOD set STATUS = N'Co Khach' where id = @idTable
	else
		UPDATE dbo.TABLEFOOD set STATUS = N'Trong' where id = @idTable
end
go
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create TRIGGER UTG_UpdateBill
on dbo.BILL for UPDATE
AS
BEGIN
	declare @idBill int
	select @idBill = id from inserted
	declare @idTable int
	select @idTable = idTable from dbo.BILL where id = @idBill
	declare @count int = 0
	select @count = count(*) from dbo.BILL where idTable = @idTable and status = 0
	if(@count = 0)
		UPDATE dbo.TABLEFOOD set STATUS = N'Trong' where id = @idTable
END
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create Trigger UTG_DeleteBillInfo
on dbo.BILLINFO for delete
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBill = deleted.idBill  from deleted

	declare @idTable int
	select @idTable = idTable from dbo.BILL where id = @idBill
	
	declare @count int = 0
	select @count = count(*) from dbo.BILLINFO as bi, dbo.BILL as b where b.ID = bi.idBill and b.ID = @idBill and status = 0

	if(@count = 0)
		Update dbo.TABLEFOOD set status = N'Trong' where  id = @idTable
end
go




