create database PRN211_project
go

use PRN211_project
go

create table Account
(
	AccountId int identity(1,1),
	UserName varchar(50) unique,
	Email varchar(100) unique,
	Password  varchar(32),
	Role  varchar(10),
	Active bit,

	constraint pk_Account primary key (AccountId)
)
go

create table Courses
(
	CourseId int identity(1,1),
	CourseName nvarchar(255),
	CourseCode varchar(50) unique,

	constraint pk_Course primary key (CourseId)
)
go

create table Teachers
(
	TeacherId int identity(1,1),
	AccountId int,
	TeachersCode int,
	FullName nvarchar(100) ,
	Address nvarchar(200),
	Phone  varchar(20),
	DOB  date,

	constraint pk_Teachers primary key (TeacherId),
	constraint fk_TeacherAccount foreign key (AccountId) references Account (AccountId)
)
go

create table TeacherDetails
(
	Id int identity(1,1),
	TeacherId int,
	CourseId int

	constraint pk_TeacherDetails primary key (Id),
	constraint fk_TeacherDetailCourse foreign key (CourseId) references Courses (CourseId),
	constraint fk_TeacherDetailTeacher foreign key (TeacherId) references Teachers (TeacherId)

)
go

create table TimeSlot
(
	TimeSlotId int identity(1,1),
	StartTime datetime,
	EndTime datetime

	constraint pk_TimeSlot primary key (TimeSlotId)
)
go

create table Rooms
(
	RoomsId int identity(1,1),
	RoomsName nvarchar(50),
	TimeSlotId int

	constraint pk_Rooms primary key (RoomsId),
	constraint fk_RoomsTime foreign key (TimeSlotId) references TimeSlot (TimeSlotId)
)
go

create table WeeklyTimeTable
(
	Id int identity(1,1),
	RoomsId int,
	TeachersId int,
	Description nvarchar(255),

	constraint pk_WeeklyTimeTable primary key (Id)

)
go

insert into Account (UserName, Email, Password, Role, Active)
values				('admin','admin@gmail.com','12345678','admin', 1),
					('duong','duong@gmail.com','12345678','user', 1)

insert into Courses(CourseName, CourseCode)
values			('Basic Cross-Platform Application Programming With .NET', 'PRN211'),
				('Mobile Programming', 'PRM392'),
				('SW Architecture and Design', 'SWD392')

insert into TimeSlot(StartTime, EndTime)
values			('07:30:00', '09:50:00'),
				('10:00:00', '12:20:00'),
				('12:50:00', '15:10:00'),
				('15:20:00', '17:40:00')

insert into Teachers (AccountId, FullName, TeachersCode)
values				 (2,'Tran Van Duong','DuongTV98')