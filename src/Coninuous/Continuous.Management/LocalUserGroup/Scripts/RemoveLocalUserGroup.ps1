#  
# Remove local user group by name
#
param([string]$name)
  
net user $name /delete