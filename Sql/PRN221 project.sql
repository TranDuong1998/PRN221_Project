create database PRN211_project
go

use PRN211_project
go

create table Accounts
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
	TeachersCode varchar(50),
	FullName nvarchar(100) ,
	Address nvarchar(200),
	Phone  varchar(20),
	DOB  date,

	constraint pk_Teachers primary key (TeacherId),
	constraint fk_TeacherAccount foreign key (AccountId) references Accounts (AccountId)
)
go

create table ClassRooms
(
	ClassId int identity(1,1),
	ClassName varchar(50),
	Description nvarchar(255)

	constraint pk_ClassRooms primary key (ClassId)
)
go

create table TeacherDetails
(
	Id int identity(1,1),
	TeacherId int,
	CourseId int,
	ClassId int

	constraint pk_TeacherDetails primary key (Id),
	constraint fk_TeacherDetailCourse foreign key (CourseId) references Courses (CourseId),
	constraint fk_TeacherDetailTeacher foreign key (TeacherId) references Teachers (TeacherId),
	constraint fk_TeacherDetailClassRooms foreign key (ClassId) references ClassRooms (ClassId),

)
go

create table TimeSlot
(
	TimeSlotId int identity(1,1),
	StartTime datetime,
	EndTime datetime,
	Description varchar(50)

	constraint pk_TimeSlot primary key (TimeSlotId)
)
go

create table Rooms
(
	RoomsId int identity(1,1),
	RoomsName nvarchar(50),
	Description nvarchar(255)

	constraint pk_Rooms primary key (RoomsId)
)
go

create table WeeklyTimeTable
(
	Id int identity(1,1),
	RoomsId int,
	TeachersId int,
	CourseId int,
	ClassId int,
	TimeSlotId int,
	LearnDate date,
	Description nvarchar(255),

	constraint pk_WeeklyTimeTable primary key (Id),
	constraint fk_WeeklyTimeTableTeachers foreign key (TeachersId) references Teachers (TeacherId),
	constraint fk_WeeklyTimeTableRooms foreign key (RoomsId) references Rooms (RoomsId),
	constraint fk_WeeklyTimeTableCourse foreign key (CourseId) references Courses (CourseId),
	constraint fk_WeeklyTimeTableClassRooms foreign key (ClassId) references ClassRooms (ClassId),
	constraint fk_WeeklyTimeTableTime foreign key (TimeSlotId) references TimeSlot (TimeSlotId)
)
go

insert into Accounts (UserName, Email, Password, Role, Active)
values				('admin','admin@gmail.com','12345678','admin', 1),
					('duong','duong@gmail.com','12345678','teacher', 1)

insert into Courses	(CourseName, CourseCode)
values			('Basic Cross-Platform Application Programming With .NET', 'PRN211'),
				('Mobile Programming', 'PRM392'),
				('SW Architecture and Design', 'SWD392')

insert into TimeSlot (StartTime, EndTime, Description)
values			('07:30:00', '09:50:00', 'Slot 1'),
				('10:00:00', '12:20:00', 'Slot 2'),
				('12:50:00', '15:10:00', 'Slot 3'),
				('15:20:00', '17:40:00', 'Slot 4')

insert into Teachers (AccountId, FullName, TeachersCode)
values				 (2,'Tran Van Duong','DuongTV98')

insert into Rooms (RoomsName)
values	('DC201'),('DC202'),('DC203'),('DC204'),('D205'),
		('D206'),('D207'),('D208'),('D209'),('D210')

insert into ClassRooms	(ClassName)
values	('SE1610'),('SE1611'),('SE1612'),('SE1613'),('SE1614'),
		('JD1610'),('JD1611'),('JD1612'),('JD1613'),('JD1614')