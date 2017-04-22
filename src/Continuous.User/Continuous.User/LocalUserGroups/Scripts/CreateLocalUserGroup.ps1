# 
# Create new local user gorup
#
param([string]$name, [string] $description)
  
net localgroup $name /add /comment:"$description"