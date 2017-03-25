param([string]$serviceName)
  
$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";

return $service
