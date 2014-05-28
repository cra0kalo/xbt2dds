@echo off
REM Convert all xbt2 textures to dds
FOR /R %%a IN ("*.xbt") DO xbt2dds -t "%%a" "outputTextures"
pause