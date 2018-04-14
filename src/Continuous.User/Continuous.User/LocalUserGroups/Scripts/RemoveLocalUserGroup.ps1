#  
# Remove local user group by name
#
param([string]$name)
 
$computer = [ADSI]"WinNT://$Env:COMPUTERNAME,Computer"

$computer.delete("group", $name)