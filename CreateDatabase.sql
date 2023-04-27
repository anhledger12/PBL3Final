--drop database LibraryManagement

--Create database LibraryManagement

Use LibraryManagement
go


-- Bảng tài khoản
create table Account(
	AccName varchar(50) primary key not null,
	FullName nvarchar(200),
	DateOfBirth date,
	Phone varchar(20),
	Email varchar(100),
	CCCD varchar(20)
);

Insert Into Account(AccName,FullName,DateOfBirth,Phone,Email,CCCD)
Values('User1',N'Trần Viết Sơn', '10/11/2000','0796688018','sontranviet21@gmail.com','068203114024')

Insert Into Account(AccName,FullName,DateOfBirth,Phone,Email,CCCD)
Values('User2',N'Trương Tấn Cường', '11/21/2000','0123535622','cuongtruong@gmail.com','0682032144024')

Insert Into Account(AccName,FullName,DateOfBirth,Phone,Email,CCCD)
Values('User3',N'Nguyễn Nguyên Anh', '10/11/2000','0731668018','anhledge03@gmail.com','0991155514024')

-- Bảng đăng nhập
-- drop table AccountLogin
create table AccountLogin(
	AccName varchar(50) primary key,
	PassHashCode varchar(MAX),
	Permission bit
	Foreign key (AccName) references Account(AccName)
)

Insert into AccountLogin(AccName,PassHashCode,Permission)
Values ('User1','abcxyz', 0)

Insert into AccountLogin(AccName,PassHashCode,Permission)
Values ('User2','abcxyz', 1)

Insert into AccountLogin(AccName,PassHashCode,Permission)
Values ('User3','abcxyz', 1)

-- end

-- Bảng Thông báo

create table Notificate(
	Id int Identity(1,1) primary key not null,
	AccReceive varchar(50),
	TimeSending date,
	Content nvarchar(MAX),
	StateRead bit,
	foreign key (AccReceive) references Account(AccName) 
)

Insert into Notificate(AccReceive,TimeSending,Content,StateRead)
Values ('User1','4/12/2023',N'Làm tốt lắm em',0)

Insert into Notificate(AccReceive,TimeSending,Content,StateRead)
Values ('User3','4/13/2023',N'Chưa trả sách em ey',0)

Insert into Notificate(AccReceive,TimeSending,Content,StateRead)
Values ('User2','4/14/2023',N'Không mượn sách à em',0)
	
-- end


--Table Lịch sử hoạt động 
-- drop table Actionlog
create table ActionLog(
	Id int Identity(1,1) primary key not null,
	Acc varchar(50),
	[Time] datetime,
	Content nvarchar(MAX),
	foreign key (Acc) references Account(AccName) 
)

insert into ActionLog(Acc,Time,Content)
Values ('User1','1/1/2020',N'Gửi đơn mượn ID = 1')

insert into ActionLog(Acc,Time,Content)
Values ('User2','1/2/2020',N'Duyệt đơn mượn ID = 1')

-- end

-- table đơn mượn

create table BookRental(
	Id int Identity(1,1) primary key not null,
	AccApprove varchar(50),
	AccSending varchar(50),
	TimeCreate date,-- ngày tạo đơn mượn
	foreign key (AccApprove) references Account(AccName),
	foreign key (AccSending) references Account(AccName) 
)

insert into BookRental(AccApprove,AccSending,TimeCreate)
values ('User2','User1','1/1/2020')

--end


-- table republish

create table Republish(
	Id int identity(1,1) primary key,
	NameRepublisher nvarchar(MAX),
	NumOfRep int,
	TimeOfRep datetime
)
Insert into Republish(NameRepublisher,NumOfRep,TimeOfRep)
values (N'Kim Đồng',1,'1/1/2000') 

Insert into Republish(NameRepublisher,NumOfRep,TimeOfRep)
values (N'Kim Ngoa',1,'1/1/2000')

Insert into Republish(NameRepublisher,NumOfRep,TimeOfRep)
values (N'Thanh niên',1,'1/1/2000')
-- end

-- table Hashtag
create table Hashtag(
	Id int identity(1,1) primary key,
	NameHashTag nvarchar(MAX),
)
Insert into Hashtag(NameHashTag)
values (N'Trinh Tham')
Insert into Hashtag(NameHashTag)
values (N'Lich su')
Insert into Hashtag(NameHashTag)
values (N'Cong nghe')
--end



-- table title
create table Title(
	IdTitle varchar(50) primary key,
	IdRepublish int,
	NameBook nvarchar(max),
	NameWriter nvarchar(max),
	ReleaseDate date,
	NameBookshelf nvarchar(max),
	foreign key (IdRepublish) references Republish(Id)
)

insert into Title
values ('TCC',1,N'Toán cao cấp',N'Vũ Hữu Bình','1/1/2000',N'Giáo khoa')
insert into Title
values ('VL',2,N'Vật Lý 12',N'Vũ Trọng','1/1/2000',N'Giáo khoa')
insert into Title
values ('NDT',3,N'Người đưa thư',N'Vũ Hữu Bình','1/1/2000',N'Giáo khoa')
--end


-- table Hashtag_Title
create table Hashtag_title(
	IdHashtag int,
	IdTitle varchar(50),
	constraint PK_ID primary key (IdHashtag,IdTitle),
	foreign key (Idhashtag) references HashTag(id),
	foreign key (IdTitle) references Title(idTitle),
)

insert into Hashtag_title
values(1,'TCC')
insert into Hashtag_title
values(1,'VL')
insert into Hashtag_title
values(2,'NDT')
insert into Hashtag_title
values(2,'TCC')
--end

-- Table Book 
create table Book(
	IdBook varchar(50) primary key,
	IdTitle varchar(50),
	StateRent bit default 0, -- 1 nếu đã được mượn
	foreign key (IdTitle) references Title(idTitle),
)
insert into Book
values ('TCC.1','TCC',1)
insert into Book
values ('TCC.2','TCC',1)
insert into Book
values ('TCC.3','TCC',1)
--end


--table detail don muon

create table BookRentDetail(
	IdBookRental int,
	IdBook varchar(50),
	StateReturn bit default 0, -- 1 nếu đã trả 
	StateApprove bit default 0, -- 1 nếu đã duyệt sách
	StateTake bit default 0, -- 1 nếu người mượn đã nhận sách
	ReturnDate datetime,
	constraint PK_IdBookRent_IdBook primary key (IdBookRental asc, IdBook asc),
	foreign key (IdBookRental) references BookRental(Id),
	foreign key (IdBook) references Book(IdBook),
)
insert into BookRentDetail
values(1,'TCC.1',0,0,0,'1/10/2000')

insert into BookRentDetail
values(1,'TCC.2',0,0,0,'1/10/2000')

insert into BookRentDetail
values(1,'TCC.3',0,0,0,'1/10/2000')
--end

/*

select * from titleX
*/
