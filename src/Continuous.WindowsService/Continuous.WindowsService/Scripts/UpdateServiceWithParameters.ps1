#
# Update existing Windows Service
#
# Update methods parameters:
#	String		DisplayName,
#	String		PathName, 
#	Byte		ServiceType,
#	Byte		ErrorControl,
#	String		StartMode, 
#	Boolean		DesktopInteract,


param(
	[string] $serviceName, 
	$fullServicePath,
	$displayName,
	$startName,
	$startPassword,
	$serviceType,
	$errorControl,
	$startMode,
	$desktopInteract
)

$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";

$service.Change($displayName, $fullServicePath, $serviceType, $errorControl, $startMode, $desktopInteract)

$startName
$startPassword