@echo off
setlocal ENABLEDELAYEDEXPANSION
if "%1"=="" goto BLANK
set timestamp=%date:~-4%%date:~-7,2%%date:~-10,2%%time:~0,2%%time:~3,2%%time:~6,2%
set name=%1

set cero=0
set timestamp=%timestamp: =!cero!%

echo "/*escribir abajo el script*/" > %timestamp%_%name%.sql

echo "Se creo el archivo" %timestamp%_%name%".sql"
goto DONE

:BLANK
echo "falta el parametro para el nombre"
:DONE
echo.