param([string]$serviceName)
  
$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";

if($service){
	$registryPath = "HKLM:\SYSTEM\CurrentControlSet\Services\$serviceName";

	$isDelayed = (Get-Item -Path $registryPath).GetValue("DelayedAutostart"); 
	$service.DelayedAutoStart = $isDelayed
}
return $service
