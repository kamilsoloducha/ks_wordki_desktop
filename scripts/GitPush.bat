@echo off
echo Start script...
call:gitcommit "vesper-oazachaosu"
call:gitcommit "vesper-wordki"

echo Success:):):)
echo.&pause&goto:eof

:gitcommit
echo Try to commit for %~1
cd %~1
git push
echo Done for %~1
cd ..
goto:eof