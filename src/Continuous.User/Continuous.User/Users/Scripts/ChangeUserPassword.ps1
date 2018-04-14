# 
# Change user password
#
param([string]$userName, [string] $password)

$user = [ADSI]("WinNT://./$userName, user");
$user.SetPassword($password)
$user.SetInfo()
