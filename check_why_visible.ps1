$connStr = "Data Source=10.57.30.10;Initial Catalog=quanlydoituong.cenit.vn;Persist Security Info=True;User Id=quanlydoituong;Password=Ek@n!N2@6VDfR;Connect Timeout=30;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
try {
    $conn.Open()

    # 1. Check proc definition of p_Major_Subject_Get
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT OBJECT_DEFINITION(OBJECT_ID('p_Major_Subject_Get'))"
    $def = $cmd.ExecuteScalar()
    Write-Host "p_Major_Subject_Get definition length: $($def.Length)"

    # 2. Check what units trunglc.kha belongs to and manages
    $cmd2 = $conn.CreateCommand()
    $cmd2.CommandText = @"
    SELECT 'Cate_Unions_Members' AS Source, cum.UnionId, u.UnionName, u.BelongUnionName
    FROM Cate_Unions_Members cum
    INNER JOIN Cate_Unions u ON cum.UnionId = u.UnionId
    WHERE cum.UserName = 'trunglc.kha'
    UNION ALL
    SELECT 'Cate_Unions_Mangers' AS Source, cum2.UnionId, u2.UnionName, u2.BelongUnionName
    FROM Cate_Unions_Mangers cum2
    INNER JOIN Cate_Unions u2 ON cum2.UnionId = u2.UnionId
    WHERE cum2.Manager = 'trunglc.kha'
"@
    $ad2 = New-Object System.Data.SqlClient.SqlDataAdapter($cmd2)
    $dt2 = New-Object System.Data.DataTable
    $ad2.Fill($dt2) | Out-Null
    Write-Host "`nUnits associated with trunglc.kha:"
    foreach ($r in $dt2.Rows) {
        Write-Host "  [$($r['Source'])] UnionId=$($r['UnionId']), Name=$($r['UnionName']), Belong=$($r['BelongUnionName'])"
    }

    # 3. Check the subject '04353323475' (Nguyễn Văn A - Phường Nam Nha Trang)
    $cmd3 = $conn.CreateCommand()
    $cmd3.CommandText = @"
    SELECT s.SubjectId, s.IdentityCardNumber, s.FullName, s.CreatedBy, s.UnionId, u.UnionName, s.ManagingUnit, s.MonitoringUnits
    FROM Major_Subjects s
    LEFT JOIN Cate_Unions u ON s.UnionId = u.UnionId
    WHERE s.IdentityCardNumber = '04353323475'
"@
    $ad3 = New-Object System.Data.SqlClient.SqlDataAdapter($cmd3)
    $dt3 = New-Object System.Data.DataTable
    $ad3.Fill($dt3) | Out-Null
    Write-Host "`nSubject 04353323475 details:"
    foreach ($r in $dt3.Rows) {
        Write-Host "  SubjectId=$($r['SubjectId']), Name=$($r['FullName']), CreatedBy=$($r['CreatedBy']), UnionId=$($r['UnionId']), UnionName=$($r['UnionName']), ManagingUnit=$($r['ManagingUnit']), MonitoringUnits=$($r['MonitoringUnits'])"
    }

    # 4. Check violations for this subject
    $cmd4 = $conn.CreateCommand()
    $cmd4.CommandText = @"
    SELECT v.ViolationId, v.SubjectId, v.CreatedBy, v.ReportingUnit, v.ReportingDepartment
    FROM Major_SubjectViolations v
    WHERE v.SubjectId IN (SELECT SubjectId FROM Major_Subjects WHERE IdentityCardNumber = '04353323475')
"@
    $ad4 = New-Object System.Data.SqlClient.SqlDataAdapter($cmd4)
    $dt4 = New-Object System.Data.DataTable
    $ad4.Fill($dt4) | Out-Null
    Write-Host "`nViolations for Subject 04353323475:"
    foreach ($r in $dt4.Rows) {
        Write-Host "  ViolationId=$($r['ViolationId']), CreatedBy=$($r['CreatedBy']), Unit=$($r['ReportingUnit']), Dept=$($r['ReportingDepartment'])"
    }

    # 5. Check if trunglc.kha is configured as SuperAdmin
    $cmd5 = $conn.CreateCommand()
    $cmd5.CommandText = "SELECT * FROM Sys_Configs WHERE ConfigKey LIKE '%ADMIN%' OR ConfigKey LIKE '%PERMIT%'"
    $ad5 = New-Object System.Data.SqlClient.SqlDataAdapter($cmd5)
    $dt5 = New-Object System.Data.DataTable
    $ad5.Fill($dt5) | Out-Null
    Write-Host "`nSys_Configs admin/permit:"
    foreach ($r in $dt5.Rows) {
        Write-Host "  Key=$($r['ConfigKey']), Val=$($r['ConfigValue'])"
    }

} catch {
    Write-Host "Error: $_"
} finally {
    if ($conn.State -eq [System.Data.ConnectionState]::Open) {
        $conn.Close()
    }
}
