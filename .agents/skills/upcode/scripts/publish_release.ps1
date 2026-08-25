<#
.SYNOPSIS
    Đóng gói mã nguồn đã build vào thư mục trunk/Release để chuẩn bị deploy.

.DESCRIPTION
    KHÔNG dùng MSBuild Publish được: file .csproj của WebApp khai báo hơn 3600
    mục <Content> trỏ tới thư mục "Configs\Contents\..." không còn tồn tại,
    khiến tiến trình Publish dừng ngay ở file đầu tiên. Vì vậy script này đồng bộ
    trực tiếp từ kết quả build sang trunk/Release theo đúng cấu trúc sẵn có.

    Nội dung được đồng bộ:
      - bin\            : toàn bộ DLL/PDB từ WebApp\bin
      - Areas\          : Views (.cshtml/.js) của các module
      - Views\          : View dùng chung
      - Contents\       : tài nguyên tĩnh (css/js/ảnh)
      - App_Data\       : cấu hình stored procedure, message...
      - Libraries\      : thư viện ngoài
      - Global.asax, Web.config, ReportViewerWebForm.aspx

    KHÔNG đụng tới thư mục Uploads (dữ liệu người dùng tải lên).

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File .agents\skills\upcode\scripts\publish_release.ps1
    powershell -ExecutionPolicy Bypass -File .agents\skills\upcode\scripts\publish_release.ps1 -SkipBackup
#>

[CmdletBinding()]
param(
    [string]$WorkspaceRoot = "d:\SVN\QuanlyDoiTuong",
    [switch]$SkipBackup,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

$WebApp  = Join-Path $WorkspaceRoot "trunk\Source\CenIT.Solution.QLHD\CenIT.Solution.QLHD.WebApp"
$Release = Join-Path $WorkspaceRoot "trunk\Release"

if (-not (Test-Path $WebApp))  { Write-Error "Khong tim thay WebApp: $WebApp"; exit 1 }
if (-not (Test-Path $Release)) { Write-Error "Khong tim thay Release: $Release"; exit 1 }

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "        DONG GOI MA NGUON -> trunk\Release                " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Nguon : $WebApp"
Write-Host "Dich  : $Release"

# ---------------------------------------------------------------- Sao luu
if (-not $SkipBackup -and -not $DryRun) {
    $stamp  = Get-Date -Format "yyyyMMdd_HHmmss"
    $bkDir  = Join-Path $WorkspaceRoot "trunk\Release_backup_$stamp"
    Write-Host "`n[1/3] Sao luu Release hien tai -> $bkDir" -ForegroundColor Yellow
    # Chi sao luu bin + Areas + Views (phan se bi ghi de), bo qua Uploads cho nhanh
    New-Item -ItemType Directory -Path $bkDir -Force | Out-Null
    foreach ($d in @("bin", "Areas", "Views", "App_Data")) {
        $src = Join-Path $Release $d
        if (Test-Path $src) { Copy-Item -LiteralPath $src -Destination $bkDir -Recurse -Force }
    }
    Write-Host "      Da sao luu." -ForegroundColor Green
} else {
    Write-Host "`n[1/3] Bo qua sao luu." -ForegroundColor DarkGray
}

# ---------------------------------------------------------------- Dong bo
$robocopyArgs = @("/E", "/NFL", "/NDL", "/NJH", "/NJS", "/NP", "/R:2", "/W:2")
if ($DryRun) { $robocopyArgs += "/L" }

$copied = 0

function Sync-Folder {
    param([string]$Name, [string[]]$ExtraArgs = @())

    $src = Join-Path $script:WebApp $Name
    $dst = Join-Path $script:Release $Name
    if (-not (Test-Path $src)) {
        Write-Host ("      - {0,-14} (khong co trong nguon, bo qua)" -f $Name) -ForegroundColor DarkGray
        return
    }
    $all = $script:robocopyArgs + $ExtraArgs
    $null = & robocopy $src $dst @all
    $rc = $LASTEXITCODE
    # Robocopy: exit code < 8 la thanh cong
    if ($rc -ge 8) {
        Write-Host ("      - {0,-14} LOI (robocopy rc={1})" -f $Name, $rc) -ForegroundColor Red
        throw "Robocopy that bai o thu muc $Name"
    }
    Write-Host ("      - {0,-14} OK" -f $Name) -ForegroundColor Green
    $script:copied++
}

Write-Host "`n[2/3] Dong bo thu muc" -ForegroundColor Yellow
Sync-Folder -Name "bin"       -ExtraArgs @("/XF", "*.xml")
Sync-Folder -Name "Areas"     -ExtraArgs @("/XF", "*.cs", "*.csproj", "*.user")
Sync-Folder -Name "Views"     -ExtraArgs @("/XF", "*.cs")
Sync-Folder -Name "Contents"
Sync-Folder -Name "App_Data"
Sync-Folder -Name "Libraries"

# KHONG dong bo Web.config: file tren Release chua cau hinh moi truong that
# (chuoi ket noi, marker reload cua IIS). Neu can doi cau hinh thi sua truc tiep.
Write-Host "`n[3/3] Dong bo file goc (bo qua Web.config)" -ForegroundColor Yellow
foreach ($f in @("Global.asax", "ReportViewerWebForm.aspx")) {
    $src = Join-Path $WebApp $f
    $dst = Join-Path $Release $f
    if (Test-Path $src) {
        if (-not $DryRun) { Copy-Item -LiteralPath $src -Destination $dst -Force }
        Write-Host ("      - {0,-26} OK" -f $f) -ForegroundColor Green
    }
}

Write-Host "`n==========================================================" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host " CHAY THU (DryRun) - khong ghi file nao." -ForegroundColor Yellow
} else {
    Write-Host " HOAN TAT. Da dong bo $copied thu muc vao trunk\Release" -ForegroundColor Green
    Write-Host " Buoc tiep theo: chay deploy_ftp.ps1 de day len server." -ForegroundColor White
}
Write-Host "==========================================================" -ForegroundColor Cyan
