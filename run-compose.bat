@echo off
for /f %%a in ('powershell Invoke-RestMethod api.ipify.org') do set PublicIP=%%a
SET DOCKER_IP = %PublicIP%

docker-compose up
