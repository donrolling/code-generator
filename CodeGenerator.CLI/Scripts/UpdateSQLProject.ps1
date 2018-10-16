$destinationPath = "C:\Projects\IdentityServer\Tools\IdentityCodeGenerator\Output"

$path = ".\Business\"
Copy-Item -Path $path -Destination $destinationPath -Recurse -Force

$path = ".\Data\"
Copy-Item -Path $path -Destination $destinationPath -Recurse -Force

$server = "971JT039H2\DROLLING"
$database = "Indentity"
$command = ".\runSQL.ps1"
$path = ".\Database\Drop"
Invoke-Expression "$command -server $server -database $database -path $path"

$path = ".\Database\Functions"
Invoke-Expression "$command -server $server -database $database -path $path"

$path = '.\Database\Stored` Procedures'
Invoke-Expression "$command -server $server -database $database -path $path"

Read-Host -Prompt "Press Enter to exit"