#  
# get local user visiblity in windows welcome screen flag
#
param([string]$name)

$path = "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList"

if((Test-Path $path) -eq $false)
{
	return $true
}

return (Get-Item -Path $path).GetValue($name) -eq $null