#Updated 20160107
#Any special instructions or cautions go here. For example, if the solution is deployed to a web app, I would note that here. 
#Path to .wsp file
$wspPath1 = "C:\temp\AEP.HQAMC.Branding.wsp"
#guids of features
$feat1 = ""
$feat2= ""

# name of .wsp file
$wspName1 = "AEP.HQAMC.Branding.wsp"

#web app to install wsp in
#$webApp = "https://vnext.aep.army.mil/"
$webApp = "https://portal.aepdev.com/"

Add-SPSolution –LiteralPath $wspPath1
$sln2 = get-spsolution -identity $wspName1
Install-SPSolution –Identity $wspName1 -GACDeployment -CompatibilityLevel {14,15} -force
echo "Started solution deployment..." 
while($sln2.JobExists) { 
echo " > Install in progress..."
start-sleep -s 10 
}
echo "Install complete"




