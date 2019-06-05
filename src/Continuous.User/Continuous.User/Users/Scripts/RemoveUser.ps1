#  
# Remove local user by username
#
param(
	[string]$name,
	[bool]$deleteProfile
)
  
$computer = [ADSI]"WinNT://$Env:COMPUTERNAME,Computer"

$computer.delete("user", $name)

if($deleteProfile) {
	[array]$profiles = Get-WmiObject -ComputerName $env:COMPUTERNAME Win32_UserProfile -filter "LocalPath Like 'C:\\Users\\$($name)'" -ea stop
	if ($profiles.count -gt 0) {
		For ($i = 0; $i -lt $profiles.count; $i++) {
			$profiles[$i].Delete()
		}
	}
}