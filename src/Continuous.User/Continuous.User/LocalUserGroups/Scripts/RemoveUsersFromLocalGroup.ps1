# 
# Remove users from local group 
#
param([string]$name, [string] $userName)
  
$group = [ADSI]("WinNT://./$name, group");

$group.Remove("WinNT://$Env:COMPUTERNAME/$userName,user");