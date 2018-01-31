@echo off
echo "Build started"

REM -- set parameters
set msBuildPath="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
set zipPath="c:\Program Files\7-Zip\7z.exe"
set winScpPath="e:\Downloads\WinSCP-5.11.3-Portable\winscp.com"
set outputPath="C:\Temp\Wordki"
set outputZipFile="Wordki.zip"
set username=%1
set password=%2
set hostkey="ssh-ed25519%20256%20b2:39:f8:2c:6f:e5:bb:fb:38:02:63:84:99:1e:ff:b5"

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
call %zipPath% a %outputZipFile% %outputPath%\*
if not %errorlevel% == 0 goto:error

REM -- Send zip file to server
call %winScpPath% /command "open sftp://%username%:%password%@37.233.102.223 -hostkey='%hostkey%'" "put %outputZipFile% /home/oazachaosu/" "exit"
if not %errorlevel% == 0 goto:error

echo "Build successful"
pause
goto:eof

:error
echo "There are some errors"
echo ":( :( :("
pause