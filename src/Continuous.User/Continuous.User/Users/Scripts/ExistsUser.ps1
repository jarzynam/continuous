#  
# check if user exists
#
param([string]$userName)

try
{
	$p = ([ADSI]("WinNT://./$userName, user"))


    return $p.Guid -ne $null
   
}
catch
{
	return $false
}