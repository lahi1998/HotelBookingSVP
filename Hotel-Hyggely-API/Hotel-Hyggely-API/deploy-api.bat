@echo off
REM =====================================
REM Hotel Hyggely API Deployment Script (Fixed)
REM =====================================

REM Server configuration
SET SERVER_USER=root
SET SERVER_HOST=157.180.73.14
SET REMOTE_PATH=/var/www/hotel-hyggely-api
SET SSH_KEY=%USERPROFILE%\.ssh\id_ed25519
SET SERVICE_NAME=hotel-hyggely-api

REM Local project configuration
SET SCRIPT_DIR=%~dp0
SET PROJECT_DIR=%SCRIPT_DIR%
SET PROJECT_FILE=%PROJECT_DIR%Hotel-Hyggely-API.csproj
SET PUBLISH_DIR=%PROJECT_DIR%\..\..\..\publish


echo =====================================
echo Building and deploying Hotel Hyggely API
echo =====================================

REM Step 1: Publish API locally
echo 📦 Publishing API project...
dotnet publish "%PROJECT_FILE%" -r linux-x64 --self-contained false -c Release -o "%PUBLISH_DIR%"
IF %ERRORLEVEL% NEQ 0 (
    echo ❌ ERROR: dotnet publish failed
    pause
    exit /b 1
)

REM Step 1.5: Copy appsettings files
echo 🗂 Copying appsettings files...
copy "%PROJECT_DIR%\appsettings*.json" "%PUBLISH_DIR%\" /Y

REM Step 2: Copy published files to server
echo 🚀 Uploading published files to %SERVER_HOST%:%REMOTE_PATH% ...
scp -r -i "%SSH_KEY%" "%PUBLISH_DIR%\*" %SERVER_USER%@%SERVER_HOST%:%REMOTE_PATH%
IF %ERRORLEVEL% NEQ 0 (
    echo ❌ ERROR: File transfer failed
    pause
    exit /b 1
)

REM Step 3: Restart the API service
echo 🔁 Restarting API service on server...
ssh -i "%SSH_KEY%" %SERVER_USER%@%SERVER_HOST% "sudo systemctl restart %SERVICE_NAME% && sudo systemctl status %SERVICE_NAME% --no-pager -l"
IF %ERRORLEVEL% NEQ 0 (
    echo ❌ ERROR: Failed to restart service
    pause
    exit /b 1
)

echo ✅ Deployment complete!
pause
