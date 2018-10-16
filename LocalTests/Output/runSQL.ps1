param([string]$server, [string]$database, [string]$path)
#$outputPath = "$path\OUTPUT\"
foreach ($f in Get-ChildItem -path $path -Filter *.sql){ 
    #$out = $outputPath + $f.name.split(".")[0] + ".txt";
    try {
        invoke-sqlcmd -ServerInstance $server -Database $database -InputFile $f.fullname 
    } catch {
        Write-Host "SQL Exception";
        $Error | format-list -force;
    }
    #| format-table | out-file -filePath $out
}