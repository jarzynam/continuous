#  
# change user flag
#
param([string]$name, [int] $flag, [bool] $value)

$user = [ADSI]("WinNT://./$name, user")

if($value -eq $true)
{
	$user.UserFlags.Value = $user.UserFlags.Value -bor $flag;
}
else
{
	$user.UserFlags.Value = $user.UserFlags.Value -bxor $flag;
}

$user.SetInfo();