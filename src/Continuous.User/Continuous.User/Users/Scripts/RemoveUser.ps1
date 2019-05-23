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
	Remove-Item "C:\Users\$($name)" -Recurse -Force -Verbose
}