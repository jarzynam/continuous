param([string]$serviceName,[string]$newAccount,[string]$newPassword)
  
$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";

$service.Change($null,$null,$null,$null,$null,$null,$newAccount,$newPassword)
