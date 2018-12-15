USE [Northwind]
GO

/****** Object:  StoredProcedure [dbo].[Customer_Tbl_CSV_Insert]    Script Date: 12/15/2018 5:08:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Hal Luchka
-- Create date: 12/15/2018
-- Description:	stored procedure to bulk insert data from a csv file into the Northwind Customer Table
--  to execute run exec Customer_Tbl_CSV_Insert
-- =============================================
CREATE PROCEDURE [dbo].[Customer_Tbl_CSV_Insert]
	AS
BEGIN

	SET NOCOUNT ON;

    bulk insert Northwind.dbo.customers
	from 'c:\test\SP_Test_Data.csv'
		with(FIRSTROW = 2,
		FIELDTERMINATOR = ',',
		ROWTERMINATOR = '\n'
		)
	
END
GO

