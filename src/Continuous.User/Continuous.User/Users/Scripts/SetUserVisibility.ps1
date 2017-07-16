# 
# Set user visibility
#
param([string]$name, [bool] $isVisible)
  
$path = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList'

if((Test-Path $path) -eq $false)
{
	New-Item $path -Force
}

if($isVisible -eq $false)
{	
	 New-ItemProperty -Path $path -Name $name -Value 0 -PropertyType DWord -Force

} else {
	Remove-ItemProperty $path -Name $name -Force -ErrorAction Ignore
}