CREATE PROCEDURE [dbo].[Usp_Filtered_Data]
@SelectQuery nvarchar(500),
@WhereQuery nvarchar(500),
@SortQuery nvarchar(500),
@LimitQuery nvarchar(500),
@Totalrecords BIGINT OUTPUT
AS
BEGIN
DECLARE @SQL varchar(MAX)
DECLARE @params NVARCHAR(255) = '@Count BIGINT OUTPUT'
DECLARE @totalRecord NVARCHAR(4000) = 'SELECT @Count = COUNT(*) FROM OlympicWinners ' + @WhereQuery;
SET @SQL = @SelectQuery + @WhereQuery + @SortQuery + ' ' + @LimitQuery;
EXEC(@SQL)
EXEC sp_executeSQL @totalRecord, @params, @Count = @Totalrecords OUTPUT;
return @Totalrecords
END