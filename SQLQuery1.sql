-- Bảng lưu thông tin người dùng
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15),
    CreatedAt DATETIME
);

-- Bảng lưu thông tin đơn hàng
CREATE TABLE Orders (
    OrderCode NVARCHAR(200) PRIMARY KEY,
    UserId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending', -- Pending, Paid, Failed, Refunded
    CreatedAt DATETIME ,
    CONSTRAINT FK_Orders_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Bảng lưu thông tin thanh toán qua VNPAY
CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderCode NVARCHAR(200),
    UserId INT NOT NULL,
	PaymentAttempt INT DEFAULT 1,
	PaymentCode NVARCHAR(200) NULL,
    VnpayTransactionId NVARCHAR(50),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending',
    PaymentTime DATETIME ,
    BankCode NVARCHAR(20),
    ResponseCode NVARCHAR(10), -- Mã phản hồi từ VNPAY (00 là thành công)
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderCode) REFERENCES Orders(OrderCode),
    CONSTRAINT FK_Payments_Users FOREIGN KEY (UserId) REFERENCES Users(Id) 
);


-- Thêm người dùng
INSERT INTO Users (FullName, Email, Phone, Password) 
VALUES 
('Nguyễn Văn A', 'a', '0912345678', '123'),
('Nguyễn Văn B', 'b', '0912345678', '123'),
('Nguyễn Văn C', 'c', '0912345678', '123'),
('Trần Thị D', 'd', '0987654321', '123'),
('Nguyễn Văn E', 'e', '0912345678', '123');




-- Xóa bảng nếu đã tồn tại (Cần xóa bảng có liên kết trước)
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Users;