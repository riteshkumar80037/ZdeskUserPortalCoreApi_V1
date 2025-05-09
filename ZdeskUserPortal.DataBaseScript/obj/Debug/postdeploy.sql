/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
ALTER TABLE zdesk_m_business_unit_tbl DROP CONSTRAINT DF_zdesk_m_business_unit_tbl_is_active;
GO
ALTER TABLE zdesk_m_business_unit_tbl ALTER COLUMN is_active bit;
GO

CREATE TABLE [RefereshToken](
  [TokenId] INT PRIMARY KEY IDENTITY(1,1),
  [Token] VARCHAR(500) NOT NULL,
  [UserId] INT NOT NULL,
  [EmailId] VARCHAR(500) NOT NULL,
  [Active] BIT NOT NULL,
  [ExpiryDate] DATETIME NOT NULL
)
GO
