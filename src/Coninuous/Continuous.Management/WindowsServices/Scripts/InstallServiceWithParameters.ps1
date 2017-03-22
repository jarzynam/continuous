#
# Installing new Windows Service
#
param(
	[string] $serviceName, 
	[string] $fullServicePath,
	[string] $displayName,
	[string] $startName,
	[string] $startPassword,
	[sbyte] $serviceType,
	[sbyte] $errorControl,
	[string] $startMode,
	[boolean] $desktopInteract
)

$params = $desktopInteract, $displayName,	$errorControl,	$null, 	$null, 	$serviceName,	$fullServicePath,	$null,     $serviceType,	$startMode,	$startName,	$startPassword 

Invoke-WmiMethod -Class "Win32_Service" -Name "Create" -ArgumentList $params
