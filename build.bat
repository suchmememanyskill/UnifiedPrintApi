rmdir /s /q Release
cd UnifiedPrintApi
dotnet publish -o ../Release
cd ..
cd Release
rmdir /s /q storage