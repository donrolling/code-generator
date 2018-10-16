Start-Process -FilePath "CodeGenerator.CLI.exe" -Wait

$postProcessScripts = (Get-Content 'config.json') -join "`n" | ConvertFrom-Json | Select -expand PostProcessScripts
foreach ($script in $postProcessScripts){ 
	$command = ".\Scripts\$script"
	Invoke-Expression "$command"
}

