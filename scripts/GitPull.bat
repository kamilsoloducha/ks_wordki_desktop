@echo off
echo Start script...
call:gitpull "vesper-oazachaosu"
call:gitpull "vesper-wordki"

echo Success:):):)
echo.&pause&goto:eof

:gitpull
echo Try to pull for %~1
cd %~1
git pull
echo Done for %~1
cd ..
goto:eof