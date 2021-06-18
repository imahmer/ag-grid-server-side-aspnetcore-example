CREATE PROCEDURE [dbo].[Usp_Filtered_Data]
@SelectQuery nvarchar(500),
@WhereQuery nvarchar(500),
@GroupQuery nvarchar(500),
@SortQuery nvarchar(500),
@LimitQuery nvarchar(500),
@TotalRecords BIGINT OUTPUT
AS
BEGIN
DECLARE @SQL varchar(MAX)
DECLARE @params NVARCHAR(255) = '@Count BIGINT OUTPUT'
DECLARE @TotalCount NVARCHAR(500) = 'SELECT @Count = COUNT(1) FROM OlympicWinners '
IF ISNULL(@WhereQuery, '') != ''
	SET @TotalCount = @TotalCount + @WhereQuery + ' ' + @GroupQuery
SET @SQL = @SelectQuery + ' ' + ISNULL(@WhereQuery, '') + ' ' + @GroupQuery + ' ' + @SortQuery + ' ' + @LimitQuery;
EXEC(@SQL)
EXEC sp_executeSQL @TotalCount, @params, @Count = @TotalRecords OUTPUT;
return @Totalrecords
END