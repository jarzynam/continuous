#  
# get logged username
#
param()

(Get-WMIObject -class Win32_ComputerSystem).username