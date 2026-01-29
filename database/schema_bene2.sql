/****************************************************************************************
  PROJECT  : Beneficiaries
  DATABASE : bene2
  ENGINE   : SQL Server
  NOTE     : Single script. All insertions are performed through Stored Procedures.
****************************************************************************************/


/****************************************************************************************
  1. DATABASE CREATION
****************************************************************************************/
IF DB_ID('bene2') IS NULL
BEGIN
    CREATE DATABASE bene2;
END
GO

USE bene2;
GO


/****************************************************************************************
  2. TABLE CREATION
****************************************************************************************/

IF OBJECT_ID('dbo.Beneficiary', 'U') IS NOT NULL DROP TABLE dbo.Beneficiary;
IF OBJECT_ID('dbo.IdentityDocument', 'U') IS NOT NULL DROP TABLE dbo.IdentityDocument;
GO

CREATE TABLE dbo.IdentityDocument
(
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_IdentityDocument PRIMARY KEY,
    Name        NVARCHAR(50)      NOT NULL,
    Abbreviation NVARCHAR(10)     NOT NULL,
    Country     NVARCHAR(50)      NOT NULL,
    Length      INT               NOT NULL,
    NumericOnly BIT               NOT NULL,
    IsActive    BIT               NOT NULL
);
GO

CREATE TABLE dbo.Beneficiary
(
    Id                 INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Beneficiary PRIMARY KEY,
    FirstNames         NVARCHAR(100)     NOT NULL,
    LastNames          NVARCHAR(100)     NOT NULL,
    IdentityDocumentId INT               NOT NULL,
    DocumentNumber     NVARCHAR(20)      NOT NULL,
    BirthDate          DATE              NOT NULL,
    Gender             CHAR(1)           NOT NULL,

    CONSTRAINT FK_Beneficiary_IdentityDocument
        FOREIGN KEY (IdentityDocumentId) REFERENCES dbo.IdentityDocument(Id),

    CONSTRAINT CK_Beneficiary_Gender
        CHECK (Gender IN ('M','F'))
);
GO

CREATE INDEX IX_Beneficiary_IdentityDocumentId ON dbo.Beneficiary(IdentityDocumentId);
CREATE INDEX IX_Beneficiary_DocumentNumber ON dbo.Beneficiary(DocumentNumber);
GO


/****************************************************************************************
  3. STORED PROCEDURES - IDENTITY DOCUMENT
****************************************************************************************/

-- CREATE
CREATE OR ALTER PROCEDURE dbo.IdentityDocument_Create
    @Name         NVARCHAR(50),
    @Abbreviation NVARCHAR(10),
    @Country      NVARCHAR(50),
    @Length       INT,
    @NumericOnly  BIT,
    @IsActive     BIT,
    @NewId        INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.IdentityDocument
    (Name, Abbreviation, Country, Length, NumericOnly, IsActive)
    VALUES
    (@Name, @Abbreviation, @Country, @Length, @NumericOnly, @IsActive);

    SET @NewId = CAST(SCOPE_IDENTITY() AS INT);
END
GO


-- GET BY ID
CREATE OR ALTER PROCEDURE dbo.IdentityDocument_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Name,
        Abbreviation,
        Country,
        Length,
        NumericOnly,
        IsActive
    FROM dbo.IdentityDocument
    WHERE Id = @Id;
END
GO


-- GET ACTIVE
CREATE OR ALTER PROCEDURE dbo.IdentityDocument_GetActive
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Name,
        Abbreviation,
        Country,
        Length,
        NumericOnly,
        IsActive
    FROM dbo.IdentityDocument
    WHERE IsActive = 1
    ORDER BY Country, Name;
END
GO


/****************************************************************************************
  4. STORED PROCEDURE - SEED IDENTITY DOCUMENT (MANDATORY)
****************************************************************************************/

CREATE OR ALTER PROCEDURE dbo.IdentityDocument_Seed
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Beneficiary;
    DELETE FROM dbo.IdentityDocument;

    DECLARE @Id INT;

    EXEC dbo.IdentityDocument_Create
        @Name = N'DNI',
        @Abbreviation = N'DNI',
        @Country = N'Peru',
        @Length = 8,
        @NumericOnly = 1,
        @IsActive = 1,
        @NewId = @Id OUTPUT;

    EXEC dbo.IdentityDocument_Create
        @Name = N'Passport',
        @Abbreviation = N'PAS',
        @Country = N'Peru',
        @Length = 9,
        @NumericOnly = 0,
        @IsActive = 1,
        @NewId = @Id OUTPUT;

    EXEC dbo.IdentityDocument_Create
        @Name = N'Foreigner Card',
        @Abbreviation = N'FC',
        @Country = N'Peru',
        @Length = 9,
        @NumericOnly = 1,
        @IsActive = 1,
        @NewId = @Id OUTPUT;

    EXEC dbo.IdentityDocument_Create
        @Name = N'DNI',
        @Abbreviation = N'DNI',
        @Country = N'Chile',
        @Length = 9,
        @NumericOnly = 1,
        @IsActive = 1,
        @NewId = @Id OUTPUT;

    EXEC dbo.IdentityDocument_Create
        @Name = N'Passport',
        @Abbreviation = N'PAS',
        @Country = N'Chile',
        @Length = 9,
        @NumericOnly = 0,
        @IsActive = 1,
        @NewId = @Id OUTPUT;

    EXEC dbo.IdentityDocument_Create
        @Name = N'Inactive Document',
        @Abbreviation = N'INA',
        @Country = N'Peru',
        @Length = 6,
        @NumericOnly = 1,
        @IsActive = 0,
        @NewId = @Id OUTPUT;
END
GO


/****************************************************************************************
  5. STORED PROCEDURES - BENEFICIARY
****************************************************************************************/

-- CREATE
CREATE OR ALTER PROCEDURE dbo.Beneficiary_Create
    @FirstNames        NVARCHAR(100),
    @LastNames         NVARCHAR(100),
    @IdentityDocumentId INT,
    @DocumentNumber    NVARCHAR(20),
    @BirthDate         DATE,
    @Gender            CHAR(1),
    @NewId             INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Beneficiary
    (FirstNames, LastNames, IdentityDocumentId, DocumentNumber, BirthDate, Gender)
    VALUES
    (@FirstNames, @LastNames, @IdentityDocumentId, @DocumentNumber, @BirthDate, @Gender);

    SET @NewId = CAST(SCOPE_IDENTITY() AS INT);
END
GO


-- UPDATE
CREATE OR ALTER PROCEDURE dbo.Beneficiary_Update
    @Id                INT,
    @FirstNames        NVARCHAR(100),
    @LastNames         NVARCHAR(100),
    @IdentityDocumentId INT,
    @DocumentNumber    NVARCHAR(20),
    @BirthDate         DATE,
    @Gender            CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Beneficiary
    SET
        FirstNames = @FirstNames,
        LastNames = @LastNames,
        IdentityDocumentId = @IdentityDocumentId,
        DocumentNumber = @DocumentNumber,
        BirthDate = @BirthDate,
        Gender = @Gender
    WHERE Id = @Id;
END
GO


-- DELETE
CREATE OR ALTER PROCEDURE dbo.Beneficiary_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Beneficiary
    WHERE Id = @Id;
END
GO


-- GET BY ID
CREATE OR ALTER PROCEDURE dbo.Beneficiary_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        b.Id,
        b.FirstNames,
        b.LastNames,
        b.IdentityDocumentId,
        b.DocumentNumber,
        b.BirthDate,
        b.Gender,

        d.Id            AS Document_Id,
        d.Name          AS Document_Name,
        d.Abbreviation  AS Document_Abbreviation,
        d.Country       AS Document_Country,
        d.Length        AS Document_Length,
        d.NumericOnly   AS Document_NumericOnly,
        d.IsActive      AS Document_IsActive
    FROM dbo.Beneficiary b
    INNER JOIN dbo.IdentityDocument d ON d.Id = b.IdentityDocumentId
    WHERE b.Id = @Id;
END
GO


-- GET ALL
CREATE OR ALTER PROCEDURE dbo.Beneficiary_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        b.Id,
        b.FirstNames,
        b.LastNames,
        b.IdentityDocumentId,
        b.DocumentNumber,
        b.BirthDate,
        b.Gender,

        d.Id            AS Document_Id,
        d.Name          AS Document_Name,
        d.Abbreviation  AS Document_Abbreviation,
        d.Country       AS Document_Country,
        d.Length        AS Document_Length,
        d.NumericOnly   AS Document_NumericOnly,
        d.IsActive      AS Document_IsActive
    FROM dbo.Beneficiary b
    INNER JOIN dbo.IdentityDocument d ON d.Id = b.IdentityDocumentId
    ORDER BY b.Id DESC;
END
GO


/****************************************************************************************
  6. EXECUTION OF SEED (AUTOMATIC)
****************************************************************************************/

EXEC dbo.IdentityDocument_Seed;
GO



USE bene2;

GO

CREATE OR ALTER PROCEDURE dbo.Beneficiary_ListPaged
    @Name               NVARCHAR(200) = NULL,
    @DocumentNumber     NVARCHAR(20)  = NULL,
    @IdentityDocumentId INT           = NULL,
    @Page               INT           = 1,
    @PageSize           INT           = 20,
    @TotalCount         INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Page < 1 SET @Page = 1;
    IF @PageSize < 1 SET @PageSize = 20;

    SELECT @TotalCount = COUNT(1)
    FROM dbo.Beneficiary b
    INNER JOIN dbo.IdentityDocument d ON d.Id = b.IdentityDocumentId
    WHERE
        (@IdentityDocumentId IS NULL OR b.IdentityDocumentId = @IdentityDocumentId)
        AND (
            @Name IS NULL
            OR b.FirstNames LIKE N'%' + @Name + N'%'
            OR b.LastNames  LIKE N'%' + @Name + N'%'
        )
        AND (
            @DocumentNumber IS NULL
            OR b.DocumentNumber LIKE N'%' + @DocumentNumber + N'%'
        );

    ;WITH Q AS
    (
        SELECT
            b.Id,
            b.FirstNames,
            b.LastNames,
            b.IdentityDocumentId,
            b.DocumentNumber,
            b.BirthDate,
            b.Gender,

            d.Id            AS Document_Id,
            d.Name          AS Document_Name,
            d.Abbreviation  AS Document_Abbreviation,
            d.Country       AS Document_Country,
            d.Length        AS Document_Length,
            d.NumericOnly   AS Document_NumericOnly,
            d.IsActive      AS Document_IsActive
        FROM dbo.Beneficiary b
        INNER JOIN dbo.IdentityDocument d ON d.Id = b.IdentityDocumentId
        WHERE
            (@IdentityDocumentId IS NULL OR b.IdentityDocumentId = @IdentityDocumentId)
            AND (
                @Name IS NULL
                OR b.FirstNames LIKE N'%' + @Name + N'%'
                OR b.LastNames  LIKE N'%' + @Name + N'%'
            )
            AND (
                @DocumentNumber IS NULL
                OR b.DocumentNumber LIKE N'%' + @DocumentNumber + N'%'
            )
    )
    SELECT *
    FROM Q
    ORDER BY Id DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
