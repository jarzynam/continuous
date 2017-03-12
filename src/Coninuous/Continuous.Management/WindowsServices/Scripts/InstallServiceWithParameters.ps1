#
# Installing new Windows Service
#
param(
	[string] $serviceName, 
	[string] $fullServicePath,
	[string] $displayName,
	[string] $startName,
	[string] $startPassword,
	[uint8] $serviceType,
	[uint8] $errorControl,
	[string] $startMode,
	[boolean] $desktopInteract,
	[string] $loadOrderGroup,
	[string] $loadOrderGroupDependencies,
	[string] $serviceDependencies
)

$params = @{
	Name = $serviceName
	DisplayName = $displayName
	PathName  = $fullServicePath
	StartName = $startName
	StartPassword = $startPassword
	ServiceType = $serviceType
	ErrorControl = $errorControl
	StartMode = $startMode
	DesktopInteract = $desktopInteract
	LoadOrderGroup = $loadOrderGroup
	LoadOrderGroupDependencies = $loadOrderGroupDependencies
	ServiceDependencies = $serviceDependencies
}
Invoke-WmiMethod 
	  -Class win32_service
      -name create
      -ArgumentList $params
