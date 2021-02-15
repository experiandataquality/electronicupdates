
# metadatawebapi.ps1
# Powershell script to download data from the Experian Data Quality Electronic Updates Web API.

# Declare credentials to communicate with the service.
# Override any values hard-coded here by named environment variable.
$token = "x-api-key " + $env:EDQ_ElectronicUpdates_Token

# Service endpoint
$endpoint = 'https://ws.updates.qas.com/metadata/v2/'

# Declare User Agent string
$version = $PSVersionTable
$userAgent = 'Powershell/{0}' -f $PSVersionTable.PSVersion.ToString()

# Declare HTTP request headers
$headers = @{'accept'= 'application/json'; 'content-type'= 'application/json; charset=UTF-8'; 'UserAgent'= $userAgent; 'Authorization'= $token}

# Declare directory to download data to
$root_download_path = Join-Path -Path '.' -ChildPath 'EDQData'

# Get the available package groups from the Web API
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$packages_request = Invoke-RestMethod -Uri ($endpoint + 'packages') -Headers $headers -Method Get
if(!$?)
{
    Write-Output 'Available packages request failed'
    Throw $Error[0]
}

$package_groups = @($packages_request)

# Iterate through the package groups
for($i = 0; $i -lt $package_groups.Count; $i++)
{
    # Get the code, vintage and packages in the group
    $package_group = $package_groups[$i]
    $code = $package_group.PackageGroupCode
    $vintage = $package_group.Vintage
    $packages = @($package_group.Packages)

    Write-Output ('Package: ''{0:<6}''; Vintage: ''{1}''' -f ($code, $vintage))

    $directory_path = Join-Path -Path $root_download_path -ChildPath "$code\$vintage"

    # Create a directory to download the files for this package into
    if(!(Test-Path -Path $directory_path -PathType Container))
    {
        New-Item -Path $directory_path -ItemType Directory
    }

    # Iterate through the packages
    for($j = 0; $j -lt $packages.Count; $j++)
    {
        # Get the available files
        $package = $packages[$j]
        $data_files = @($package.Files)

        # Iterate through the files
        for($k = 0; $k -lt $data_files.Count; $k++)
        {
            $data_file = $data_files[$k]
            $file_name = $data_file.Filename
            $file_hash = $data_file.Md5Hash
            $file_size = [int]$data_file.Size
            $file_path = Join-Path -Path $directory_path -ChildPath $file_name

            $download_file = $True

            # Has the file already been downloaded?
            if(Test-Path -Path $file_path -PathType Leaf)
            {
                $data_file_item = Get-Item -Path $file_path
                $size_on_disk = $data_file_item.Length

                # Does the size of the file match that already on disk?
                if($size_on_disk -ne $file_size)
                {
                    Write-Output ('File ''{0}'' has already been downloaded, but is corrupt due to truncation.' -f ($file_path))
                    Remove-Item -Path $file_path
                }
                else
                {
                    # Verify the MD5 hash of the file on disk.
                    # In a fuller solution, the MD5 hashes of the files
                    # that have been downloaded would be cached to disk
                    # and corruption would not be tested every time.
                    $hash_on_disk = (Get-FileHash -Path $file_path -Algorithm MD5).Hash

                    # If the hash does not match, the file needs to be downloaded again
                    if($file_hash -ieq $hash_on_disk)
                    {
                        Write-Output ('File ''{0}'' has already been downloaded.' -f ($file_path))
                        $download_file = $False
                    }
                    else
                    {
                        Write-Output ('File ''{0}'' has already been downloaded, but is corrupt.' -f ($file_path))
                        Remove-Item -Path $file_path
                    }
                }
            }

            if($download_file)
            {
                Write-Output ('Requesting download URI for file ''{0}''.' -f ($file_path))

                # Request the download URI for this file from the Web API
                $request = @{'FileName'= $file_name; 'FileMd5Hash'= $file_hash}
                try
                {
                    $download_uri_request = Invoke-RestMethod -Method Post -Uri ($endpoint + 'filelink') -Headers $headers -Body ($request | ConvertTo-Json)
                }
                catch
                {
                    Write-Output ('Download URI request failed.')
                    Throw $Error[0]
                }

                $download_uri = $download_uri_request.DownloadUri

                # Download the file from the returned download URI
                Write-Output ('Downloading ''{0}''...' -f ($download_uri))
                
                Invoke-RestMethod -Method Get -Uri $download_uri -OutFile $file_path
                Write-Output ('Downloaded file ''{0}''.' -f ($file_path))
            }
        }
    }
}

Write-Output ('All data files up-to-date.')
