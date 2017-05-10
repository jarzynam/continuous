param([string]$serviceName)
  
$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";

if($service)
{	
	$registry = Get-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\$serviceName";

	$service.DelayedAutoStart = $registry.GetValue("DelayedAutostart");

	$serviceDependencies = $registry.GetValue("DependOnService");
	$service | Add-Member ServiceDependencies $serviceDependencies;
}

return $service
