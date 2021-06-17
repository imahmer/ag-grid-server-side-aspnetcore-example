CREATE PROCEDURE [dbo].[Usp_OlympicWinnerGetGridFilterList]
@SearchKeyword NVARCHAR(256), 
@OlympicWinnerId BIGINT,
@Startindex INT, 
@Pagesize INT, 
@Totalrecords BIGINT OUTPUT
AS
BEGIN
	SELECT  RowNum, Id, Athlete, Age, Country, [Year], [Date], Sport, Gold, Silver, Bronze, Total
	FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY OlympicWinners.Id ) AS RowNum, *
			  FROM      OlympicWinners
			) AS RowConstrainedResult
	WHERE   RowNum > (@Startindex-1)
		AND RowNum <= @Pagesize
	ORDER BY RowNum

	SELECT @Totalrecords = COUNT(OlympicWinners.Id) FROM OlympicWinners
END