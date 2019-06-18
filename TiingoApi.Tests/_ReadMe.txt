
Get the authorization token here: https://api.tiingo.com/account/api/token

The authentication token should be put into the user secrets cache.  Don't want it checked into the source control.

	dotnet user-secrets set TiingoSettings:AuthorizationToken "Token goes here"

User-secrets

	dotnet user-secrets --help
	dotnet user-secrets set TiingoSettings:AuthorizationToken "notValidab3NotValidk58NotValid4rNotValid"
	dotnet user-secrets clear : removes all secrets from the store
	dotnet user-secrets list : shows you all existing keys
	dotnet user-secrets remove <key> : removes the specific key

	The nuget package does not put the <DotNetCliToolReference> and <UserSecretsId> in the .csproj file.
	Need these tags for the command "dotnet user-secrets" to work.
	See: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio-code

Testing

	http://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata
