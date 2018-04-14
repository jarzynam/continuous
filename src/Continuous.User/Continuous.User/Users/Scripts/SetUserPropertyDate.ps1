#  
# change user date property by name
# not all properties are changable, so test your modifications
#
param([string]$name, [string] $propertyName, [datetime] $propertyValue)

$user = [ADSI]("WinNT://./$name, user");

$user.InvokeSet($propertyName, $propertyValue);
$user.CommitChanges();
