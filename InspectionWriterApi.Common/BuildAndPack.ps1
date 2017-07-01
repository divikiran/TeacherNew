New-Item -ItemType Directory -Force -Path .\Build

msbuild InspectionWriterWebApi.Common.sln /t:Clean
msbuild InspectionWriterWebApi.Common.sln /t:Build /p:Configuration=Release
Write-Debug "Build Complete"

nuget pack .\InspectionWriterWebApi.Common\InspectionWriterWebApi.Common.csproj -properties "Configuration=Release" -outputdirectory .\Build -noninteractive