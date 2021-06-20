USE [BankSystem]
GO

/****** Object:  StoredProcedure [dbo].[ADD_TRANSACTION_RECORD]    Script Date: 20-06-2021 20:31:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ADD_TRANSACTION_RECORD]
	-- Add the parameters for the stored procedure here
(
	@Username varchar(20),
	@AccNumber varchar(12),
	@Amount float
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @id1 int	
	DECLARE @id2 int	

    -- Insert statements for procedure here
	SELECT @id1 = Id FROM Account WHERE AccNumber = @AccNumber

	INSERT INTO Transactions(TranDate, Amount, TranType, AccId) VALUES (
	SYSDATETIME(),
	@Amount,
	'Credited',
	@id1
	)

	SELECT @id2 = A.Id FROM Account A INNER JOIN Customer C on A.CustId = C.Id WHERE C.Username = @Username

	INSERT INTO Transactions(TranDate, Amount, TranType, AccId) VALUES (
	SYSDATETIME(),
	@Amount,
	'Debited',
	@id2
	)
END
GO


