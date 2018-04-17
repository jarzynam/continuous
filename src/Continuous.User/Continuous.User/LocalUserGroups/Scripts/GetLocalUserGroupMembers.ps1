#  
# get local group members by name
#
param([string] $name)
  
$group = [ADSI]("WinNT://./$name, group");

return $group.psbase.invoke("members")  | ForEach {

  $_.GetType.Invoke().InvokeMember("Name", "GetProperty", $null, $_, $null)
}