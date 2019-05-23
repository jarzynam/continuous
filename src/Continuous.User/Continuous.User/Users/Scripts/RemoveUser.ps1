#  
# Remove local user by username
#
param(
	[string]$name,
	[bool]$deleteFolder
)
  
$computer = [ADSI]"WinNT://$Env:COMPUTERNAME,Computer"

$computer.delete("user", $name)
if($deleteFolder) {
	[array]$users = Get-WmiObject -ComputerName $env:COMPUTERNAME Win32_UserProfile -filter "LocalPath Like 'C:\\Users\\$($name)'" -ea stop
	if ($users.count -gt 0) {
		For ($i = 0; $i -lt $users.count; $i++) {
			$users[$i].Delete()
		}
	}
}