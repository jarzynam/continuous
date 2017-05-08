param([string]$serviceName, [string] $propertyName, [string] $propertyValue, [string] $propertyType)

$registryPath = "HKLM:\SYSTEM\CurrentControlSet\Services\$serviceName";

New-ItemProperty -Path $registryPath -Name $propertyName -Value $propertyValue -PropertyType $propertyType -Force 