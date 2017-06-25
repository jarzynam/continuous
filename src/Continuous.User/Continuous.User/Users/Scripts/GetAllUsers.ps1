#  
# get all users from current domain
#

$users = [ADSI]("WinNT://$Env:USERDOMAIN");

$users.Children | where { $_.SchemaClassName -eq 'user'} 