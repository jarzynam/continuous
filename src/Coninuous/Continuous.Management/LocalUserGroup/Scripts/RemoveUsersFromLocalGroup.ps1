# 
# Remove users from local group 
#
param([string]$name, [string] $members)
  
net user $name $members /delete