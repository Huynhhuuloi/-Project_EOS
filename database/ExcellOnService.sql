use master 
go
create database Excell_On_Service
go
use Excell_On_Service
go

CREATE TABLE Categories(
	CategoriesID int Primary Key identity,
	CategoriesName nvarchar(100),
	
)
-- Tạo bảng Services (Dịch vụ)
CREATE TABLE [Services] (
    ServiceID INT PRIMARY KEY identity,
    ServiceName NVARCHAR(100) NOT NULL,
    ServiceDescription NVARCHAR(255) , --mô tả dịch vu
    ServiceCharge DECIMAL(18, 2) NOT NULL--giá dịch vụ
);


-- Tạo bảng Departments (Phòng ban)
CREATE TABLE [Departments] (
    DepartmentID INT PRIMARY KEY identity,
    DepartmentName NVARCHAR(100) NOT NULL,
    DepartmentDescription NVARCHAR(255)NOT NULL --mô tả
);

-- Tạo bảng Employees (Nhân viên)
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY identity,
    EmployeeName NVARCHAR(100) NOT NULL,
    DepartmentID INT NOT NULL,
    Designation NVARCHAR(100) NOT NULL, --tên công việc
	ServiceID INT NOT NULL,
    Salary DECIMAL(18, 2) NOT NULL,--lương
	Email NVARCHAR(100),
    JoinDate DATE NOT NULL,--ngày vào làm
	Is_Active BIT,
	Username NVARCHAR(50) NOT NULL , 
    [Password] NVARCHAR(255) NOT NULL ,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
	FOREIGN KEY (ServiceID) REFERENCES [Services](ServiceID)
);

-- Tạo bảng Clients (Khách hàng)
CREATE TABLE [Clients] (
    ClientID INT PRIMARY KEY identity,
    ClientName NVARCHAR(100),
	CompanyName NVARCHAR(100),--tên công ty
    ContactPerson NVARCHAR(100),--tên người đại diên 
    ContactNumber NVARCHAR(20),-- số liên hệ(số diện thoai)
    Email NVARCHAR(100),
	[Image] nvarchar(100),
    [Address] NVARCHAR(255),
    Username NVARCHAR(50) NOT NULL, 
	[Password] NVARCHAR(255) NOT NULL ,
);

-- Tạo bảng ClientServices (Dịch vụ của khách hàng) là bảng tạo ra từ quan hệ nhiều nhiều của bảng client và bảng Service
CREATE TABLE ClientServices (
    ClientServiceID INT PRIMARY KEY identity,
    ClientID INT,
    ServiceID INT,
    ServiceStartDate DATE, --ngày bắt đầu 
    ServiceEndDate DATE,--ngày kết thúc
	EmployeeID int,
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID),
    FOREIGN KEY (ServiceID) REFERENCES [Services](ServiceID),
	FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

-- Tạo bảng Products (Sản phẩm)
CREATE TABLE [Products] (
    ProductID INT PRIMARY KEY identity,
    ProductName NVARCHAR(100),
    ProductDescription NVARCHAR(255),-- mô tả sản phẩm 
    ClientID INT, --
	[Image] nvarchar(100),
	ServiceID INT,--ten dịch vụ đi kèm
	CategoriesID int,
    UnitPrice DECIMAL(18, 2),--giá 
    QuantityInStock INT, --sô lượng còn lại
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID),
	FOREIGN KEY (CategoriesID) REFERENCES Categories(CategoriesID),
	
);
-- Tạo bảng Payments (Thanh toán)
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY identity,
    ClientID INT,
    PaymentAmount DECIMAL(18, 2), --tổng số tiền cần thanh toán 
    PaymentDate DATE,--nagyf thanh toán
    PaymentMethod NVARCHAR(100),
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID)
);

-- Create the CallLogs table
CREATE TABLE CallLogs (
  CallLogID INT IDENTITY(1,1) PRIMARY KEY,
  EmployeeID INT NOT NULL, --nhân viên nhận cuộc gọi cũng như chủ động gọi để quảng cáo 
  ClientID INT NOT NULL, -- người gọi
  ServiceID INT NOT NULL, -- dịch vụ được nhắc đến 
  CallDateTime DATETIME NOT NULL, --ngày giờ gọi
  CallType nvarchar(55) NOT NULL, -- phương thưc gọi đi hoặc gọi đến
  Notes TEXT, -- độ hài lòng của khách hàng
 
);
 Create Table [Order] (
	OrderID int primary key identity,
	ClientID int,
	ProductID int,
	ServiceID int,
	Total Decimal(10,2),
	PaymentID int,
	DateOrder Datetime,
	FOREIGN key(PaymentID) REFERENCES Payments(PaymentID),
	FOREIGN key(ClientID) REFERENCES Clients(ClientID),
 )

 -- Chèn dữ liệu vào bảng Categories
INSERT INTO Categories (CategoriesName) VALUES
('Gói di động'),
('Dịch vụ Internet'),
('Dịch vụ Điện thoại cố định');

-- Chèn dữ liệu vào bảng Services
INSERT INTO [Services] (ServiceName, ServiceDescription, ServiceCharge) VALUES
('Inbound', ' vận chuyển hàng hóa từ nguồn cung cấp đến kho hoặc nhà máy sản xuất.',4500.00),
('OutBound', 'vận chuyển hàng hóa từ kho hoặc nhà máy sản xuất đến khách hàng cuối cùng', 6000.00),
('Tele Maketing', 'cầu nối kết nối giữa doanh nghiệp và khách hàng.', 5500.00);

-- Chèn dữ liệu vào bảng Departments
INSERT INTO Departments (DepartmentName, DepartmentDescription) VALUES
('Bộ phận Bán hàng', 'Chịu trách nhiệm bán các dịch vụ viễn thông'),
('Bộ phận Hỗ trợ kỹ thuật', 'Cung cấp hỗ trợ kỹ thuật cho khách hàng'),
('Bộ phận Thanh toán', 'Xử lý các yêu cầu thanh toán từ khách hàng');

-- Chèn dữ liệu vào bảng Employees
INSERT INTO Employees (EmployeeName, DepartmentID, Designation, ServiceID, Salary, Email, JoinDate, Is_Active, Username, [Password]) VALUES
('Nguyễn Văn A', 1, 'Nhân viên bán hàng', 1, 5000.00, 'nguyen.van.a@example.com', '2023-01-01', 1, 'nva_username', 'nva_password'),
('Trần Thị B', 2, 'Chuyên viên hỗ trợ kỹ thuật', 2, 4000.00, 'tran.thi.b@example.com', '2023-01-01', 1, 'ttb_username', 'ttb_password'),
('Hoàng Minh C', 3, 'Chuyên viên thanh toán', 3, 3500.00, 'hoang.minh.c@example.com', '2023-01-01', 1, 'hmc_username', 'hmc_password');

-- Chèn dữ liệu vào bảng Clients
INSERT INTO Clients (ClientName, CompanyName, ContactPerson, ContactNumber, Email, [Image], [Address], Username, [Password]) VALUES
('Viễn thông ABC', 'Công ty Viễn thông ABC', 'Nguyễn Văn Khánh', '1234567890', 'khanh.nguyen@viễnthongabc.com', 'abc_logo.jpg', '123 Đường ABC, Thành phố XYZ', 'abc_username', 'abc_password'),
('Giao thông XYZ', 'Công ty Giao thông XYZ', 'Trần Thị Lan', '0987654321', 'lan.tran@giaothongxyz.com', 'xyz_logo.jpg', '456 Đường XYZ, Thành phố ABC', 'xyz_username', 'xyz_password');

-- Chèn dữ liệu vào bảng ClientServices
INSERT INTO ClientServices (ClientID, ServiceID, ServiceStartDate, ServiceEndDate, EmployeeID) VALUES
(1, 1, '2023-01-01', '2024-01-01', 1),
(2, 2, '2023-01-01', '2024-01-01', 2);

-- Chèn dữ liệu vào bảng Products
INSERT INTO [Products] (ProductName, ProductDescription, ClientID, [Image], ServiceID, CategoriesID, UnitPrice, QuantityInStock) VALUES
('Thiết bị di động', 'Mẫu smartphone mới nhất', 1, 'mobile_device.jpg', 1, 1, 500.00, 50),
('Bộ định tuyến', 'Bộ định tuyến hiệu suất cao cho internet tại nhà', 2, 'router.jpg', 2, 2, 100.00, 100);

-- Chèn dữ liệu vào bảng Payments
INSERT INTO Payments (ClientID, PaymentAmount, PaymentDate, PaymentMethod) VALUES
(1, 1000.00, '2023-01-01', 'Thẻ tín dụng'),
(2, 1500.00, '2023-01-01', 'Chuyển khoản ngân hàng');

-- Chèn dữ liệu vào bảng CallLogs
INSERT INTO CallLogs (EmployeeID, ClientID, ServiceID, CallDateTime, CallType, Notes) VALUES
(1, 2, 2, '2023-01-01 10:00:00', 'Gọi đi', 'Hỏi về tốc độ internet'),
(2, 1, 1, '2023-01-01 11:00:00', 'Gọi đến', 'Quan tâm đến việc nâng cấp gói di động');
