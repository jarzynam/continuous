# 
# Add user to existing group by user name
#
param([string]$name, [string] $userName)

$group = [ADSI]("WinNT://$Env:COMPUTERNAME/$name,group");

$group.Add("WinNT://$Env:COMPUTERNAME/$userName,user");