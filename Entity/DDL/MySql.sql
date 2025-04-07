--CREATE TABLE Rol (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    Active BOOLEAN NOT NULL
--);

--CREATE TABLE `User`(
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    LastName TEXT NOT NULL,
--    Email TEXT NOT NULL,
--    Password TEXT NOT NULL,
--    Identification INT NOT NULL,
--    Phone BIGINT NOT NULL,
--    Address TEXT NOT NULL,
--    IsDeleted BOOLEAN NOT NULL
--);

--CREATE TABLE Permission (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    Description TEXT NOT NULL,
--    IsDeleted BOOLEAN NOT NULL
--);

--CREATE TABLE Form (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    Description TEXT NOT NULL,
--    IsDeleted BOOLEAN NOT NULL
--);

--CREATE TABLE Module (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    Description TEXT NOT NULL,
--    Code TEXT NOT NULL,
--    IsDeleted BOOLEAN NOT NULL
--);

--CREATE TABLE ModuleForm (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    FormId INT NOT NULL,
--    ModuleId INT NOT NULL,
--    FOREIGN KEY (FormId) REFERENCES Form(Id),
--    FOREIGN KEY (ModuleId) REFERENCES Module(Id)
--);

--CREATE TABLE Log (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    TableName TEXT NOT NULL,
--    PropietyName TEXT NOT NULL,
--    Action TEXT NOT NULL,
--    TimeStamp DATETIME NOT NULL,
--    UserId INT NOT NULL,
--    OldValue TEXT NOT NULL,
--    NewValue TEXT NOT NULL
--);

--CREATE TABLE Certificate (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    TypeCertificate TEXT NOT NULL,
--    IssueDate DATETIME NOT NULL,
--    ExpirationDate DATETIME NOT NULL,
--    UserId INT NOT NULL,
--    FOREIGN KEY (UserId) REFERENCES User(Id)
--);

--CREATE TABLE Paremeter (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    Value DECIMAL(15,2) NOT NULL,
--    Description TEXT NOT NULL
--);

--CREATE TABLE RolFormPermission (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    RolId INT NOT NULL,
--    PermissionId INT NOT NULL,
--    FormId INT NOT NULL,
--    FOREIGN KEY (RolId) REFERENCES Rol(Id),
--    FOREIGN KEY (PermissionId) REFERENCES Permission(Id),
--    FOREIGN KEY (FormId) REFERENCES Form(Id)
--);

--CREATE TABLE TypeEstablishment (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Name TEXT NOT NULL,
--    UVT DECIMAL(13,2) NOT NULL,
--    PaymentTime TEXT NOT NULL
--);

--CREATE TABLE Establishment (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Nit INT NOT NULL,
--    State BOOLEAN NOT NULL,
--    Name TEXT NOT NULL,
--    Address TEXT NOT NULL,
--    IsDeleted BOOLEAN NOT NULL,
--    TypeEstablishmentId INT NOT NULL,
--    FOREIGN KEY (TypeEstablishmentId) REFERENCES TypeEstablishment(Id)
--);

--CREATE TABLE RolUser (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    RolId INT NOT NULL,
--    UserId INT NOT NULL,
--    FOREIGN KEY (RolId) REFERENCES Rol(Id),
--    FOREIGN KEY (UserId) REFERENCES User(Id)
--);

--CREATE TABLE Debt (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    DebtDate DATETIME NOT NULL,
--    GrossValue DECIMAL(13,2) NOT NULL,
--    IvaValue DECIMAL(13,2) NOT NULL,
--    DebtTotal DECIMAL(13,2) NOT NULL,
--    IsDeleted BOOLEAN NOT NULL,
--    UserId INT NOT NULL,
--    EstablishmentId INT NOT NULL,
--    FOREIGN KEY (UserId) REFERENCES User(Id),
--    FOREIGN KEY (EstablishmentId) REFERENCES Establishment(Id)
--);

--CREATE TABLE Bill (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Barcode BIGINT NOT NULL,
--    IssueDate DATETIME NOT NULL,
--    ExpirationDate DATETIME NOT NULL,
--    Amount DECIMAL(13,2) NOT NULL,
--    State BOOLEAN NOT NULL
--);

--CREATE TABLE DebtBill (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    BillId INT NOT NULL,
--    DebtId INT NOT NULL,
--    FOREIGN KEY (BillId) REFERENCES Bill(Id),
--    FOREIGN KEY (DebtId) REFERENCES Debt(Id)
--);

--CREATE TABLE Rent (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    ContractDate DATETIME NOT NULL,
--    ExpirationDate DATETIME NOT NULL,
--    SocialReason TEXT NOT NULL,
--    State BOOLEAN NOT NULL,
--    UserId INT NOT NULL,
--    EstablishmentId INT NOT NULL,
--    FOREIGN KEY (UserId) REFERENCES User(Id),
--    FOREIGN KEY (EstablishmentId) REFERENCES Establishment(Id)
--);

--CREATE TABLE Payment (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    PaymentDate DATETIME NOT NULL,
--    AmountPaid DECIMAL(13,2) NOT NULL,
--    State BOOLEAN NOT NULL,
--    DebtId INT NOT NULL,
--    FOREIGN KEY (DebtId) REFERENCES Debt(Id)
--);

--CREATE TABLE Statement (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Type TEXT NOT NULL,
--    Title TEXT NOT NULL,
--    Description TEXT NOT NULL,
--    GenerationDate DATETIME NOT NULL,
--    PeriodStart DATETIME NOT NULL,
--    PeriodEnd DATETIME NOT NULL,
--    TotalAmount DECIMAL(13,2) NOT NULL,
--    IsDeleted BOOLEAN NOT NULL
--);

--CREATE TABLE Calculation (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Type TEXT NOT NULL,
--    Formula TEXT NOT NULL,
--    Description TEXT NOT NULL
--);

--CREATE TABLE StatementDetail (
--    Id INT AUTO_INCREMENT PRIMARY KEY,
--    Catogory TEXT NOT NULL,
--    ItemName TEXT NOT NULL,
--    Amount DECIMAL(13,2) NOT NULL,
--    Period TEXT NOT NULL,
--    PreviousPeriodAmount TEXT NOT NULL,
--    TargetAmount DECIMAL(13,2) NOT NULL,
--    Variance DECIMAL(13,2) NOT NULL,
--    VariancePercentage DECIMAL(13,2) NOT NULL,
--    CreatedAt DATETIME NOT NULL,
--    EstablishmentId INT NOT NULL,
--    PaymentId INT NOT NULL,
--    StatementId INT NOT NULL,
--    FOREIGN KEY (EstablishmentId) REFERENCES Establishment(Id),
--    FOREIGN KEY (PaymentId) REFERENCES Payment(Id),
--    FOREIGN KEY (StatementId) REFERENCES Statement(Id)
--);
