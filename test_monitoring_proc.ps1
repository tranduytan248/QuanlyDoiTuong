$connStr = "Data Source=10.57.30.10;Initial Catalog=quanlydoituong.cenit.vn;Persist Security Info=True;User Id=quanlydoituong;Password=Ek@n!N2@6VDfR;Connect Timeout=30;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
try {
    $conn.Open()
    Write-Host "Database connected!"
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandType = [System.Data.CommandType]::StoredProcedure
    $cmd.CommandText = "dbo.p_Major_Subject_GetMonitoringUnits"
    $cmd.Parameters.AddWithValue("@SubjectId", [Guid]"fd7e0be6-55f1-42f0-a6e9-7e8cf7576439") | Out-Null
    $cmd.Parameters.AddWithValue("@UserName", "trunglc.kha") | Out-Null
    
    $ad = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
    $dt = New-Object System.Data.DataTable
    $ad.Fill($dt) | Out-Null
    Write-Host "p_Major_Subject_GetMonitoringUnits SUCCESS! Returned $($dt.Rows.Count) rows."
    foreach ($col in $dt.Columns) {
        Write-Host "  Col: $($col.ColumnName) ($($col.DataType.Name))"
    }
    foreach ($r in $dt.Rows) {
        Write-Host "  Row: RecordTypeName=$($r['RecordTypeName']), UnitName=$($r['UnitName']), ReporterName=$($r['ReporterName']), RecordDate=$($r['RecordDate'])"
    }
} catch {
    Write-Host "Error running p_Major_Subject_GetMonitoringUnits: $_"
} finally {
    if ($conn.State -eq [System.Data.ConnectionState]::Open) {
        $conn.Close()
    }
}
