# 🛍️ eShop - Hệ Thống Thương Mại Điện Tử & Quản Lý Đơn Hàng

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-InteractiveServer-512BD4?style=flat&logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?style=flat&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Dapper](https://img.shields.io/badge/ORM-Dapper-blue)](https://github.com/DapperLib/Dapper)
[![SQL Server](https://img.shields.io/badge/Database-SQL_Server-CC292B?style=flat&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/sql-server)
[![Bootstrap](https://img.shields.io/badge/UI-Bootstrap_5-7952B3?style=flat&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)

---

## 👨‍💻 Thông Tin Sinh Viên Thực Hiện

* **Họ và tên:** Trần Viết Mạnh Cường
* **Mã sinh viên (MSSV):** 23K4080003
* **GitHub Repository:** [https://github.com/ManhCuong-Code/eShop.Web](https://github.com/ManhCuong-Code/eShop.Web)

---

## 📖 Giới Thiệu Dự Án

**eShop** là một ứng dụng thương mại điện tử hiện đại, toàn diện được xây dựng trên nền tảng **ASP.NET Core Blazor (Interactive Server)** với phiên bản **.NET 10**. 

Dự án áp dụng chặt chẽ mô hình kiến trúc **Clean Architecture (Onion Architecture)** kết hợp với **Plugin Pattern**, phân tách rành mạch giữa nghiệp vụ cốt lõi, use case xử lý logic, tầng lưu trữ dữ liệu (Data Access) và các module giao diện người dùng (Customer Portal, Admin Portal).

---

## 🏗️ Kiến Trúc Hệ Thống (Clean Architecture & Plugin Pattern)

Hệ thống được tổ chức thành các project chuyên biệt, đảm bảo tính đóng gói, dễ mở rộng và dễ bảo trì:

```
eShop/
│
├── 📦 eShop.CoreBusiness/                  # Tầng Nghiệp Vụ Cốt Lõi (Domain Layer)
│   └── Models/ (Product, Order, OrderLineItem)
│
├── ⚙️ eShop.UseCases/                       # Tầng Ca Sử Dụng (Application Layer)
│   ├── PluginInterfaces/ (DataStore, StateStore, UI)
│   ├── ShoppingCartScreen/ (ViewCart, AddToCart, DeleteProduct, UpdateQuantity, PlaceOrder)
│   ├── OrderConfirmationScreen/ (OrderConfirmation)
│   └── AdminPortal/ (OutstandingOrders, ProcessedOrders, ProcessOrder, ViewOrderDetail)
│
├── 🔌 Plugins/                             # Tầng Triển Khai Hạ Tầng & Dịch Vụ (Infrastructure)
│   ├── eShop.DataStore.SQL.Dapper/         # Lưu trữ CSDL SQL Server sử dụng Dapper micro-ORM
│   ├── eShop.DataStore.HandCoded/          # In-memory Mock DataStore dùng trong thử nghiệm
│   ├── eShop.ShoppingCart.LocalStorage/    # Lưu trữ giỏ hàng trên trình duyệt của người dùng
│   └── eShop.StateStore.DI/                # Quản lý trạng thái tương tác giỏ hàng thời gian thực (State Store)
│
├── 🖥️ eShop.Web.Modules/                   # Các Module Giao Diện Người Dùng (Blazor Components)
│   ├── eShop.Web.Common/                   # Controls dùng chung (SearchBarComponent)
│   ├── eShop.Web.CustomerPortal/           # Cổng thông tin & Trải nghiệm mua sắm của Khách Hàng
│   └── eShop.Web.AdminPortal/              # Cổng quản trị dành cho Người Quản Trị Hệ Thống
│
└── 🚀 eShop.Web/                           # Ứng Dụng Web Host (Entry Point)
    ├── Components/ (Layout, App.razor, Routes.razor, Pages)
    └── wwwroot/ (CSS, Bootstrap, Assets)
```

---

## ✨ Các Tính Năng Nổi Bật

### 🛒 1. Khách Hàng (Customer Portal)
* **Trang chủ & Danh mục sản phẩm:** Xem danh sách sản phẩm với thẻ hiển thị hình ảnh, thương hiệu, giá tiền và hiệu ứng hover hiện đại.
* **Tìm kiếm sản phẩm:** Thanh tìm kiếm tức thời theo tên và thương hiệu.
* **Chi tiết sản phẩm:** Xem hình ảnh kích thước lớn, thông tin thương hiệu, trạng thái tồn kho, mô tả chi tiết và nút thêm vào giỏ hàng.
* **Giỏ hàng trực quan (Realtime State):**
  * Tăng/giảm số lượng sản phẩm tức thì.
  * Xoá từng mặt hàng ra khỏi giỏ.
  * Tự động tính toán tổng số tiền hàng, phí vận chuyển và cập nhật huy hiệu số lượng trên thanh Menu điều hướng.
* **Quy trình Thanh toán & Đặt hàng (Checkout Flow):**
  * Thanh tiến trình 3 bước (Stepper): `1. Giỏ hàng` $\rightarrow$ `2. Thông tin giao hàng` $\rightarrow$ `3. Hoàn tất đơn hàng`.
  * Nhập thông tin người nhận (Họ tên, địa chỉ, tỉnh/thành phố, quận/huyện, quốc gia) với input-group có icon và cảnh báo hợp lệ.
  * Thẻ tóm tắt đơn hàng (Order Summary) hiển thị danh sách sản phẩm thu nhỏ, tổng tiền và cam kết an toàn, bảo mật.
  * Phương thức thanh toán khi nhận hàng (**COD**).
* **Xác nhận đặt hàng thành công (Order Confirmation):**
  * Mã tra cứu đơn hàng duy nhất (`UniqueId`) kèm nút **"Sao chép mã"**.
  * Timeline theo dõi trạng thái đơn hàng: *Đã Đặt Hàng* $\rightarrow$ *Chờ Xử Lý* $\rightarrow$ *Đang Giao Hàng* $\rightarrow$ *Đã Nhận Hàng*.
  * Bảng chi tiết mặt hàng đã đặt, tính năng **"In Hoá Đơn"** (`window.print`) và nút tiếp tục mua sắm.

### 🔐 2. Quản Trị Viên (Admin Portal)
* **Xác thực bảo mật:** Đăng nhập và phân quyền truy cập thông qua Cookie Authentication.
* **Quản lý đơn hàng chờ xử lý (Outstanding Orders):** Xem danh sách tất cả các đơn hàng khách vừa đặt, lọc theo trạng thái.
* **Xem chi tiết đơn hàng:** Đầy đủ thông tin khách hàng, số lượng từng sản phẩm và tổng tiền.
* **Xử lý đơn hàng (Process Order):** Xác nhận duyệt đơn, hệ thống ghi nhận thời gian xử lý và tên người quản trị duyệt.
* **Lịch sử đơn hàng đã xử lý (Processed Orders):** Báo cáo và theo dõi danh sách các đơn hàng đã được xử lý thành công.

### 🎨 3. Giao Diện & Trải Nghiệm (UI/UX)
* Phông chữ chuẩn hiện đại: **Plus Jakarta Sans** (Google Fonts).
* Hệ thống biểu tượng: **Bootstrap Icons 1.11.3**.
* Phối màu tao nhã, phong cách thiết kế phẳng, thẻ bo tròn mềm mại và hỗ trợ đầy đủ responsive trên Mobile, Tablet, Desktop.

---

## 🛠️ Công Nghệ & Thư Viện Sử Dụng

| Hạng mục | Công nghệ / Thư viện |
| :--- | :--- |
| **Nền tảng** | .NET 10.0 SDK |
| **Ngôn ngữ** | C# 13 |
| **Framework Web** | ASP.NET Core Blazor (Interactive Server) |
| **ORM & Truy cập dữ liệu** | Dapper Micro-ORM, Microsoft.Data.SqlClient |
| **Hệ quản trị CSDL** | Microsoft SQL Server / LocalDB |
| **Frontend Styling** | Bootstrap 5, Bootstrap Icons, Custom CSS |
| **Kiến trúc phần mềm** | Clean Architecture, CQRS / Use Case Driven, Plugin Pattern |

---

## 🚀 Hướng Dẫn Cài Đặt & Chạy Ứng Dụng

### 1. Yêu Cầu Hệ Thống
* Đã cài đặt [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* Đã cài đặt [SQL Server](https://www.microsoft.com/sql-server) hoặc SQL Server Express / LocalDB
* Trình soạn thảo: [Visual Studio 2022 / 2025+](https://visualstudio.microsoft.com/) hoặc [Visual Studio Code](https://code.visualstudio.com/)

### 2. Clone Repository
```bash
git clone https://github.com/ManhCuong-Code/eShop.Web.git
cd eShop.Web
```

### 3. Cấu Hình Chuỗi Kết Nối Cơ Sở Dữ Liệu
Mở file `eShop/eShop.Web/appsettings.json` và cấu hình chuỗi kết nối SQL Server của bạn:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=eShop;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Build và Khởi Chạy Ứng Dụng
```bash
# Phục hồi packages
dotnet restore

# Biên dịch ứng dụng
dotnet build

# Chạy ứng dụng
dotnet run --project "eShop/eShop.Web/eShop.Web.csproj"
```

Ứng dụng sẽ khởi chạy tại:
* **HTTP:** `http://localhost:5204`
* **HTTPS:** `https://localhost:7251`

---

## 📜 Giấy Phép & Bản Quyền

Dự án được xây dựng và phát triển phục vụ mục đích học tập và nghiên cứu công nghệ phát triển ứng dụng web hiện đại với .NET Core và Blazor.

&copy; 2026 **Trần Viết Mạnh Cường (MSSV: 23K4080003)**. All rights reserved.
