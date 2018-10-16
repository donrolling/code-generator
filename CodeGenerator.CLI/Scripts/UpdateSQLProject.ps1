$destinationPath = "C:\Projects\IdentityServer\"

$path = ".\Output\Business\"
Copy-Item -Path $path -Destination $destinationPath -Recurse -Force

$path = ".\Output\Data\"
Copy-Item -Path $path -Destination $destinationPath -Recurse -Force

$server = "971JT039H2\DROLLING"
$database = "Identity"
$command = ".\Scripts\runSQL.ps1"
$path = ".\Output\Database\Drop"
Invoke-Expression "$command -server $server -database $database -path $path"

$path = ".\Output\Database\Functions"
Invoke-Expression "$command -server $server -database $database -path $path"

$path = '.\Database\Stored` Procedures'
Invoke-Expression "$command -server $server -database $database -path $path"

Read-Host -Prompt "Press Enter to exit"