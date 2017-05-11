#
# Execute custom command
#

param([string] $serviceName, [int32] $commandCode)

$service = Get-WmiObject -Class Win32_Service -Filter "Name = '$serviceName'"  -ErrorAction Stop

if($service){
	$service.UserControlService($commandCode)
}
else
{
	throw "$serviceName service not found";
}
 


 