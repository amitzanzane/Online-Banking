CREATE TABLE Customer (
Id int IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100),
DOB DATE,
Phone VARCHAR(12),
Email VARCHAR(50),
Address VARCHAR(200),
Username VARCHAR(20),
Password VARCHAR(20),
Reg_Date DATETIME
)

CREATE TABLE Branch (
Id int IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(20),
Description VARCHAR(100),
IFSC VARCHAR(10),
Phone VARCHAR(12)
)

CREATE TABLE Account (
Id int IDENTITY(1,1) PRIMARY KEY,
AccNumber VARCHAR(12),
AccType VARCHAR(10),
Reg_Date DATETIME,
Balance FLOAT(8),
CustId int FOREIGN KEY REFERENCES Customer(Id),
BranchId int FOREIGN KEY REFERENCES Branch(Id)
)

CREATE TABLE Transactions (
Id int IDENTITY(1,1) PRIMARY KEY,
TranDate DATETIME,
Amount FLOAT(8),
TranType VARCHAR(10),
AccId int FOREIGN KEY REFERENCES Account(Id)
)

INSERT INTO Branch VALUES ('Mumbai', 'GF, Gresham House United India Life Bldg, Sir Phirozshah Mehta Rd, Mumbai, Maharashtra 400023', 'SBIN007000', '044-22633164')
INSERT INTO Branch VALUES ('Chennai', '84 Rajaji Salai, Chennai, Tamilnadu 600 001', 'SBIN000080', '044-25220141')
INSERT INTO Branch VALUES ('Delhi', '11sansad Marg, New Delhi 110 001', 'SBIN000069', '011-23374050')
SELECT * FROM Branch