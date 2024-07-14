REM @echo off
rem ���̃o�b�`�t�@�C����VS�ォ��R�}���h�v�����v�g�ŊJ����OK
rem OpenCover�̃C���X�g�[����
SET OPEN_COVER=%USERPROFILE%\.nuget\packages\opencover\4.7.1221\tools

rem Report�����c�[���̃C���X�g�[����
SET REPORT_GEN=%USERPROFILE%\.nuget\packages\reportgenerator\5.2.4\tools\net6.0

rem �e�X�g�t���[�����[�N�̃C���X�g�[����
SET MS_TEST=%ProgramFiles%\Microsoft Visual Studio\2022\Community

rem �^�[�Q�b�g�A�Z���u�� (�e�X�g�N���X������DLL�t�@�C��) 
SET TARGET_TEST=BookManagerTest.dll

rem �^�[�Q�b�g�A�Z���u���̊i�[�� (�e�X�g�N���X������ꏊ)
SET TARGET_TEST_DIR=%USERPROFILE%\Desktop\codes\BookManager\BookManagerTest\bin\Debug\net8.0-windows7.0

REM �J�o���b�W�v���Ώ� (�e�X�g�ΏۃN���X��NAMESPACE)
SET FILTERS= +[BookManager]* -[*]*.App -[*]*.*Window* -[*]*.*Sqlite* -[*]*.Resources -[*]*.Settings -[*]XamlGeneratedNamespace*
REM " -[BookManager*]*Window*"

REM �p�X�̐ݒ�
SET PATH=%PATH%;%OPEN_COVER%;%MS_TEST%;%REPORT_GEN%

REM OpenCover��ﾀ�s (./CoverageReport�t�H���_�֌��ʏo��)
OpenCover.Console -register:user -target:"%MS_TEST%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" -targetargs:"%TARGET_TEST%" -targetdir:"%TARGET_TEST_DIR%" -filter:"%FILTERS%" -output:".\CoverageReport\result.xml"

REM ReportGeneratorr��ﾀ�s (./CoverageReport�t�H���_�֌��ʏo��)
ReportGenerator -reports:".\CoverageReport\result.xml" -reporttypes:Html -targetdir:".\CoverageReport"

REM ���ʂ��J��
.\CoverageReport\index.html
pause
