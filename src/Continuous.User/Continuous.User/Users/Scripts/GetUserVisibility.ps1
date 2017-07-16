#  
# get local user by username
#
param([string]$name)

$path = "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList"

if((Test-Path $path) -eq $false)
{
	return $true
}

return (Get-ItemProperty -Path $path -Name $name -ErrorAction Ignore) -eq $null