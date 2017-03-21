#Updated 20160302
#Any special instructions or cautions go here. For example, if the solution is deployed to a web app, I would note that here. 
#Path to .wsp file
$wspPath1 = "D:\solutions\AEP.HQAMC.Branding.wsp"

# name of .wsp file
$wspName1 = "AEP.HQAMC.Branding.wsp"

#web app to install wsp in
#$webApp = "https://vnext.aep.army.mil/"
$webApp = "https://portal.aepdev.com/"
$sln2 = get-spsolution -identity $wspName1
Update-SPSolution –Identity $wspName1 -LiteralPath $wspPath1 -GACDeployment -force
echo "Started solution deployment..." 
while($sln2.JobExists) { 
echo " > Update in progress..."
start-sleep -s 10 
}
echo "Install complete"




