-- Bảng lưu thông tin người dùng
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15),
    CreatedAt DATETIME
);

-- Bảng ví tiền của người dùng (1 user có 1 ví)
CREATE TABLE Wallet (
    UserId INT PRIMARY KEY,  -- Khóa chính, cũng là khóa ngoại đến Users(Id)
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0, -- Số dư ban đầu = 0
    CreatedAt DATETIME ,
    UpdatedAt DATETIME ,
    CONSTRAINT FK_Wallet_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Bảng lưu thông tin đơn hàng
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    OrderCode NVARCHAR(50) UNIQUE NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending', -- Pending, Paid, Failed, Refunded
    CreatedAt DATETIME ,
    CONSTRAINT FK_Orders_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Bảng lưu thông tin thanh toán qua VNPAY
CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    UserId INT NOT NULL, -- UserId này sẽ liên kết với Wallet(UserId)
    VnpayTransactionId NVARCHAR(50),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending',
    PaymentTime DATETIME ,
    BankCode NVARCHAR(20),
    ResponseCode NVARCHAR(10), -- Mã phản hồi từ VNPAY (00 là thành công)
    CONSTRAINT FK_Payments_Order FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Payments_User FOREIGN KEY (UserId) REFERENCES Wallet(UserId) ON DELETE CASCADE
);


-- Thêm người dùng
INSERT INTO Users (FullName, Email, Phone, Password) 
VALUES 
('Nguyễn Văn A', 'a', '0912345678', '123'),
('Trần Thị B', 'b', '0987654321', '123');

-- Tạo ví tiền cho người dùng
INSERT INTO Wallet (UserId, Balance) 
VALUES 
(1, 1000000), -- Ví của Nguyễn Văn A có 1 triệu VND
(2, 2000000); -- Ví của Trần Thị B có 2 triệu VND





-- Xóa bảng nếu đã tồn tại (Cần xóa bảng có liên kết trước)
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Wallet;
DROP TABLE IF EXISTS Users;