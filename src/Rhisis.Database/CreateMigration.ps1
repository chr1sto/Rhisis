<#
 # This scripts is here to help developers to generate Entity Framework Core migrations.
 # At the moment, the DatabaseFactory implements the IDesignTimeDbContextFactory interface and has the CreateDbContext(string[] args) method.
 # This method is used to create Db contexts but it is not used **yet** in Entity Framework Core.
 # So this script is an alternative.
 # Tracking issue: https://github.com/aspnet/EntityFrameworkCore/issues/8332
 #>

$migration_name = Read-Host 'Migration name?'
$db_config_file = Read-Host 'Database configuration file path?'

$env:DB_CONFIG = $db_config_file
dotnet ef migrations add $migration_name -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"

$confirmation = Read-Host "Apply current migration? (y/n)"

if ($confirmation -eq 'y') {
  dotnet ef database update -p "Rhisis.Database.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"
}

$env:DB_CONFIG = ""