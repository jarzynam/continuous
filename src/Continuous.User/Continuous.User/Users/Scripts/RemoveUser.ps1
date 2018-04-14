#  
# Remove local user by username
#
param([string]$name)
  
$computer = [ADSI]"WinNT://$Env:COMPUTERNAME,Computer"

$computer.delete("user", $name)