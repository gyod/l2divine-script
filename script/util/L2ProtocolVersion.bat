@echo off
rem change l2.exe path to fit your envilonment
rem "C:\Program Files\NCSoft\Lineage II\system\l2.exe" -l2protocolversion
C:
cd "\Program Files\NCSoft\Lineage II\system"
del /f /q l2.exe
copy /y l2.bin l2.exe
l2.exe -l2protocolversion
