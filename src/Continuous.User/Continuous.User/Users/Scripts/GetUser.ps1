#  
# get local user by username
#
param([string]$name)

$user = [ADSI]("WinNT://./$name, user");

$objUser = New-Object System.Security.Principal.NTAccount($name);
$strSID = $objUser.Translate([System.Security.Principal.SecurityIdentifier]).Value;

$user | Add-Member -MemberType NoteProperty -Name SecurityId -Value $strSID  -Force;

return $user;