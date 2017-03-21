# what about compatibility level? i got errors after trying to add that parameter
#upgrade feature deploycomposedlook after update-spsolution
$webappurl = "http://wingtipserver"
$wspID = "aep.hqamc.branding.wsp"
$featID = "b26a1751-cb57-46ba-bb8b-5144888fd126"
$feat1ID = "bc4ebce0-8a4b-4cb4-a319-862e63108293"
$feat1Name = "AEP.HQAMC.Branding.SIPR_DeployComposedLookSiteNav"
$wspLiteralPath = "C:\solutions\aep.hqamc.branding.wsp"
update-spsolution -identity $wspID -literalpath $wspLiteralPath -gacdeployment
# start upgrade
$featureid = new-object system.guid -argumentlist $featID
$featureid2 = new-object system.guid -argumentlist $feat1ID
$webapp = get-spwebapplication $webappurl
# upgrade old feature
$features = $webapp.queryfeatures($featureid, $true)
foreach($feature in $features){$feature.upgrade($true)}
#install new feature
$wsp = get-spsolution $wspID
install-SPFeature -Path "AEP.HQAMC.Branding.SIPR_DeployComposedLookSiteNav"  

