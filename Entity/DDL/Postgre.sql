CREATE TABLE Rol (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Active BOOLEAN NOT NULL
);

CREATE TABLE "User" (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL,
    Password TEXT NOT NULL,
    Identification INT NOT NULL,
    Phone BIGINT NOT NULL,
    Address TEXT NOT NULL,
    IsDeleted BOOLEAN NOT NULL
);

CREATE TABLE Permission (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Description TEXT NOT NULL,
    IsDeleted BOOLEAN NOT NULL
);

CREATE TABLE Form (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Description TEXT NOT NULL,
    IsDeleted BOOLEAN NOT NULL
);

CREATE TABLE Module (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Description TEXT NOT NULL,
    Code TEXT NOT NULL,
    IsDeleted BOOLEAN NOT NULL
);

CREATE TABLE ModuleForm (
    Id SERIAL PRIMARY KEY,
    FormId INT NOT NULL REFERENCES Form(Id),
    ModuleId INT NOT NULL REFERENCES Module(Id)
);

CREATE TABLE Log (
    Id SERIAL PRIMARY KEY,
    TableName TEXT NOT NULL,
    PropietyName TEXT NOT NULL,
    Action TEXT NOT NULL,
    TimeStamp TIMESTAMP NOT NULL,
    UserId INT NOT NULL,
    OldValue TEXT NOT NULL,
    NewValue TEXT NOT NULL
);

CREATE TABLE Certificate (
    Id SERIAL PRIMARY KEY,
    TypeCertificate TEXT NOT NULL,
    IssueDate TIMESTAMP NOT NULL,
    ExpirationDate TIMESTAMP NOT NULL,
    UserId INT NOT NULL REFERENCES "User"(Id)
);

CREATE TABLE Paremeter (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Value NUMERIC(15,2) NOT NULL,
    Description TEXT NOT NULL
);

CREATE TABLE RolFormPermission (
    Id SERIAL PRIMARY KEY,
    RolId INT NOT NULL REFERENCES Rol(Id),
    PermissionId INT NOT NULL REFERENCES Permission(Id),
    FormId INT NOT NULL REFERENCES Form(Id)
);

CREATE TABLE TypeEstablishment (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    UVT NUMERIC(13,2) NOT NULL,
    PaymentTime TEXT NOT NULL
);

CREATE TABLE Establishment (
    Id SERIAL PRIMARY KEY,
    Nit INT NOT NULL,
    State BOOLEAN NOT NULL,
    Name TEXT NOT NULL,
    Address TEXT NOT NULL,
    IsDeleted BOOLEAN NOT NULL,
    TypeEstablishmentId INT NOT NULL REFERENCES TypeEstablishment(Id)
);

CREATE TABLE RolUser (
    Id SERIAL PRIMARY KEY,
    RolId INT NOT NULL REFERENCES Rol(Id),
    UserId INT NOT NULL REFERENCES "User"(Id)
);

CREATE TABLE Debt (
    Id SERIAL PRIMARY KEY,
    DebtDate TIMESTAMP NOT NULL,
    GrossValue NUMERIC(13,2) NOT NULL,
    IvaValue NUMERIC(13,2) NOT NULL,
    DebtTotal NUMERIC(13,2) NOT NULL,
    IsDeleted BOOLEAN NOT NULL,
    UserId INT NOT NULL REFERENCES "User"(Id),
    EstablishmentId INT NOT NULL REFERENCES Establishment(Id)
);

CREATE TABLE Bill (
    Id SERIAL PRIMARY KEY,
    Barcode BIGINT NOT NULL,
    IssueDate TIMESTAMP NOT NULL,
    ExpirationDate TIMESTAMP NOT NULL,
    Amount NUMERIC(13,2) NOT NULL,
    State BOOLEAN NOT NULL
);

CREATE TABLE DebtBill (
    Id SERIAL PRIMARY KEY,
    BillId INT NOT NULL REFERENCES Bill(Id),
    DebtId INT NOT NULL REFERENCES Debt(Id)
);

CREATE TABLE Rent (
    Id SERIAL PRIMARY KEY,
    ContractDate TIMESTAMP NOT NULL,
    ExpirationDate TIMESTAMP NOT NULL,
    SocialReason TEXT NOT NULL,
    State BOOLEAN NOT NULL,
    UserId INT NOT NULL REFERENCES "User"(Id),
    EstablishmentId INT NOT NULL REFERENCES Establishment(Id)
);

CREATE TABLE Payment (
    Id SERIAL PRIMARY KEY,
    PaymentDate TIMESTAMP NOT NULL,
    AmountPaid NUMERIC(13,2) NOT NULL,
    State BOOLEAN NOT NULL,
    DebtId INT NOT NULL REFERENCES Debt(Id)
);

CREATE TABLE Statement (
    Id SERIAL PRIMARY KEY,
    Type TEXT NOT NULL,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    GenerationDate TIMESTAMP NOT NULL,
    PeriodStart TIMESTAMP NOT NULL,
    PeriodEnd TIMESTAMP NOT NULL,
    TotalAmount NUMERIC(13,2) NOT NULL,
    IsDeleted BOOLEAN NOT NULL
);

CREATE TABLE Calculation (
    Id SERIAL PRIMARY KEY,
    Type TEXT NOT NULL,
    Formula TEXT NOT NULL,
    Description TEXT NOT NULL
);

CREATE TABLE StatementDetail (
    Id SERIAL PRIMARY KEY,
    Catogory TEXT NOT NULL,
    ItemName TEXT NOT NULL,
    Amount NUMERIC(13,2) NOT NULL,
    Period TEXT NOT NULL,
    PreviousPeriodAmount TEXT NOT NULL,
    TargetAmount NUMERIC(13,2) NOT NULL,
    Variance NUMERIC(13,2) NOT NULL,
    VariancePercentage NUMERIC(13,2) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    EstablishmentId INT NOT NULL REFERENCES Establishment(Id),
    PaymentId INT NOT NULL REFERENCES Payment(Id),
    StatementId INT NOT NULL REFERENCES Statement(Id)
);
