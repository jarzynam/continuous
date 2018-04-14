# 
# Create new local user by username
#
param([string]$name, [string] $fullName, [string] $description, [string] $password)

$computer = [ADSI]"WinNT://$Env:COMPUTERNAME,Computer"

$user = $Computer.Create("User", $name)

$user.SetPassword($password)
$user.SetInfo()

$user.FullName = $fullName
$user.Description = $description
$user.SetInfo()
