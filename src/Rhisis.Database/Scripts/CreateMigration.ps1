if ($args.Count -lt 2) {
    Write-Host 'Usage: ./CreateMigration.ps1 [MigrationName] [DatabaseConfigurationFile]'
    exit 1
}

$migration_name = $args[0]
$env:DB_CONFIG = $args[1]

dotnet ef migrations add $migration_name -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"

$confirmation = Read-Host "Apply migrations? (y/n)"

if ($confirmation -eq 'y') {
  dotnet ef database update -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"
}

$env:DB_CONFIG = "" 