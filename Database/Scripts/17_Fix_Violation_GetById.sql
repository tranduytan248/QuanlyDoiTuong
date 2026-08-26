/* =============================================================================
   17. SỬA LỖI MỞ SỬA LẦN VI PHẠM KHÔNG TÍCH SẴN HÀNH VI ĐÃ CHỌN
   -----------------------------------------------------------------------------
   TRIỆU CHỨNG: một lần vi phạm đã lưu 2-3 hành vi, nhưng khi bấm Sửa thì khung
   chọn hành vi trống trơn, "Đã chọn: 0 hành vi".

   NGUYÊN NHÂN: proc p_Major_SubjectViolation_GetById KHÔNG trả về cột
   BehaviorIds. Cac hanh vi duoc luu o bang noi Major_SubjectViolation_Behaviors
   nhung proc khong doc bang do. Model nhan BehaviorIds = NULL nen giao dien
   khong co gi de tich san.

   Day la loi co san tu truoc, khong phai do giao dien moi.

   CÁCH SỬA: bổ sung 2 cột lấy từ bảng nối:
       BehaviorIds   - danh sách id, phân tách bởi dấu phẩy (để tích sẵn checkbox)
       BehaviorNames - danh sách tên hành vi (để hiển thị)
       FieldNames    - danh sách lĩnh vực (để biết mở tab nào)
   ============================================================================= */

IF OBJECT_ID('dbo.p_Major_SubjectViolation_GetById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_GetById;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_GetById
    @ViolationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT v.ViolationId,
           v.SubjectId,
           s.FullName AS SubjectName,
           s.IdentityCardNumber,
           s.PhoneNumber,
           v.ViolationDate,
           v.TreatmentMeasures,
           v.RelatedDocuments,
           v.Images,
           v.Notes,
           v.ReporterName,
           v.ReporterUnit,
           v.ReporterPosition,
           v.ReporterPhone,
           v.ReporterUnionId,
           v.CreatedDate,
           v.CreatedBy,
           v.UpdatedDate,
           v.UpdatedBy,

           /* Danh sách id hành vi - dùng để tích sẵn checkbox trên giao diện sửa */
           STUFF((SELECT ',' + CAST(vb.BehaviorId AS VARCHAR(20))
                  FROM dbo.Major_SubjectViolation_Behaviors AS vb
                  WHERE vb.ViolationId = v.ViolationId
                  ORDER BY vb.BehaviorId
                  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS BehaviorIds,

           /* Tên các hành vi - dùng để hiển thị */
           STUFF((SELECT N', ' + b.BehaviorName
                  FROM dbo.Major_SubjectViolation_Behaviors AS vb
                  INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                  WHERE vb.ViolationId = v.ViolationId
                  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS BehaviorNames,

           /* Lĩnh vực của các hành vi - dùng để biết mở tab nào */
           STUFF((SELECT DISTINCT N', ' + f.FieldName
                  FROM dbo.Major_SubjectViolation_Behaviors AS vb
                  INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                  INNER JOIN dbo.Cate_Fields AS f ON f.FieldId = b.FieldId
                  WHERE vb.ViolationId = v.ViolationId
                  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS FieldNames

    FROM dbo.Major_SubjectViolations AS v
    INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = v.SubjectId
    WHERE v.ViolationId = @ViolationId
      AND ISNULL(v.IsDeleted, 0) = 0;
END
GO
