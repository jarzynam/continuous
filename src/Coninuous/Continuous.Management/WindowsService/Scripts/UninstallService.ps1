#
# Uninstalling Windows Service with provided name
#

param([string] $serviceName)

$service = Get-WmiObject -Class Win32_Service -Filter "Name = '$serviceName'"  -ErrorAction Stop
 
$id = $service | select -expand ProcessId;
 
if($id)
{
	( Get-Process -Id $id).Kill()
}
 
if($service)
{
     $result = $service.delete()

	if($result.ReturnValue -notlike 0)
	{
		throw "Cannot remove serivce. StatusCode: "+ $result.ReturnValue
	}	
} 
else
{
	throw "$serviceName service not found";
}
 