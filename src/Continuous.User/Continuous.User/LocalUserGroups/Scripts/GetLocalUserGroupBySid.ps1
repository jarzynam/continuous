#  
# get local user group by SID
#
param([string]$sid)
  
Get-WMIObject -class Win32_Group -Filter "SID= '$sid' and LocalAccount= '$true'"