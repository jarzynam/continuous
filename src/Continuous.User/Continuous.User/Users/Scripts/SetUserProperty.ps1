#  
# change user property by name
# not all properties are changable, so test your modificaitons
#
param([string]$name, [string] $propertyName, $propertyValue)

$user = [ADSI]("WinNT://./$name, user");

$user.InvokeSet($propertyName, $propertyValue);
$user.CommitChanges();
