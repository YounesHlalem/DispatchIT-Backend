param 
(
    [Parameter(Mandatory)]
    [string] $filePath,
    [Parameter(Mandatory)]
    $replacementConfig,
    [Parameter()]
    [ValidateNotNullOrEmpty()]
    [switch]$base64Encode
)
 
    $ErrorActionPreference = "Stop"
    
    $configJson = ConvertFrom-Json -InputObject $replacementConfig

    $fileContents = Get-Content $filePath | Out-String

    #Check for tokens to be replaced
    $matches = [regex]::Matches($fileContents, '\@@(.*?)@@')
    Write-Host "$($matches.Count) found"
 

    foreach ($match in $matches) 
    {
        $tokenName = $match.Groups[1].Value
        Write-Host "Identified token $tokenName"

        # Get the value from the provided configuration
        $tokenLookup = ($configJson.Where{$_.token -eq $tokenName})

        if ($tokenLookup) 
        {
            $tokenValue = $tokenLookup.value

            if($tokenValue)
            {
                if($base64Encode.IsPresent)
                {
                    $tokenValue = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($tokenValue))
                }
                $fileContents = $fileContents -replace $match.Value,$tokenValue

                Write-Host "Replaced token $tokenName with value $tokenValue"
            }
        }

    }
    
    
    Set-Content -Path $filePath $fileContents