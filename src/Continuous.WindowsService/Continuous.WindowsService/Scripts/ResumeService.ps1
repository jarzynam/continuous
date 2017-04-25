#
# Pause running Windows Service with provided name
#

param([string] $serviceName)

$service = Get-WmiObject -Class Win32_Service -Filter "Name = '$serviceName'"  -ErrorAction Stop

if($service){
	$service.ResumeService();
}
else
{
	throw "$serviceName service not found";
}
 


 