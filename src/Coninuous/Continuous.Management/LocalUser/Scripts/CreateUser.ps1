# 
# Create new local user by username
#
param([string]$name, [string] $fullName, [string] $description, [string] $password, [string] $expires)
  
net user $name $password /add /fullname:"$fullName" /comment:"$description" /expires:"$expires"