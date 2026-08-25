# GEMINI.md - Hướng Dẫn & Quy Tắc Phát Triển Hệ Thống

Tài liệu này là quy chuẩn bắt buộc dành cho AI Assistant và Lập trình viên khi tham gia phát triển, bảo trì và refactor mã nguồn trong dự án **Phần mềm Quản lý Đối tượng**.

---

## 1. Thông Tin Chung & Kiến Trúc Dự Án

- **Tên dự án**: Phần mềm Quản lý Đối tượng
- **Framework**: .NET Framework 4.8 (ASP.NET MVC 5, Web API 2)
- **Kiến trúc**: N-Layer (TSFramework Architecture)
  - `TSFramework.*`: Core framework, base attributes, helpers, caching, provider layer.
  - `Cores.*`: Business Logic (`Biz`), Cache Layer (`Caches`), DTO/Models (`Models`), `Enums`.
  - `Extends.*`: Các module mở rộng (Notifications: Email, SMS, Zalo, InSite, Message).
  - `Modules.*`: Controllers và Views theo từng phân hệ (`Modules.Sys`, `Modules.Cate`, `Modules.Major`, `Modules.eContract`).
  - `CenIT.Solution.QLHD.WebApp`: Ứng dụng Web chính khởi chạy trên IIS.
  - `Reports.*`: Các module báo cáo RDLC.
- **Cơ sở dữ liệu**: Microsoft SQL Server.
- **Build Engine**: MSBuild Visual Studio 2022 (`MSBuild.exe CenIT.Solution.QLHD.sln /p:Configuration=Release /m`).

---

## 2. Quy Tắc Encoding & Tiếng Việt (BẮT BUỘC - CRITICAL)

> [!IMPORTANT]
> **Toàn bộ source code, views và tài liệu trong hệ thống PHẢI TUÂN THỦ CHUẨN UTF-8 CÓ BOM (`UTF-8 with BOM` / `utf-8-sig`).**

1. **Chuẩn mã hóa file**:
   - Mọi file `.cshtml`, `.cs`, `.xml`, `.config`, `.json`, `.js`, `.css`, `.html`, `.sql` khi tạo mới hoặc chỉnh sửa **BẮT BUỘC** lưu dưới định dạng **UTF-8 with BOM**.
   - Tuyệt đối không lưu file dưới dạng ANSI, Windows-1252, UTF-16 hoặc UTF-8 No-BOM (vì ASP.NET Razor trên Windows sẽ đọc file No-BOM bằng ANSI gây lỗi vỡ font Mojibake như `PHÁº§N MÁ»□M`).
2. **Không gây lỗi Font / Unicode**:
   - Cấm xuất hiện ký tự `?`, ký tự thay thế `` (`\ufffd`), hoặc chuỗi vỡ font.
   - Khi chỉnh sửa file bằng script/công cụ (Python/PowerShell), phải luôn chỉ định encoding rõ ràng (`utf-8-sig` trong Python, `[System.Text.UTF8Encoding]($true)` trong .NET/PowerShell).
3. **Cấu hình Globalization trong Web.config**:
   - Giữ nguyên cấu hình trong `<system.web>`:
     ```xml
     <globalization fileEncoding="utf-8" requestEncoding="utf-8" responseEncoding="utf-8" uiCulture="vi" culture="vi-VN" />
     ```

---

## 3. Quy Tắc Lập Trình C# (C# Coding Rules)

### 3.1. Naming Convention
- **Class / Interface / Struct / Enum**: `PascalCase` (Interface luôn có tiền tố `I`, ví dụ: `ICustomerService`).
- **Method / Property / Constant**: `PascalCase` (Method luôn bắt đầu bằng động từ, ví dụ: `GetCustomer()`, `CreateContract()`).
- **Private Field**: `camelCase` có tiền tố dấu gạch dưới `_` (ví dụ: `_customerRepository`, `_invAdjustCache`).
- **Local Variable / Parameter**: `camelCase` (ví dụ: `customerId`, `searchModel`).
- **Boolean Properties / Variables**: Đặt tên có tiền tố thể hiện trạng thái (`IsActive`, `HasPermission`, `CanEdit`).

### 3.2. Code Quality & Logic
- **Không tự ý thay đổi kiến trúc**: Giữ nguyên cấu trúc thư mục, namespace, class hierarchy hiện có.
- **Không dùng Magic Number / Magic String**: Luôn định nghĩa và sử dụng Enum hoặc Constants (ví dụ: `EnumInvType`, `ConstsCusType`).
- **Xử lý Exception**:
  - Không dùng empty catch block `catch (Exception) {}`.
  - Luôn log lỗi và trả về thông báo an toàn, thân thiện cho người dùng.
- **Asynchronous Code**: Khi khai báo method `async`, phải luôn có lệnh `await` hợp lệ; tránh khai báo `async` rỗng gây cảnh báo `CS1998`.
- **Cập nhật Project File (`.csproj`)**: Khi thêm mới bất kỳ file `.cs`, `.cshtml` hoặc reference thư viện DLL mới, phải cập nhật đúng mục `<Compile Include="..." />`, `<Content Include="..." />` hoặc `<Reference Include="..." />` trong file `.csproj` tương ứng.

---

## 4. Quy Tắc Cơ Sở Dữ Liệu (SQL Server Rules)

1. **Đặt tên đối tượng**:
   - Table: `PascalCase` (dạng số ít, ví dụ: `Contract`, `Customer`, `Department`).
   - Primary Key: Luôn đặt là `Id`.
   - Foreign Key: `<TableName>Id` (ví dụ: `CustomerId`, `DepartmentId`).
   - Stored Procedure: `usp_<Action><Entity>` (ví dụ: `usp_GetCustomer`, `usp_UpdateContractStatus`).
   - View: `vw_<Entity>` (ví dụ: `vw_ContractOverview`).
   - Function: `fn_<Action><Entity>` (ví dụ: `fn_CalculateRemainingDays`).
2. **Truy vấn SQL an toàn & tối ưu**:
   - **Tuyệt đối không dùng `SELECT *`**: Luôn liệt kê rõ ràng danh sách các cột cần truy vấn.
   - **JOIN rõ ràng**: Luôn dùng cú pháp `INNER JOIN`, `LEFT JOIN` tường minh; không dùng `FROM TableA, TableB`.
   - **Chống SQL Injection**: Luôn sử dụng Parameterized Query hoặc Stored Procedure; không nối chuỗi SQL động với dữ liệu đầu vào.
   - **Hiệu năng**: Bắt buộc tạo Index cho các Foreign Key và các cột thường xuyên được `WHERE`, `ORDER BY`, `JOIN`.

---

## 5. Quy Tắc Source Control & Bảo Mật

- **Không commit các tệp tin build/tạm**:
  - Thư mục `bin/`, `obj/`, `publish/`, `packages/`, `.vs/`, `TestResults/`.
- **Bảo mật**:
  - Không hardcode mật khẩu, token, secret key hoặc Production Connection String vào code.
  - Sử dụng Web.config / App.config và biến môi trường (Environment Variables / GitHub Secrets) để quản lý cấu hình nhạy cảm.

---

## 6. Quy Trình Build, Test & Deployment (Skill: `upcode`)

1. **Build Solution**:
   - Sử dụng MSBuild 2022 để kiểm tra lỗi biên dịch trước khi triển khai:
     ```powershell
     & "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "d:\SVN\QuanlyDoiTuong\trunk\Source\CenIT.Solution.QLHD\CenIT.Solution.QLHD.sln" /p:Configuration=Release /m
     ```
2. **Kiểm tra cục bộ**:
   - Kiểm tra trên IIS local qua domain: `http://quanlydoituong.tdt.vn` (127.0.0.1).
3. **Deploy lên FTP Server (`upcode`)**:
   - Sử dụng skill `upcode` hoặc chạy script deploy:
     ```powershell
     powershell -ExecutionPolicy Bypass -File ".agents/skills/upcode/scripts/deploy_ftp.ps1" -SourcePath "trunk/Release" -Force
     ```
