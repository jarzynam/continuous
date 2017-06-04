# 
# Change user password
#
param([string]$userName, [string] $password)
  
 $user = [ADSI]"WinNT://./$username";
 $user.SetPassword($password)