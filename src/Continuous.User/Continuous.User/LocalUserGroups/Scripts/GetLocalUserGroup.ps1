#  
# get local user group by name
#
param([string]$name)
  
$group = [ADSI]("WinNT://./$name, group");


return $group