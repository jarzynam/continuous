$services = Get-WMIObject -Class Win32_Service;


foreach($item in $services){
	$serviceName = $item.Name;

	$registry = Get-Item -Path "HKLM:\SYSTEM\CurrentControlSet\Services\$serviceName";

	$item.DelayedAutoStart = $registry.GetValue("DelayedAutostart");

	$serviceDependencies = $registry.GetValue("DependOnService");
	$item | Add-Member ServiceDependencies $serviceDependencies;
}

return $services
