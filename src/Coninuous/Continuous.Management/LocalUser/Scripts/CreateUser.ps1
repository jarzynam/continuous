# Create new local user 
# Compability only with pwershell 5.1 or higher
#
param([string]$name, [string] $fullName, [string] $description, [string] $password)
  
New-LocalUser $name -Password $password -FullName $fullName -Description $description