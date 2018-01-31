@echo off
echo "Build started"

REM -- set parameters
set msBuildPath="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
set zipPath="c:\Program Files\7-Zip\7z.exe"
set outputPath="C:\Temp\Wordki"
set outputZipFile="Wordki.zip"

REM -- remove output path to be sure that nothing is there
del /s /q %outputPath%

REM -- remove output zip file
del /s /q %outputPath%\..\wordki.zip

REM -- build project to proper place
cd ..
call %msBuildPath% Wordki.sln /t:Clean;Rebuild /p:Configuration=Release /p:OutputPath=%outputPath%
if not %errorlevel% == 0 goto:error

cd %outputPath%

REM -- remove all xml files
del *.xml

REM -- move all dll and pdb files to lib directory
move *.dll lib/
move *.pdb lib/

cd ..

REM -- zip whole directory to output zip file
call %zipPath% a %outputZipFile% .\Wordki\*
if not %errorlevel% == 0 goto:error

echo "Build successful"
pause
goto:eof

:error
echo "There are some errors"
echo ":( :( :("
pause