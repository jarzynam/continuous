# 
# Change user password
#
param([string]$userName, [string] $password)
  
 net user $userName $password