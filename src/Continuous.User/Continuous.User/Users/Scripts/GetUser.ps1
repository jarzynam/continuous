#  
# get local user by username
#
param([string]$name)

[ADSI]("WinNT://./$name, user")