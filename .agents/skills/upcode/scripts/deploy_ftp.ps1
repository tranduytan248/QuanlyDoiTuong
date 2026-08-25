<#
.SYNOPSIS
    Deploy / Upload pre-compiled source files (trunk/Release) to the FTP Server.
#>

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string]$SourcePath = "trunk/Release",

    [Parameter()]
    [string]$FtpHost = "",

    [Parameter()]
    [string]$FtpUser = "",

    [Parameter()]
    [string]$FtpPass = "",

    [Parameter()]
    [string]$SubFolder = "",

    [Parameter()]
    [switch]$DryRun,

    [Parameter()]
    [switch]$Force
)

# 1. Resolve FTP Host, User, Pass
if ([string]::IsNullOrWhiteSpace($FtpHost)) {
    if ($env:FTP_HOST) { $FtpHost = $env:FTP_HOST }
    elseif ($env:HOST_FTP) { $FtpHost = $env:HOST_FTP }
    elseif ($env:SERVER_FTP) { $FtpHost = $env:SERVER_FTP }
    else { $FtpHost = "10.57.30.10" }
}

if ([string]::IsNullOrWhiteSpace($FtpUser)) {
    if ($env:FTP_USER) { $FtpUser = $env:FTP_USER }
    elseif ($env:USER_FTP) { $FtpUser = $env:USER_FTP }
    elseif ($env:USERNAME_FTP) { $FtpUser = $env:USERNAME_FTP }
    else { $FtpUser = "quanlydoituong" }
}

if ([string]::IsNullOrWhiteSpace($FtpPass)) {
    if ($env:FTP_PASS) { $FtpPass = $env:FTP_PASS }
    elseif ($env:PASS_FTP) { $FtpPass = $env:PASS_FTP }
    elseif ($env:PASSWORD_FTP) { $FtpPass = $env:PASSWORD_FTP }
    else { $FtpPass = "Ek@n!N2@(VDft" }
}

if (-not $FtpHost.StartsWith("ftp://", [System.StringComparison]::InvariantCultureIgnoreCase)) {
    $FtpHost = "ftp://$FtpHost"
}
$FtpHost = $FtpHost.TrimEnd('/')

# 2. Resolve Source Directory
$WorkspaceRoot = "d:\SVN\QuanlyDoiTuong"
$ResolvedSource = Join-Path $WorkspaceRoot $SourcePath

if (-not (Test-Path $ResolvedSource)) {
    $AltSource = Join-Path $WorkspaceRoot "trunk/Publish"
    if (Test-Path $AltSource -and (Get-ChildItem $AltSource).Count -gt 0) {
        $ResolvedSource = $AltSource
    } else {
        Write-Error "Source directory not found: $ResolvedSource"
        return
    }
}

if (-not [string]::IsNullOrWhiteSpace($SubFolder)) {
    $TargetFolder = Join-Path $ResolvedSource $SubFolder
    if (-not (Test-Path $TargetFolder)) {
        Write-Error "Subfolder '$SubFolder' not found under '$ResolvedSource'"
        return
    }
    $ItemsToUpload = Get-ChildItem -Path $TargetFolder -Recurse -File
    $BaseDir = $ResolvedSource
} else {
    $ItemsToUpload = Get-ChildItem -Path $ResolvedSource -Recurse -File
    $BaseDir = $ResolvedSource
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "             UPCODE / FTP DEPLOYMENT TOOL                 " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "FTP Server    : $FtpHost" -ForegroundColor White
Write-Host "FTP User      : $FtpUser" -ForegroundColor White
Write-Host "Local Source  : $BaseDir" -ForegroundColor White
if (-not [string]::IsNullOrWhiteSpace($SubFolder)) {
    Write-Host "Filter Folder : $SubFolder" -ForegroundColor White
}
Write-Host "Total Files   : $($ItemsToUpload.Count)" -ForegroundColor White
Write-Host "DryRun Mode   : $(if ($DryRun) { 'YES (No files will be uploaded)' } else { 'NO (Uploading to FTP)' })" -ForegroundColor $(if ($DryRun) { 'Yellow' } else { 'Green' })
Write-Host "==========================================================" -ForegroundColor Cyan

if ($ItemsToUpload.Count -eq 0) {
    Write-Host "Khong co file nao de upload." -ForegroundColor Yellow
    return
}

$CreatedDirs = @{}

function Ensure-FtpDirectory {
    param(
        [string]$RemoteDirPath,
        [string]$HostUrl,
        [System.Net.NetworkCredential]$Credentials
    )

    if ([string]::IsNullOrWhiteSpace($RemoteDirPath) -or $RemoteDirPath -eq "/" -or $CreatedDirs.ContainsKey($RemoteDirPath)) {
        return
    }

    $segments = $RemoteDirPath.Trim('/').Split('/')
    $currentPath = ""

    foreach ($segment in $segments) {
        if ([string]::IsNullOrWhiteSpace($segment)) { continue }
        $currentPath += "/$segment"
        if ($CreatedDirs.ContainsKey($currentPath)) { continue }

        $dirUri = "$HostUrl$currentPath"
        try {
            $req = [System.Net.FtpWebRequest]::Create($dirUri)
            $req.Credentials = $Credentials
            $req.Method = [System.Net.WebRequestMethods+Ftp]::MakeDirectory
            $req.UsePassive = $true
            $req.KeepAlive = $false
            $resp = $req.GetResponse()
            $resp.Close()
        } catch {
        }
        $CreatedDirs[$currentPath] = $true
    }
}

$Credentials = New-Object System.Net.NetworkCredential($FtpUser, $FtpPass)
$Stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$SuccessCount = 0
$ErrorCount = 0
$TotalBytes = 0

$index = 0
foreach ($file in $ItemsToUpload) {
    $index++
    $relPath = $file.FullName.Substring($BaseDir.Length).TrimStart('\', '/')
    $remotePath = "/" + $relPath.Replace('\', '/')
    $remoteUri = "$FtpHost$remotePath"
    $remoteDir = [System.IO.Path]::GetDirectoryName($remotePath).Replace('\', '/')

    $percent = [math]::Round(($index / $ItemsToUpload.Count) * 100, 1)
    $sizeKb = [math]::Round($file.Length / 1KB, 1)

    if ($DryRun) {
        Write-Host "[$index/$($ItemsToUpload.Count)] ($percent%) [DRY-RUN] Would upload: $relPath ($sizeKb KB)" -ForegroundColor Gray
        $SuccessCount++
        $TotalBytes += $file.Length
        continue
    }

    try {
        if (-not [string]::IsNullOrWhiteSpace($remoteDir) -and $remoteDir -ne "/") {
            Ensure-FtpDirectory -RemoteDirPath $remoteDir -HostUrl $FtpHost -Credentials $Credentials
        }

        $uploadReq = [System.Net.FtpWebRequest]::Create($remoteUri)
        $uploadReq.Credentials = $Credentials
        $uploadReq.Method = [System.Net.WebRequestMethods+Ftp]::UploadFile
        $uploadReq.UseBinary = $true
        $uploadReq.UsePassive = $true
        $uploadReq.KeepAlive = $false

        $fileStream = [System.IO.File]::OpenRead($file.FullName)
        $reqStream = $uploadReq.GetRequestStream()

        $buffer = New-Object byte[] 65536
        while (($read = $fileStream.Read($buffer, 0, $buffer.Length)) -gt 0) {
            $reqStream.Write($buffer, 0, $read)
        }

        $fileStream.Close()
        $reqStream.Close()

        $uploadResp = $uploadReq.GetResponse()
        $uploadResp.Close()

        Write-Host "[$index/$($ItemsToUpload.Count)] ($percent%) OK: $relPath ($sizeKb KB)" -ForegroundColor Green
        $SuccessCount++
        $TotalBytes += $file.Length
    } catch {
        Write-Host "[$index/$($ItemsToUpload.Count)] ($percent%) FAILED: $relPath - $($_.Exception.Message)" -ForegroundColor Red
        $ErrorCount++
    }
}

$Stopwatch.Stop()
$TotalMb = [math]::Round($TotalBytes / 1MB, 2)
$ElapsedSec = [math]::Round($Stopwatch.Elapsed.TotalSeconds, 1)

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "                DEPLOYMENT SUMMARY                        " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Thanh cong : $SuccessCount files ($TotalMb MB)" -ForegroundColor Green
if ($ErrorCount -gt 0) {
    Write-Host "That bai   : $ErrorCount files" -ForegroundColor Red
}
Write-Host "Thoi gian  : $ElapsedSec giay" -ForegroundColor White
Write-Host "==========================================================" -ForegroundColor Cyan
