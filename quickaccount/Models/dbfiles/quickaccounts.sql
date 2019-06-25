-- create database quickaccounts﻿;

create TABLE Customer(
	Id int NOT NULL PRIMARY KEY auto_increment,
	Name varchar(50),
	Address varchar(100),
	Phone varchar(50),
	Email varchar(50)
    );
    
    create TABLE Vendor(
	Id int NOT NULL PRIMARY KEY auto_increment,
	Name varchar(50),
	Address varchar(100),
	Phone varchar(50),
	Email varchar(50)
    );

    
create table Finance_Account(
Id int NOT NULL PRIMARY KEY auto_increment,
Name varchar(30),
Finance_Account_Type varchar(50),
FK_Parent_Id int,
FOREIGN KEY (FK_Parent_Id) references Finance_Account(Id)
);


create table Finance_Transaction(
Id int NOT NULL PRIMARY KEY auto_increment,
Group_Id int,
Name varchar(50),
Amount float,
Status varchar(50),
DateTime datetime,
ChildOf int,
User_Type varchar(50),
FK_aspnetusers_Id varchar(200),
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
Id int NOT NULL PRIMARY KEY auto_increment,
Scope varchar(50),
Name varchar(50),
Related_Id int,
Path varchar(200)
);


create table Settings(
Id int NOT NULL PRIMARY KEY auto_increment,
Name varchar(100),
Scope varchar(50),
UserType varchar(50),
UserId int,
FloatValue float,
DateValue float,
Boolvalue boolean,
VarcharValue varchar(200),
VarcharValue2 varchar(200),
VarcharValue3 varchar(200)
);


/* Asset Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(101,'Bank','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(1011,'Meezan Bank','Asset',101);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(1012,'Faisal Bank','Asset',101);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(102,'Cash','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(103,'Petty Cash','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(104,'Undeposited Funds','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(105,'Account Receivables','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(106,'Fixed Assets','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(107,'Current Assets','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(108,'Other Assets','Asset',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(109,'Inventory','Asset',null);
/*Liability Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(201,'Notes Payable','Liability',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(202,'Account Payable','Liability',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(203,'Tax Payable','Liability',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(204,'Salaries Payable','Liability',null);
/*Equity Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(301,'Owner Equity','Equity',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(302,'Share Capital','Equity',null);
/*Income Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(401,'Products Sale','Income',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(402,'Services Sale','Income',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(403,'Other Income','Income',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(405,'Inventory Gain','Income',null);
/*Expence Accounts */
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(501,'Operating Expence','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(502,'Salaries','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(503,'Paid Taxes','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(504,'Cost Of Goods Sold','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(509,'Discounts','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(510,'Other Expence','Expence',null);
insert into Finance_Account(Id,Name,Finance_Account_Type,FK_Parent_Id) values(511,'Inventory Loss','Expence',null);


-- demo data
insert into Vendor(Name) Values ('Vendor 1'),('Vendor 2'),('Vendor 3');
insert into Customer(Name) Values ('Customer 1'),('Customer 2'),('Customer 2');

