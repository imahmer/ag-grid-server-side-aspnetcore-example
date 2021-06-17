create PROCEDURE [dbo].[usp_Generate_Create_Table_Schema] 
    @Tabla AS VARCHAR(100)
AS 
BEGIN 
    DECLARE @Script AS VARCHAR(MAX) ='CREATE TABLE [dbo].[' + @Tabla + '](' + CHAR(13) 
    DECLARE @Columnas As TABLE(indice INT, Columna VARCHAR(1000))
    INSERT INTO @Columnas 
    SELECT
        ROW_NUMBER()OVER(ORDER BY C.column_id),
        '   [' + C.name + '] [' + TY.name + ']' + 
        CASE WHEN 
            TY.name='nvarchar' OR 
            TY.name='nchar' OR 
            TY.name='char' OR 
            TY.name='varbinary' OR 
            TY.name='varchar' OR 
            TY.name='text' THEN 
                '(' + CASE WHEN C.max_length>0 THEN CAST(C.max_length AS VARCHAR(10)) ELSE 'MAX' END + ')' ELSE '' 
        END + 
        CASE WHEN C.is_identity=1 THEN ' IDENTITY(1,1)' ELSE '' END +
        ' ' + CASE WHEN C.is_nullable=1 THEN 'NULL' ELSE 'NOT NULL' END + ',' 
    FROM SYS.COLUMNS AS C 
        INNER JOIN SYS.TYPES AS TY ON C.system_type_id=TY.system_type_id
        INNER JOIN SYS.TABLES AS T ON C.object_id=T.object_id
    WHERE T.name=@Tabla 

    DECLARE @i AS INT
    SELECT @i=MIN(indice) FROM @Columnas
    WHILE @i IS NOT NULL
    BEGIN
        SELECT @Script+=Columna+CHAR(13) FROM @Columnas WHERE indice=@i
        SELECT @i=MIN(indice) FROM @Columnas WHERE indice>@i
    END

    SET @Script=SUBSTRING(@Script,0,LEN(@Script)-1) + CHAR(13) + ')'
    PRINT @Script
END