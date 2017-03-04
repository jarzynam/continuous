#
# Installing new Windows Service
#
param(
	[string] $serviceName, 
	[string] $fullServicePath
)

New-Service -Name $serviceName -BinaryPathName $fullServicePath