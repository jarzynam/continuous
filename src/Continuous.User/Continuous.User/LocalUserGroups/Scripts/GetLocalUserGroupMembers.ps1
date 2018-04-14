#  
# get local user group by name
#
param([string]$name)
  
$group = [ADSI]("WinNT://./$name, group");

return $group.psbase.invoke("members")  | ForEach {

  $_.GetType().InvokeMember("Name","GetProperty",$Null,$_,$Null)
}