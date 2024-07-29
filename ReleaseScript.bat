@echo on

REM MSBuild
SET MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"

REM ビルド
%MSBUILD_PATH% BookManager.sln /p:Configuration=release;Platform="Any CPU"

REM 配置
xcopy /Y ".\BookManager\bin\Release\net8.0-windows7.0\*" "C:\MyProgramFiles\BookManager\bin"

pause
