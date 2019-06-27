"%ProgramFiles%\dotnet\dotnet.exe" build 
"%ProgramFiles%\dotnet\dotnet.exe" test ../Traffic.Tests
"%ProgramFiles%\dotnet\dotnet.exe" run --project ../Traffic
PAUSE