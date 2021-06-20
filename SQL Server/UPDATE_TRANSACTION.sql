USE [BankSystem]
GO

/****** Object:  StoredProcedure [dbo].[UPDATE_TRANSACTION]    Script Date: 20-06-2021 20:33:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_TRANSACTION]
	-- Add the parameters for the stored procedure here
(
	@Username varchar(20),
	@AccNumber varchar(12),
	@IFSC varchar(10),
	@AccHolder varchar(100),
	@Amount float,
	@text nvarchar(500) OUTPUT 
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT COUNT(*) FROM Customer C Inner Join Account A on C.Id = A.CustId Inner Join Branch B on A.BranchId = B.Id 
	WHERE A.AccNumber = @AccNumber AND B.IFSC = @IFSC AND C.Name = @AccHolder

	IF (COUNT(*) > 0)
		UPDATE Account SET Balance = Balance + @Amount WHERE AccNumber = @AccNumber
	ELSE
		SET @text = 'Transaction Failed.'

	IF (COUNT(*) > 0)
		UPDATE Account SET Balance = Balance - @Amount FROM Account A INNER JOIN Customer C on A.CustId = C.Id WHERE C.Username = @Username
	 
	
END
GO


