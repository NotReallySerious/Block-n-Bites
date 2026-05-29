-- Users
CREATE TABLE Users (
    UserID       INT PRIMARY KEY IDENTITY(1,1),
    Name         VARCHAR(255) NOT NULL,
    Email        VARCHAR(255) NOT NULL UNIQUE,
    Password     VARCHAR(255) NOT NULL,
    Role         VARCHAR(20)  NOT NULL CHECK (Role IN ('Admin', 'Manager', 'Chef', 'Customer')),
    BBCBalance DECIMAL(10,2) NOT NULL DEFAULT 0.00
);

-- BBCTransactions
CREATE TABLE BBCTransactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    UserID        INT NOT NULL,
    Amount        DECIMAL(10,2) NOT NULL,
    Type          VARCHAR(10) NOT NULL CHECK (Type IN ('TopUp', 'Deduction', 'Refund')),
    Date          DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);