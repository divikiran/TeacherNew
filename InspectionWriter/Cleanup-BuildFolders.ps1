function DeleteIfExists( $path )
{
    if( Test-Path $path ) 
    {
        Remove-Item $path -Recurse -Force
    }
    else
    {
        Write-Host "Path not found: $path" -ForegroundColor Yellow
    }
}

$paths = @(
    "./src/NPA.Xamarin.Common/",
    "./src/NPA.Xamarin.Common.Android/",
    "./src/NPA.Xamarin.Common.Android.Unity/",
    "./src/NPA.Xamarin.Common.iOS/",
    "./src/NPA.Xamarin.Common.iOS.Unity/",
    "./src/NPA.Xamarin.Module/",
    "./src/NPAInspectionWriter/",
    "./src/NPAInspectionWriter.Droid/",
    "./src/NPAInspectionWriter.iOS/"
)

foreach( $path in $paths )
{
    DeleteIfExists -path "$path/bin"
    DeleteIfExists -path "$path/obj"
}