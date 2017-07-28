msbuild /t:Build /p:Configuration=Release /nr:false

echo f|xcopy IconGenerator\bin\Release\icogen.exe !Release\icogen.exe /y