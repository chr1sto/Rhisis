$migration_name = Read-Host 'Migration name?'
$db_config_file = Read-Host 'Database configuration file path?'

$env:DB_CONFIG = $db_config_file
dotnet ef database update $migration_name -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"
$env:DB_CONFIG = ""