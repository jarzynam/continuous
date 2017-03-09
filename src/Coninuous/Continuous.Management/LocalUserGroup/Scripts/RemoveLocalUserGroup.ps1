#  
# Remove local user group by name
#
param([string]$name)
  
net localgroup $name /delete