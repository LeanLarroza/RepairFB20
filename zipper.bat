del "C:\Proyectos VB.Net\RepairFB20\*.zip" /s /q
xcopy /s "C:\Proyectos VB.Net\RepairFB20\Output" "C:\Proyectos VB.Net\RepairFB20\Output2\"
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.config" /s /q
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.application" /s /q
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.manifest" /s /q
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.pdb" /s /q
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.ini" /s /q
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.xml" /s /q
del "C:\Proyectos VB.Net\RepairFB20\Output2\*.lnk" /s /q
@RD /S /Q "C:\Proyectos VB.Net\RepairFB20\Output2\Logs"
@RD /S /Q "C:\Proyectos VB.Net\RepairFB20\Output2\app.publish"
pause
"C:\Program Files\7-Zip\7z" a -tzip "C:\Proyectos VB.Net\RepairFB20\RepairFB20.zip" "C:\Proyectos VB.Net\RepairFB20\Output2\*.*" -mx5
"C:\Program Files\7-Zip\7z" x "C:\Proyectos VB.Net\RepairFB20\RepairFB20.zip" -o"C:\Proyectos VB.Net\RepairFB20\RepairFB20\RepairFB20" -aoa
@RD /S /Q "C:\Proyectos VB.Net\RepairFB20\Output2"
echo File RepairFB20.zip / Cartella RepairFB20 creati
start %windir%\explorer.exe "C:\Proyectos VB.Net\RepairFB20\RepairFB20" 