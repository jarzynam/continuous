# 
# Add users to existing user name
#
param([string]$name, [string] $members)
  
net localgroup $name $members /add