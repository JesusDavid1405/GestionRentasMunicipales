CREATE TABLE [Rol] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [Active] BIT NOT NULL
);

CREATE TABLE [User] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [LastName] NVARCHAR(MAX) NOT NULL,
    [Email] NVARCHAR(MAX) NOT NULL,
    [Password] NVARCHAR(MAX) NOT NULL,   
    [Identification] INT NOT NULL,
    [Phone] BIGINT NOT NULL,
    [Address] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL
);

CREATE TABLE [Permission] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL
);

CREATE TABLE [Form] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL
);

CREATE TABLE [Module] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Code] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL
);

CREATE TABLE [ModuleForm] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [FormId] INT NOT NULL,
    [ModuleId] INT NOT NULL,
    CONSTRAINT [FK_ModuleForm_Form] FOREIGN KEY ([FormId]) REFERENCES [Form]([Id]),
    CONSTRAINT [FK_ModuleForm_Module] FOREIGN KEY ([ModuleId]) REFERENCES [Module]([Id])
);

CREATE TABLE [Log] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [TableName] NVARCHAR(MAX) NOT NULL,
    [PropietyName] NVARCHAR(MAX) NOT NULL,
    [Action] NVARCHAR(MAX) NOT NULL,
    [TimeStamp] DATETIME NOT NULL,
    [UserId] INT NOT NULL,
    [OldValue] NVARCHAR(MAX) NOT NULL,
    [NewValue] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [Certificate] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [TypeCertificate] NVARCHAR(MAX) NOT NULL,
    [IssueDate] DATETIME NOT NULL,
    [ExpirationDate] DATETIME NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [FK_Certificate_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
);

CREATE TABLE [Paremeter] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [Value] DECIMAL(15,2) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [RolFormPermission] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [RolId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    [FormId] INT NOT NULL,
    CONSTRAINT [FK_RolFormPermission_Rol] FOREIGN KEY ([RolId]) REFERENCES [Rol]([Id]),
    CONSTRAINT [FK_RolFormPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]),
    CONSTRAINT [FK_RolFormPermission_Form] FOREIGN KEY ([FormId]) REFERENCES [Form]([Id])
);

CREATE TABLE [TypeEstablishment] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(MAX) NOT NULL,
    [UVT] DECIMAL(13,2) NOT NULL,
    [PaymentTime] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [Establishment] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Nit] INT NOT NULL,
    [State] BIT NOT NULL,
    [Name] NVARCHAR(MAX) NOT NULL,
    [Address] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    [TypeEstablishmentId] INT NOT NULL,
    CONSTRAINT [FK_Establishment_TypeEstablishment] FOREIGN KEY ([TypeEstablishmentId]) REFERENCES [TypeEstablishment]([Id])
);

CREATE TABLE [RolUser] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [RolId] INT NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [FK_RolUser_Rol] FOREIGN KEY ([RolId]) REFERENCES [Rol]([Id]),
    CONSTRAINT [FK_RolUser_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
);

CREATE TABLE [Debt] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [DebtDate] DATETIME NOT NULL,
    [GrossValue] DECIMAL(13,2) NOT NULL,
    [IvaValue] DECIMAL(13,2) NOT NULL,
    [DebtTotal] DECIMAL(13,2) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    [UserId] INT NOT NULL,
    [EstablishmentId] INT NOT NULL,
    CONSTRAINT [FK_Debt_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Debt_Establishment] FOREIGN KEY ([EstablishmentId]) REFERENCES [Establishment]([Id])
);

CREATE TABLE [Bill] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Barcode] BIGINT NOT NULL,
    [IssueDate] DATETIME NOT NULL,
    [ExpirationDate] DATETIME NOT NULL,
    [Amount] DECIMAL(13,2) NOT NULL,
    [State] BIT NOT NULL
);

CREATE TABLE [DebtBill] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [BillId] INT NOT NULL,
    [DebtId] INT NOT NULL,
    CONSTRAINT [FK_DebtBill_Bill] FOREIGN KEY ([BillId]) REFERENCES [Bill]([Id]),
    CONSTRAINT [FK_DebtBill_Debt] FOREIGN KEY ([DebtId]) REFERENCES [Debt]([Id])
);

CREATE TABLE [Rent] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [ContractDate] DATETIME NOT NULL,
    [ExpirationDate] DATETIME NOT NULL,
    [SocialReason] NVARCHAR(MAX) NOT NULL,
    [State] BIT NOT NULL,
    [UserId] INT NOT NULL,
    [EstablishmentId] INT NOT NULL,
    CONSTRAINT [FK_Rent_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Rent_Establishment] FOREIGN KEY ([EstablishmentId]) REFERENCES [Establishment]([Id])
);

CREATE TABLE [Payment] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [PaymentDate] DATETIME NOT NULL,
    [AmountPaid] DECIMAL(13,2) NOT NULL,
    [State] BIT NOT NULL,
    [DebtId] INT NOT NULL,
    CONSTRAINT [FK_Payment_Debt] FOREIGN KEY ([DebtId]) REFERENCES [Debt]([Id])
);

CREATE TABLE [Statement] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Type] NVARCHAR(MAX) NOT NULL,
    [Title] NVARCHAR(MAX) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [GenerationDate] DATETIME NOT NULL,
    [PeriodStart] DATETIME NOT NULL,
    [PeriodEnd] DATETIME NOT NULL,
    [TotalAmount] DECIMAL(13,2) NOT NULL,
    [IsDeleted] BIT NOT NULL
);

CREATE TABLE [Calculation] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Type] NVARCHAR(MAX) NOT NULL,
    [Formula] NVARCHAR(MAX) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [StatementDetail] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Catogory] NVARCHAR(MAX) NOT NULL,
    [ItemName] NVARCHAR(MAX) NOT NULL,
    [Amount] DECIMAL(13,2) NOT NULL,
    [Period] NVARCHAR(MAX) NOT NULL,
    [PreviousPeriodAmount] NVARCHAR(MAX) NOT NULL,
    [TargetAmount] DECIMAL(13,2) NOT NULL,
    [Variance] DECIMAL(13,2) NOT NULL,
    [VariancePercentage] DECIMAL(13,2) NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [EstablishmentId] INT NOT NULL,
    [PaymentId] INT NOT NULL,
    [StatementId] INT NOT NULL,
    CONSTRAINT [FK_StatementDetail_Establishment] FOREIGN KEY ([EstablishmentId]) REFERENCES [Establishment]([Id]),
    CONSTRAINT [FK_StatementDetail_Payment] FOREIGN KEY ([PaymentId]) REFERENCES [Payment]([Id]),
    CONSTRAINT [FK_StatementDetail_Statement] FOREIGN KEY ([StatementId]) REFERENCES [Statement]([Id])
);   