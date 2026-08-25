/* =============================================================================
   00. KIỂM TRA CẤU TRÚC CSDL TRƯỚC KHI CHẠY CÁC SCRIPT SAU
   -----------------------------------------------------------------------------
   CHẠY SCRIPT NÀY ĐẦU TIÊN và đọc kỹ kết quả.

   Tên bảng trong các script đã được đối chiếu với CSDL thật
   (máy chủ 10.57.30.10, CSDL quanlydoituong.cenit.vn) và dùng dạng SỐ NHIỀU:

       Major_Subjects, Major_SubjectViolations,
       Major_SubjectViolation_Behaviors, Cate_ViolationBehaviors,
       Cate_Fields, Cate_Unions, Cate_Unions_Members,
       Cate_Unions_Mangers  (lưu ý: lỗi chính tả có sẵn trong CSDL),
       Sys_Configs

   Phần 2 và 3 kiểm tra các CỘT mà script cần dùng. Nếu có dòng nào báo THIẾU,
   hãy sửa lại tên cột tương ứng trong các script 03-07, 10 rồi mới chạy tiếp.
   ============================================================================= */

SET NOCOUNT ON;

PRINT N'===== 1. CÁC BẢNG SCRIPT CẦN DÙNG =====';

;WITH CanCo(TenBang, MoTa) AS
(
    SELECT 'Major_Subjects',                   N'Đối tượng'                      UNION ALL
    SELECT 'Major_SubjectViolations',          N'Lịch sử vi phạm'                UNION ALL
    SELECT 'Major_SubjectViolation_Behaviors', N'Bảng nối vi phạm - hành vi'     UNION ALL
    SELECT 'Cate_ViolationBehaviors',          N'Danh mục hành vi vi phạm'       UNION ALL
    SELECT 'Cate_Fields',                      N'Danh mục lĩnh vực'              UNION ALL
    SELECT 'Cate_Unions',                      N'Danh mục đơn vị'                UNION ALL
    SELECT 'Cate_Unions_Members',              N'Thành viên đơn vị'              UNION ALL
    SELECT 'Cate_Unions_Mangers',              N'Người quản lý đơn vị'           UNION ALL
    SELECT 'Sys_Configs',                      N'Cấu hình hệ thống'              UNION ALL
    SELECT 'Sys_Users',                        N'Người dùng'                     UNION ALL
    SELECT 'Sys_Menus',                        N'Menu'
)
SELECT
    c.TenBang,
    c.MoTa,
    CASE WHEN OBJECT_ID('dbo.' + c.TenBang, 'U') IS NOT NULL
         THEN N'OK'
         ELSE N'>>> KHÔNG TÌM THẤY'
    END AS TrangThai
FROM CanCo AS c
ORDER BY TrangThai DESC, c.TenBang;


PRINT '';
PRINT N'===== 2. CÁC CỘT SCRIPT CẦN DÙNG =====';

;WITH CanCo(TenBang, TenCot, MoTa) AS
(
    SELECT 'Major_Subjects', 'SubjectId',          N'Khoá chính'                 UNION ALL
    SELECT 'Major_Subjects', 'IdentityCardNumber', N'Số CCCD'                    UNION ALL
    SELECT 'Major_Subjects', 'FullName',           N'Họ tên'                     UNION ALL
    SELECT 'Major_Subjects', 'PlaceOfOrigin',      N'Quê quán'                   UNION ALL
    SELECT 'Major_Subjects', 'IsDeleted',          N'Cờ xoá mềm'                 UNION ALL
    SELECT 'Major_Subjects', 'CreatedBy',          N'Người tạo'                  UNION ALL

    SELECT 'Major_SubjectViolations', 'ViolationId',   N'Khoá chính'             UNION ALL
    SELECT 'Major_SubjectViolations', 'SubjectId',     N'Khoá đối tượng'         UNION ALL
    SELECT 'Major_SubjectViolations', 'ViolationDate', N'Ngày vi phạm'           UNION ALL
    SELECT 'Major_SubjectViolations', 'IsDeleted',     N'Cờ xoá mềm'             UNION ALL
    SELECT 'Major_SubjectViolations', 'CreatedBy',     N'Người khai báo'         UNION ALL

    SELECT 'Major_SubjectViolation_Behaviors', 'ViolationId', N'Khoá vi phạm'    UNION ALL
    SELECT 'Major_SubjectViolation_Behaviors', 'BehaviorId',  N'Khoá hành vi'    UNION ALL

    SELECT 'Cate_ViolationBehaviors', 'BehaviorId',   N'Khoá chính'              UNION ALL
    SELECT 'Cate_ViolationBehaviors', 'BehaviorName', N'Tên hành vi'             UNION ALL
    SELECT 'Cate_ViolationBehaviors', 'FieldId',      N'Khoá lĩnh vực'           UNION ALL

    SELECT 'Cate_Fields', 'FieldId',   N'Khoá chính'                             UNION ALL
    SELECT 'Cate_Fields', 'FieldName', N'Tên lĩnh vực'                           UNION ALL

    SELECT 'Cate_Unions', 'UnionId',     N'Khoá chính'                           UNION ALL
    SELECT 'Cate_Unions', 'BelongUnion', N'Đơn vị cha (phân cấp)'                UNION ALL

    SELECT 'Cate_Unions_Members', 'BelongUnion', N'Khoá đơn vị'                  UNION ALL
    SELECT 'Cate_Unions_Members', 'UserName', N'Tài khoản thành viên'            UNION ALL

    SELECT 'Cate_Unions_Mangers', 'UnionId', N'Khoá đơn vị'                      UNION ALL
    SELECT 'Cate_Unions_Mangers', 'Manager', N'Tài khoản quản lý'                UNION ALL

    SELECT 'Sys_Configs', 'ConfigKey',   N'Khoá cấu hình'                        UNION ALL
    SELECT 'Sys_Configs', 'ConfigValue', N'Giá trị cấu hình'
)
SELECT
    c.TenBang,
    c.TenCot,
    c.MoTa,
    CASE WHEN OBJECT_ID('dbo.' + c.TenBang, 'U') IS NULL
              THEN N'(bảng không tồn tại)'
         WHEN COL_LENGTH('dbo.' + c.TenBang, c.TenCot) IS NOT NULL
              THEN N'OK'
         ELSE N'>>> THIẾU CỘT - phải sửa script'
    END AS TrangThai
FROM CanCo AS c
ORDER BY TrangThai DESC, c.TenBang, c.TenCot;


PRINT '';
PRINT N'===== 3. CỘT THỰC TẾ CỦA CÁC BẢNG CHÍNH (để đối chiếu thủ công) =====';

SELECT TABLE_NAME AS TenBang, COLUMN_NAME AS TenCot, DATA_TYPE AS KieuDuLieu
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Major_Subjects', 'Major_SubjectViolations',
                     'Major_SubjectViolation_Behaviors', 'Cate_Unions_Mangers')
ORDER BY TABLE_NAME, ORDINAL_POSITION;


PRINT '';
PRINT N'===== 4. SAO LƯU NỘI DUNG PROC HIỆN TẠI TRƯỚC KHI THAY =====';
PRINT N'Chạy từng lệnh sau và LƯU LẠI kết quả trước khi chạy script 04, 05, 07, 10:';
PRINT N'    EXEC sp_helptext ''dbo.p_Major_Subject_Get'';';
PRINT N'    EXEC sp_helptext ''dbo.p_Major_Subject_Save'';';
PRINT N'    EXEC sp_helptext ''dbo.p_Major_Subject_Delete'';';
PRINT N'    EXEC sp_helptext ''dbo.p_Major_SubjectViolation_Get'';';
PRINT N'    EXEC sp_helptext ''dbo.p_Major_SubjectViolation_GetBySubjectId'';';
PRINT N'    EXEC sp_helptext ''dbo.p_Major_SubjectViolation_Save'';';
