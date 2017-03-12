# 
# Remove users from local group 
#
param([string]$name, [string] $members)
  
net localgroup $name $members /delete