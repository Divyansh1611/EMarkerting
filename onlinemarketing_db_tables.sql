create database onlinemarketing;
use onlinemarketing;


create table admin (
	ad_id int primary key identity,
	ad_name nvarchar (50) not null unique,
	ad_password nvarchar(50) not null unique,
)
----------------------------------------------------
insert into admin  values ('Divyansh', 'abc123');
insert into admin  values ('Div', 'abc124');

----------------------------------------------------
create table category (
	cat_id int primary key identity,
	cat_name nvarchar(50) not null,
	cat_image nvarchar(max) not null, 
	ad_ID_FK int foreign key references admin(ad_id)
	)
----------------------------------------------------

create table tbl_user (
	u_id int primary key identity,
	u_name nvarchar(50) not null,
	u_password nvarchar(50) not null,
	u_image nvarchar(max) not null, 
	u_contact nvarchar(50) not null unique,
	u_email nvarchar(50) not null unique
)

-----------------------------------------

create table product (
	pro_id int primary key identity,
	pro_name nvarchar(50) not null,
	pro_image nvarchar(max) not null, 
	pro_price int, 
	pro_desc nvarchar(max) not null,
	cat_id_fk int foreign key references category (cat_id),
	pro_user_id_FK int foreign key references tbl_user(u_id)
	)