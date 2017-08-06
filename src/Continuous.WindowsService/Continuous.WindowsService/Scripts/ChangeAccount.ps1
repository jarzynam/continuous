param([string]$serviceName, [string] $newAccount,[string]$newPassword, [string]$domain)
    
Add-Type -AssemblyName System.DirectoryServices.AccountManagement

$DS = New-Object System.DirectoryServices.AccountManagement.PrincipalContext('machine',$env:COMPUTERNAME)
if($DS.ValidateCredentials($newAccount, $newPassword) -eq $false){
	# simulate wmi-object invalid account result
	$v = New-Object -TypeName PSObject;
	$v | Add-Member -MemberType NoteProperty -Name ReturnValue -Value 22
	return $v;
}
      

$service = Get-WMIObject -Class Win32_Service -Filter "Name = '$serviceName'";
$fullAccount = $domain + '\' + $newAccount;

$service.Change($null,$null,$null,$null,$null,$null,$fullAccount,$newPassword)
