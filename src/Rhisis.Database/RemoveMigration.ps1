$db_config_file = Read-Host 'Database configuration file path?'

$env:DB_CONFIG = $db_config_file
dotnet ef migrations remove -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"
$env:DB_CONFIG = ""