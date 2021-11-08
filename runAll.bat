@echo off

npm run build
dotnet run
pypy3 main.py
py -3 main.py

pause