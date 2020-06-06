if ($args.Count -lt 2) {
    Write-Host 'Usage: ./CreateMigration.ps1 [MigrationName] [DatabaseConfigurationFile]'
    exit 1
}

$migration_name = $args[0]
$env:DB_CONFIG = $args[1]

dotnet ef database update $migration_name -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"