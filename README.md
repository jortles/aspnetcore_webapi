# .NET 10 WebAPI Boilerplate

## Running
- Install the .NET 10 SDK and the EF Core tools (`dotnet tool install --global dotnet-ef --version "10.*"`).
- From the `aspnetcore_webapi` project folder, set the `AppData` connection string with user secrets. For a local SQL Server, include `TrustServerCertificate=True` (also add it to an `AppData` secret you already have from the ASP.NET Core 3.1 version):
  `dotnet user-secrets set "ConnectionStrings:AppData" "Server=localhost;Database=aspnetcore_webapi;User Id=<user>;Password=<password>;TrustServerCertificate=True"`
- Set `JwtKey` with `dotnet user-secrets set "JwtKey" "<key>"`. HS256 token signing on .NET 10 requires a key of at least 32 bytes (32 ASCII characters); shorter keys fail with IDX10720.
- Set the `EmailConfiguration` section (`From`, `SmtpServer`, `Port`, `UserName`, `Password`) the same way; the app expects it at startup.
- AutoMapper 15 and later is commercially licensed by Lucky Penny Software (a free Community License is available). Without a key it is allowed for development and testing only; production use requires a license. Set your key with `dotnet user-secrets set "AutoMapper:LicenseKey" "<key>"`, never in the repository.
- Create the database with `dotnet ef database update`, then start the app with `dotnet run`.

# Todo
- Add JWT Refresh Tokens
