@echo off
setlocal enabledelayedexpansion

REM Remove old publish directory
if exist publish (
    echo Deleting existing publish folder...
    rmdir /s /q publish
)

REM Define runtimes
set runtimes=win-x64 osx-x64 linux-x64

REM Publish each runtime
for %%r in (%runtimes%) do (
    echo Publishing for %%r ...
    dotnet publish -c Release -r %%r --self-contained true -p:PublishSingleFile=true -o publish/%%r
)

REM Create individual zips
for %%r in (%runtimes%) do (
    echo Zipping publish/%%r ...
    powershell -Command "Compress-Archive -Path publish/%%r\* -DestinationPath publish/%%r.zip -Force"
)

REM Zip all zips into one
echo Creating combined zip ...
powershell -Command "Compress-Archive -Path publish\*.zip -DestinationPath publish\all.zip -Force"

echo Done.
