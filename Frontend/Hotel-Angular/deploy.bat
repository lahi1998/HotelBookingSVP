@echo off
REM =====================================
REM Portable Deployment Script (Test)
REM =====================================

REM Server configuration
SET SERVER_USER=root
SET SERVER_HOST=157.180.73.14
SET REMOTE_ROOT=/var/www/hotel-hyggely/wwwroot
SET SSH_KEY=%USERPROFILE%\.ssh\id_ed25519

REM Local paths (relative to this script)
SET SCRIPT_DIR=%~dp0
SET LOCAL_DIST=%SCRIPT_DIR%dist\Hotel-Angular\browser\

REM Check if local folder exists
if not exist "%LOCAL_DIST%" (
    echo ERROR: Local folder not found: %LOCAL_DIST%
    pause
    exit /b 1
)

echo Deploying %LOCAL_DIST% to %SERVER_HOST%:%REMOTE_ROOT% ...

REM Step 1: Copy files to server
scp -r -i "%SSH_KEY%" "%LOCAL_DIST%*" %SERVER_USER%@%SERVER_HOST%:%REMOTE_ROOT%
IF %ERRORLEVEL% NEQ 0 (
    echo ERROR: File transfer failed
    pause
    exit /b 1
)

REM Step 2: Reload Nginx
ssh -i "%SSH_KEY%" %SERVER_USER%@%SERVER_HOST% "sudo systemctl reload nginx"
IF %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to reload Nginx
    pause
    exit /b 1
)

echo ✅ Deployment complete!
pause