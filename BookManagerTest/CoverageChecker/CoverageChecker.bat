REM @echo off
rem このバッチファイルをVS上からコマンドプロンプトで開けばOK
rem OpenCoverのインストール先
SET OPEN_COVER=%USERPROFILE%\.nuget\packages\opencover\4.7.1221\tools

rem Report生成ツールのインストール先
SET REPORT_GEN=%USERPROFILE%\.nuget\packages\reportgenerator\5.2.4\tools\net6.0

rem テストフレームワークのインストール先
SET MS_TEST=%ProgramFiles%\Microsoft Visual Studio\2022\Community

rem ターゲットアセンブリ (テストクラスがあるDLLファイル) 
SET TARGET_TEST=BookManagerTest.dll

rem ターゲットアセンブリの格納先 (テストクラスがある場所)
SET TARGET_TEST_DIR=%USERPROFILE%\Desktop\codes\BookManager\BookManagerTest\bin\Debug\net8.0-windows7.0

REM カバレッジ計測対象 (テスト対象クラスのNAMESPACE)
SET FILTERS= +[BookManager]* -[*]*.App -[*]*.*Window* -[*]*.Resources -[*]*.Settings -[*]XamlGeneratedNamespace*

REM パスの設定
SET PATH=%PATH%;%OPEN_COVER%;%MS_TEST%;%REPORT_GEN%

REM OpenCoverを�ｾ�行 (./CoverageReportフォルダへ結果出力)
OpenCover.Console -register:user -target:"%MS_TEST%\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" -targetargs:"%TARGET_TEST%" -targetdir:"%TARGET_TEST_DIR%" -filter:"%FILTERS%" -output:".\CoverageReport\result.xml"

REM ReportGeneratorrを�ｾ�行 (./CoverageReportフォルダへ結果出力)
ReportGenerator -reports:".\CoverageReport\result.xml" -reporttypes:Html -targetdir:".\CoverageReport"

REM 結果を開く
.\CoverageReport\index.html
pause
