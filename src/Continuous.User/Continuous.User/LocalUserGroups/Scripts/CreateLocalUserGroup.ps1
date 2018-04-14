# 
# Create new local user gorup
#
param([string]$name, [string] $description)

$computer = [ADSI]"WinNT://$Env:COMPUTERNAME,Computer"

$group = $computer.Create("Group", $name)
$group.SetInfo()

$group.Description = $description;
$group.SetInfo()
  