#Updated 20160107
#Any special instructions or cautions go here. For example, if the solution is deployed to a web app, I would note that here.
if ((Get-PSSnapin "Microsoft.Sharepoint.PowerShell" -ErrorAction SilentlyContinue) -eq $Null)
{
Add-PSSnapin "Microsoft.Sharepoint.PowerShell"
} 
#Path to .wsp file
#$wspPath1 = "C:\solutions\AEP.HQAMC.Branding.wsp"
$wspPath1 = "C:\temp\AEP.HQAMC.Branding.wsp"

# name of .wsp file
$wspName1 = "AEP.HQAMC.Branding.wsp"

# guids of features to install
#feat1 scoped to web app level, other features don't need activating
$feat1 = "f7719f61-fbb6-4e06-8806-3a387e706b52"
#$feat2 = get-spfeature "a32b6912-77e8-459d-ad50-e387c465320f"
#$feat3 = get-spfeature "b26a1751-cb57-46ba-bb8b-5144888fd126"
#feature pack id to add feature to
#$featurePackID = ""

#web app to install wsp in
#$webApp = "https://vnext.aep.army.mil/"
#$webApp = "https://hqamc2.aep.army.smil.mil/"
$webApp = "http://portal.aepdev.com"

#Deactivate site scoped features for all site collections in a web app
#checks to see if feature is deactivated before attempting to deactivate
#$webAppDeactivate = Get-SPWebApplication -Identity $webApp
#$webAppDeactivate | Get-SPSite -limit all | ForEach-Object {	
#if ($_.Features[$feat1.ID]) 
#	{
#	Disable-SPFeature $feat1 -Url $_.Url -Force -confirm:$false
#	}
#
#}


#echo " "
#echo "deactivating features"

#echo "Deactivation complete..."
#echo " "


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
Write-Host "Activating web application scoped feature at $webApp" -foregroundcolor Yellow
Enable-SPFEature -Identity $feat1 -Url $webApp -confirm:$false

