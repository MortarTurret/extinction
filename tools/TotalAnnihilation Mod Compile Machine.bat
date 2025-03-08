@echo off
title Total Annihilation compiler (By BaseEncrypt)
set acolor=0
set text=c
set goinsane=10
color 0c

:start
echo.
echo  T O T A L  A N N I H I L A T I O N
echo.
echo  Mod compiler machine.
echo  By BaseEncrypt.
echo.
echo  For the Tribes community.
echo  Keep hitting those midairs you dirty tribals.
echo  Press 1-9 to change colors. do not press 0.
echo  https://tr1bes.us/
IF NOT EXIST vt.exe goto incomplete
IF NOT EXIST vtExtract.exe goto incomplete
IF NOT EXIST voldir.exe goto incomplete
IF NOT EXIST cygwin1.dll goto incomplete

:choice
echo.
ECHO A. Visit https://tr1bes.us/
IF EXIST scripts.vol (
ECHO B. Backup Scripts.vol to Scripts.bak
)

IF EXIST *.cs (
ECHO C. Compile new Scripts.vol
)

IF EXIST scripts.vol (
ECHO D. Decompile Scripts.vol
)
IF EXIST *.cs (
ECHO E. Compile ENCRYPTED Scripts.vol
)
IF EXIST *.cs (
ECHO F. File existing .cs Files.
IF EXIST oldcs ECHO P. Purge .cs files from mod directory.
) 
ECHO Q. Quit.
IF NOT EXIST *.cs (
ECHO R. Restore backed up .cs files.
)




echo.

:choose
set choice=
set /p choice=Choose an option.
if not '%choice%'=='' set choice=%choice:~0,1%
if '%choice%'=='a' goto annweb
if '%choice%'=='b' goto backup
if '%choice%'=='c' goto compile
if '%choice%'=='d' goto decompile
if '%choice%'=='e' goto encrypt
if '%choice%'=='f' goto backupcs
if '%choice%'=='m' start https://tr1bes.us/
if '%choice%'=='p' goto delcs
if '%choice%'=='q' goto end
if '%choice%'=='r' goto restore
if '%choice%'=='t' start https://tr1bes.us/
if '%choice%'=='x' start https://tr1bes.us/

if '%choice%'=='0' goto insane
goto changecolor

:changecolor
set /a acolor+=1
IF '%acolor%'=='8' (
set acolor=0
IF '%text%'=='c' set text=f
IF '%text%'=='f' set text=a
IF '%text%'=='a' set text=b
IF '%text%'=='b' set text=d
IF '%text%'=='d' set text=e
IF '%text%'=='e' set text=c
) 
color %acolor%%text%
if %goinsane% GTR 0 (
set /a goinsane-=1
goto changecolor
)

goto choice

:insane
echo.
echo  N O B O D Y S A I D T O P R E S S Z E R O ! ! !
set goinsane=1000
goto changecolor

:compile
echo.
IF EXIST scripts.vol (
del scripts.vol /q
echo Old Scripts.vol deleted.
)
echo Compiling...
for %%f in (*.cs) do vt -q scripts.vol %%f
echo New Scripts.vol created.
goto choice

:encrypt
echo.
IF EXIST scripts.vol (
del scripts.vol /q
echo Old Scripts.vol deleted.
)
echo Compiling...
for %%f in (*.cs) do vt -q -lzh scripts.vol %%f
echo Encrypted Scripts.vol created.
goto choice

:backup
echo.
IF EXIST scripts.vol (
IF EXIST scripts.bak (
del scripts.bak /q
echo Previous backup deleted.
 )
copy /y scripts.vol scripts.bak
del scripts.vol
echo Scripts.vol backed up to Scripts.bak
 ) ELSE (
echo No Scripts.vol to back up.
)
goto choice

:restore
IF EXIST *.cs goto changecolor
echo.
for %%f in (oldcs\*.cs) do copy %%f *.cs
echo All .cs files in oldcs directory restored.
goto choice


:backupcs
echo.
IF EXIST *.cs (
IF EXIST oldcs (
del oldcs /q
rd oldcs /q
echo Previous backup deleted.
 )
md oldcs
for %%f in (*.cs) do copy %%f oldcs
echo All .cs files backed up in oldcs directory
 ) ELSE (
echo No .cs files to back up.
)
goto choice

:delcs
IF NOT EXIST oldcs goto changecolor
echo.
IF EXIST *.cs (
del *.cs /q
echo All .cs files in root deleted.
 ) ELSE (
echo No .cs files to delete.
)
goto choice

:decompile
echo.
IF EXIST decompiled (
del decompiled /q
rd decompiled /q
echo Previous decompile deleted.
 )
echo Decompiling...
voldir scripts.vol > modlist
md decompiled
for /f %%f in (modlist) do vtextract scripts.vol %%f decompiled/%%f
del modlist /q
echo Done.
goto choice

:annweb
start https://tr1bes.us/
goto choice

:incomplete
echo.
echo Missing components.
echo Please make sure vt.exe, vtExtract.exe, cygwin1.dll , and voldir.exe are present and restart.
echo Visit https://tr1bes.us/ for help.
pause
:end