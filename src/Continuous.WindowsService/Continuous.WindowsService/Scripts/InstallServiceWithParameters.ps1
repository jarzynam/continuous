#
# Installing new Windows Service
#
# Create methods parameters:
#DesktopInteract                Boolean                              
#DisplayName                     String                              
#ErrorControl                     UInt8                              
#LoadOrderGroup                  String                              
#LoadOrderGroupDependencies StringArray                              
#Name                            String                              
#PathName                        String                              
#ServiceDependencies        StringArray                              
#ServiceType                      UInt8                      
#StartMode                       String                   
#StartName                       String                              
#StartPassword                   String        


param(
	[string] $serviceName, 
	[string] $fullServicePath,
	$displayName,
	$startName,
	$startPassword,
	[byte] $serviceType,
	[byte] $errorControl,
	[string] $startMode,
	[boolean] $desktopInteract,
	[string[]] $serviceDependencies
)


 
$params = $desktopInteract, $displayName, $errorControl, $null, $null, $serviceName, $fullServicePath, $serviceDependencies, $serviceType, $startMode, $startName, $startPassword


Invoke-WmiMethod -Class "Win32_Service" -Name "Create" -ArgumentList $params 
