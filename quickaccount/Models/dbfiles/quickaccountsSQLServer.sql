-- create database quickaccounts﻿;

create TABLE Customer(
	Id int NOT NULL PRIMARY KEY Identity(1,1),
	Name varchar(50),
	Address varchar(100),
	Phone varchar(50),
	Email varchar(50),
    );
    
    create TABLE Vendor(
	Id int NOT NULL PRIMARY KEY Identity(1,1),
	Name varchar(50),
	Address varchar(100),
	Phone varchar(50),
	Email varchar(50)
    );

    
create table Finance_Account(
Id int NOT NULL PRIMARY KEY Identity(1,1),
Name varchar(30),
Finance_Account_Type varchar(50),
FK_Parent_Id int,
FOREIGN KEY (FK_Parent_Id) references Finance_Account(Id)
);


create table Finance_Transaction(
Id int NOT NULL PRIMARY KEY Identity(1,1),
Group_Id int,
Name varchar(50),
Amount float,
Status varchar(50),
DateTime datetime,
ChildOf int,
User_Type varchar(50),
FK_aspnetusers_Id nvarchar(450),
PaymentMethod varchar(30),
ReferenceNumber varchar(50),
Bank varchar(50),
Branch varchar(100),
ChequeDate datetime,
OtherDetail varchar(200),
OherDetails2 varchar(200),
User_Id int,
FK_Finance_Account_Id int,
foreign key (FK_Finance_Account_Id) references Finance_Account(Id),
foreign key (FK_aspnetusers_Id) references aspnetusers(Id)

);



create table Image(
Id int NOT NULL PRIMARY KEY Identity(1,1),
Scope varchar(50),
Name varchar(50),
Related_Id int,
Path varchar(200)
);


create table Settings(
Id int NOT NULL PRIMARY KEY Identity(1,1),
Name varchar(100),
Scope varchar(50),
UserType varchar(50),
UserId int,
FloatValue float,
DateValue float,
Boolvalue bit,
VarcharValue varchar(200),
VarcharValue2 varchar(200),
VarcharValue3 varchar(200)
);


SET IDENTITY_INSERT Finance_Account ON;
/* Asset Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(101,'Bank','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(1011,'Meezan Bank','Asset',101);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(1012,'Faisal Bank','Asset',101);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(102,'Cash','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(103,'Petty Cash','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(104,'Undeposited Fund','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(105,'Account Receivable','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(106,'Fixed Asset','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(107,'Current Asset','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(108,'Other Asset','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(109,'Inventory','Asset',null);
/*Liability Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(201,'Notes Payable','Liability',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(202,'Account Payable','Liability',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(203,'Tax Payable','Liability',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(204,'Salary Payable','Liability',null);
/*Equity Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(301,'Owner Equity','Equity',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(302,'Share Capital','Equity',null);
/*Income Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(401,'Product Sale','Income',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(402,'Service Sale','Income',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(403,'Other Income','Income',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(405,'Inventory Gain','Income',null);
/*Expence Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(501,'Operating Expence','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(502,'Salary','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(503,'Paid Tax','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(504,'Cost Of Good Sold','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(509,'Discount','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(510,'Other Expence','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(511,'Inventory Loss','Expence',null);

SET IDENTITY_INSERT Finance_Account OFF;

