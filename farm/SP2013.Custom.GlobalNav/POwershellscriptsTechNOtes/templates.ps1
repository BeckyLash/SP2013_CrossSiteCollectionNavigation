#Updated 20160107
#Any special instructions or cautions go here. For example, if the solution is deployed to a web app, I would note that here.
if ((Get-PSSnapin "Microsoft.Sharepoint.PowerShell" -ErrorAction SilentlyContinue) -eq $Null)
{
Add-PSSnapin "Microsoft.Sharepoint.PowerShell"
} 
#Path to .wsp file
#$wspPath1 = "C:\solutions\AEP.HQAMC.Branding.wsp"
$wspPath1 = "C:\users\administrator\downloads\AEP.HQAMC.Branding.wsp"

# name of .wsp file
$wspName1 = "AEP.HQAMC.Branding.wsp"

# guids of features to install
$feat1 = get-spfeature "b26a1751-cb57-46ba-bb8b-5144888fd126"
$feat2 = get-spfeature "cd3773b0-c091-43aa-8ba2-208f2d7409d0"
$feat3 = get-spfeature "24d18d93-de0e-421f-9235-bbc73df55c80"
#feature pack id to add feature to
#$featurePackID = ""

#web app to install wsp in
#$webApp = "https://vnext.aep.army.mil/"
#$webApp = "https://hqamc2.aep.army.smil.mil/"
$webApp = "https://portal.aepdev.com/"

#Deactivate site scoped features for all site collections in a web app
#checks to see if feature is deactivated before attempting to deactivate
$webAppDeactivate = Get-SPWebApplication -Identity $webApp
$webAppDeactivate | Get-SPSite -limit all | ForEach-Object {	
if ($_.Features[$feat1.ID]) 
	{
	Disable-SPFeature $feat1 -Url $_.Url -Force -confirm:$false
	}
if ($_.Features[$feat2.ID]) 
	{
	Disable-SPFeature $feat2 -Url $_.Url -Force -confirm:$false
	}
if ($_.Features[$feat3.ID]) 
	{
	Disable-SPFeature $feat3 -Url $_.Url -Force -confirm:$false
	}

}


echo " "
echo "deactivating features"

echo "Deactivation complete..."
echo " "


echo "Uninstalling wsp"

$sln1 = get-spsolution -identity $wspName1
uninstall-spsolution -identity $wspName1 -confirm:$false
echo "Started solution retraction..." 
while($sln1.JobExists) { 
echo "Uninstall in progress..."
start-sleep -s 20 
}
echo "Uninstall complete..."
remove-spsolution -identity $wspName1 -confirm:$false
echo "Removed wsp"

Add-SPSolution –LiteralPath $wspPath1

$sln2 = get-spsolution -identity $wspName1
Install-SPSolution –Identity $wspName1 -GACDeployment -CompatibilityLevel {14,15} -force
echo "Started solution deployment..." 
while($sln2.JobExists) { 
echo "Install in progress..."
start-sleep -s 10 
}
echo "Install complete"

#remove # on next three lines if you need to assign to a feature pack 
#echo Assigning to feature pack
#Add-SPSiteSubscriptionFeaturePackMember -Identity $featurePackID -FeatureDefinition $feat1
#echo "Assigning to feature pack complete"

echo "Starting hidden feature activation"
$webs = get-spsite -webapplication $webApp
Foreach($oneWeb in $webs)
{
$feat1 = get-spfeature "b26a1751-cb57-46ba-bb8b-5144888fd126"
$feat2 = get-spfeature "cd3773b0-c091-43aa-8ba2-208f2d7409d0"
$feat3 = get-spfeature "24d18d93-de0e-421f-9235-bbc73df55c80"

write-host $oneWeb
$siteFeature1 = get-spfeature -site $oneWeb | Where {$_.ID -eq $feat1}
	if ($siteFeature1 -eq $null)
	{
	Write-Host "Activating site level features at $oneWeb" -foregroundcolor Yellow
	Enable-SPFEature -Identity $feat1 -URL $oneWeb.URL -confirm:$false

	}
	else
	{
	Write-Host "Feature $feature1 is already activated on $oneWeb" -foregroundcolor green
	}
$siteFeature2 = get-spfeature -site $oneWeb | Where {$_.ID -eq $feat2}
	if ($siteFeature2 -eq $null)
	{
	Write-Host "Activating site level features at $oneWeb" -foregroundcolor Yellow
	Enable-SPFEature -Identity $feat2 -URL $oneWeb.URL -confirm:$false

	}
	else
	{
	Write-Host "Feature $feature2 is already activated on $oneWeb" -foregroundcolor green
	}

$siteFeature3 = get-spfeature -site $oneWeb | Where {$_.ID -eq $feat3}
	if ($siteFeature3 -eq $null)
	{
	Write-Host "Activating site level features at $oneWeb" -foregroundcolor Yellow
	Enable-SPFEature -Identity $feat3 -URL $oneWeb.URL -confirm:$false

	}
	else
	{
	Write-Host "Feature $feature3 is already activated on $oneWeb" -foregroundcolor green
	}


}
