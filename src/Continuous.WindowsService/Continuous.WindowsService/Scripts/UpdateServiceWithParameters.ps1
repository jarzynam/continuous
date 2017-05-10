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
#   string		StartName,
#	String		StartPassword,
#	String		LoadOrderGroup,
#	String[]	LoadOrderGroupDependencies
#	String[]	ServiceDependencies


param(
	[String] $serviceName, 
	$fullServicePath,
	$displayName,
	$startName,
	$startPassword,
	$serviceType,
	$errorControl,
	$startMode,
	$desktopInteract,
	$serviceDependencies
)

$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";

$service.Change($displayName, $fullServicePath, $serviceType, $errorControl, $startMode, $desktopInteract, $null, $null, $null, $null, $serviceDependencies)
