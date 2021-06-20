USE [BankSystem]
GO

/****** Object:  StoredProcedure [dbo].[INSERT_INTO_BANK_TABLES]    Script Date: 20-06-2021 20:32:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_INTO_BANK_TABLES]
(
	-- Add the parameters for the stored procedure here
	@name	 varchar(100),
	@dob	 date,
	@phone	 varchar(12),
	@email	 varchar(50),
	@address varchar(200),
	@username varchar(20),
	@password varchar(20),
	--@accNumber varchar,
	@accType varchar(10),
	@branchId int
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO Customer (Name, DOB, Phone, Email, Address, Username, Password, Reg_Date) VALUES (
	@name,
	@dob,
	@phone,
	@email,
	@address,
	@username,
	@password,
	sysdatetime()
	)

	Declare @id int
	Declare @accNumber varchar(12)
	SET @accNumber = null

	SELECT @id = Id FROM Customer WHERE Username = @username

	SELECT Top 1 @accNumber = AccNumber FROM Account ORDER BY Id DESC
	
	IF (@accNumber is null)
		SET @accNumber = '100000000000'
	ELSE
		SELECT @accNumber = CAST((CAST(@accNumber AS bigint) + 1) AS varchar)

	INSERT INTO Account (AccNumber, AccType, Reg_Date, Balance, CustId, BranchId) VALUES (
	@accNumber,
	@accType,
	sysdatetime(),
	3000.00,
	@id,
	@branchId
	)

END
GO


