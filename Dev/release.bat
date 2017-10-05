@echo off

echo.
echo *** Copy unium
echo.

rmdir /s /q Unium
call robocopy Tutorial\Assets\Unium  Unium /xf *.meta /E



REM echo.
REM echo *** Build tutorial
REM echo.

REM cd Dev\tutorial
REM rmdir /s /q root
REM call yarn
REM call yarn release
REM cd ..\..


echo.
echo *** Copy tutorial
echo.

REM rmdir /s /q Tutorial\Assets\StreamingAssets\tutorial
set TUT=Tutorial\Assets\StreamingAssets\tutorial
for /f %%F in ('dir %TUT% /b /a-d /b /a-d ^| findstr /vile ".meta"') do del %TUT%\%%F

call robocopy Dev\tutorial\root Tutorial\Assets\StreamingAssets\tutorial
call robocopy Extras\examples Tutorial\Assets\StreamingAssets\tutorial
