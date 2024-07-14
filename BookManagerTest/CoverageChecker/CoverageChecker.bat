REM @echo off
rem ‚±‚Ìƒoƒbƒ`ƒtƒ@ƒCƒ‹‚ğVSã‚©‚çƒRƒ}ƒ“ƒhƒvƒƒ“ƒvƒg‚ÅŠJ‚¯‚ÎOK
rem OpenCover‚ÌƒCƒ“ƒXƒg[ƒ‹æ
SET OPEN_COVER=%USERPROFILE%\.nuget\packages\opencover\4.7.1221\tools

rem Report¶¬ƒc[ƒ‹‚ÌƒCƒ“ƒXƒg[ƒ‹æ
SET REPORT_GEN=%USERPROFILE%\.nuget\packages\reportgenerator\5.2.4\tools\net6.0

rem ƒeƒXƒgƒtƒŒ[ƒ€ƒ[ƒN‚ÌƒCƒ“ƒXƒg[ƒ‹æ
SET MS_TEST=%ProgramFiles%\Microsoft Visual Studio\2022\Community

rem ƒ^[ƒQƒbƒgƒAƒZƒ“ƒuƒŠ (ƒeƒXƒgƒNƒ‰ƒX‚ª‚ ‚éDLLƒtƒ@ƒCƒ‹) 
SET TARGET_TEST=BookManagerTest.dll

rem ƒ^[ƒQƒbƒgƒAƒZƒ“ƒuƒŠ‚ÌŠi”[æ (ƒeƒXƒgƒNƒ‰ƒX‚ª‚ ‚éêŠ)
SET TARGET_TEST_DIR=%USERPROFILE%\Desktop\codes\BookManager\BookManagerTest\bin\Debug\net8.0-windows7.0

REM ƒJƒoƒŒƒbƒWŒv‘ª‘ÎÛ (ƒeƒXƒg‘ÎÛƒNƒ‰ƒX‚ÌNAMESPACE)
SET FILTERS= +[BookManager]* -[*]*.App -[*]*.*Window* -[*]*.*Sqlite* -[*]*.Resources -[*]*.Settings -[*]XamlGeneratedNamespace*
REM " -[BookManager*]*Window*"

REM ƒpƒX‚Ìİ’è
SET PATH=%PATH%;%OPEN_COVER%;%MS_TEST%;%REPORT_GEN%

REM OpenCover‚ğï¾€s (./CoverageReportƒtƒHƒ‹ƒ_‚ÖŒ‹‰Êo—Í)
OpenCover.Console -register:user -target:"%MS_TEST%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" -targetargs:"%TARGET_TEST%" -targetdir:"%TARGET_TEST_DIR%" -filter:"%FILTERS%" -output:".\CoverageReport\result.xml"

REM ReportGeneratorr‚ğï¾€s (./CoverageReportƒtƒHƒ‹ƒ_‚ÖŒ‹‰Êo—Í)
ReportGenerator -reports:".\CoverageReport\result.xml" -reporttypes:Html -targetdir:".\CoverageReport"

REM Œ‹‰Ê‚ğŠJ‚­
.\CoverageReport\index.html
pause
