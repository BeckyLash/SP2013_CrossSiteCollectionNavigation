#creates site collection based on $site and then creates subsites
$Owner = "wingtip\administrator"
$WebAppURL = "http://aep-dev-vm:28652/"
$Departments= Get-Content "C:\solutions\sitelist.txt"
$Site = New-SPSite ($WebAppURL + "Sites/" + "test10") -Template STS#0 -OwnerAlias $Owner
$Site.RootWeb.CreateDefaultAssociatedGroups($Owner,$null,$Site.Title)
Foreach ($Department in $Departments) {$Web = New-SPWeb -Url (($Site.URL) + "/" + $Department.replace(" ","")) -Name $Department -Template STS#0 -UniquePermissions;$Web.CreateDefaultAssociatedGroups($Owner,$null,$Web.Title)}