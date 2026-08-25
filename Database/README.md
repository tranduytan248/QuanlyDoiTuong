# Script cơ sở dữ liệu — Phân quyền dữ liệu, Tra cứu & Log cập nhật

Thư mục này chứa các script T-SQL cần chạy để kích hoạt các tính năng mới.

> [!IMPORTANT]
> **Chưa chạy script = chức năng chưa hoạt động.** Đã kiểm tra thực tế trên
> `quanlydoituong.tdt.vn`: các màn hình mở được (HTTP 200) nhưng
> `/Major/Subject/Get` và `/Major/SubjectViolation/Get` trả **lỗi 500**, và
> `/Cate/UserField/Get` trả **403**, vì các stored procedure / bảng / đăng ký
> chức năng dưới đây chưa tồn tại trong CSDL.

## Tên bảng đã được đối chiếu với CSDL thật

Đã truy vấn `sys.tables` trên máy chủ `10.57.30.10` (CSDL `quanlydoituong.cenit.vn`).
Tên bảng thực tế ở dạng **số nhiều**, các script đã được sửa cho khớp:

| Tên theo quy ước | Tên thật trong CSDL |
|---|---|
| `Major_Subject` | `Major_Subjects` |
| `Major_SubjectViolation` | `Major_SubjectViolations` |
| `Major_SubjectViolation_Behavior` | `Major_SubjectViolation_Behaviors` |
| `Cate_ViolationBehavior` | `Cate_ViolationBehaviors` |
| `Cate_Field` | `Cate_Fields` |
| `Cate_Union` | `Cate_Unions` |
| `Cate_Union_Member` | `Cate_Unions_Members` |
| `Cate_Union_Manager` | `Cate_Unions_Mangers` ⚠️ *(lỗi chính tả có sẵn trong CSDL)* |
| `Sys_Config` | `Sys_Configs` |

Riêng **tên cột** thì chưa kiểm tra được hết. Script `00` sẽ báo cột nào thiếu.

## Thứ tự chạy

| # | File | Nội dung |
|---|------|----------|
| 00 | `00_KiemTra_TenBang.sql` | **Chạy trước.** Kiểm tra bảng + cột, và sao lưu nội dung proc cũ. |
| 01 | `01_SysUserField_Table.sql` | Bảng `Sys_User_Field` — phân quyền lĩnh vực. |
| 02 | `02_SubjectViolation_Reporter_Columns.sql` | Cột người khai báo + `ReporterUnionId`. |
| 03 | `03_Scope_Functions.sql` | Hàm xác định phạm vi dữ liệu (đệ quy theo cây đơn vị). |
| 04 | `04_Major_Subject_Get.sql` | Tra cứu đối tượng — phân quyền + tra cứu 3 tiêu chí. |
| 05 | `05_Major_SubjectViolation.sql` | Lưu / tra cứu vi phạm + cờ `IsOwner`, `FieldNames`. |
| 06 | `06_SysUserField_Procs.sql` | Proc cho màn hình phân quyền lĩnh vực. |
| 07 | `07_Major_Subject_Save.sql` | Lưu đối tượng — thêm `@ReporterUnionId`. |
| 08 | `08_Menu_ReOrganize.sql` | Sắp xếp lại menu. |
| 09 | `09_Permission_Register.sql` | Đăng ký chức năng mới *(sửa lỗi 403 của UserField)*. |
| 10 | `10_Major_Subject_ChangeLog.sql` | Bảng log cập nhật + xoá mềm đối tượng. |

> [!WARNING]
> Script 04, 05, 07, 10 **thay thế** các proc đang chạy. Hãy chạy phần 4 của
> script `00` để sao lưu nội dung proc cũ trước, phòng khi cần khôi phục.

## Quy tắc phân quyền dữ liệu

Phạm vi đơn vị là **đệ quy theo cây** (`fn_GetPermittedUnions`):
Đơn vị 1 là cha của Đơn vị 2; Đơn vị 2 là cha của 3, 4, 5 → người được phân quản lý
**Đơn vị 1** sẽ thấy dữ liệu của **2, 3, 4 và 5**.

Người dùng thấy được một đối tượng / lần vi phạm khi thoả **đồng thời**:

1. **Đơn vị khai báo** nằm trong phạm vi của họ.
2. **Lĩnh vực** của hành vi vi phạm nằm trong danh sách được phân công.

| Người dùng | Đơn vị | Lĩnh vực | Thấy được |
|---|---|---|---|
| A | Tổ 1 | A1, A2 | Đối tượng & vi phạm do Tổ 1 khai báo, thuộc A1 hoặc A2 |
| B | Tổ 2 | A2, A3 | Đối tượng & vi phạm do Tổ 2 khai báo, thuộc A2 hoặc A3 |
| C | Phòng A, quản lý Tổ 1 + 2 | A2 | Đối tượng & vi phạm do Phòng A, Tổ 1, Tổ 2 và mọi đơn vị con khai báo, **chỉ** thuộc A2 |

**Super admin** (khoá `CONFIG_SUPER_ADMIN_PERMIT` trong `Sys_Configs`) bỏ qua mọi giới hạn.

## Quyền thao tác

| Chức năng | Ai được làm |
|---|---|
| Thêm đối tượng | Người có quyền `Add` trên `Major/Subject` (nút tự ẩn nếu không có quyền) |
| Sửa / Xoá đối tượng | Người có quyền `Edit` / `Delete` trên `Major/Subject` |
| Sửa / Xoá một lần vi phạm | **Chỉ đúng tài khoản đã khai báo** (`CreatedBy`) hoặc super admin |
| Xem lịch sử vi phạm | Bất kỳ ai trong phạm vi phân quyền (nút "Xem") |

Xoá đối tượng là **xoá mềm** (gán cờ `IsDeleted`), dữ liệu vẫn giữ để báo cáo.

## Sau khi chạy script

1. Build lại solution và deploy (`trunk/Release`).
2. **Danh mục → Phân quyền lĩnh vực**: gán lĩnh vực cho từng người dùng.
3. Màn hình **Đơn vị**: gán người quản lý đơn vị (tổ / phòng).
4. Màn hình **Phân quyền**: cấp quyền `Add` / `Edit` / `Delete` trên `Major/Subject`.

> [!NOTE]
> **Dữ liệu cũ** có `ReporterUnionId = NULL` sẽ không hiển thị với người dùng
> thường sau khi bật phân quyền. Cần backfill theo `CreatedBy`:
>
> ```sql
> UPDATE s
> SET s.ReporterUnionId = m.UnionId
> FROM dbo.Major_Subjects AS s
> INNER JOIN dbo.Cate_Unions_Members AS m ON m.UserName = s.CreatedBy
> WHERE s.ReporterUnionId IS NULL;
>
> UPDATE v
> SET v.ReporterUnionId = m.UnionId
> FROM dbo.Major_SubjectViolations AS v
> INNER JOIN dbo.Cate_Unions_Members AS m ON m.UserName = v.CreatedBy
> WHERE v.ReporterUnionId IS NULL;
> ```
