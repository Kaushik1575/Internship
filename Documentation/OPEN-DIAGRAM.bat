@echo off
echo Opening diagram in your browser...
echo Make sure "dotnet run" is running first!
start http://localhost:5000/Home/Diagram
timeout /t 8
