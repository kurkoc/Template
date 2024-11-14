# .NET API Development Makefile

# Variables
CONTEXT_PROJECT = src/TemplateSolution.Infrastructure
STARTUP_PROJECT = src/TemplateSolution.API
MIGRATIONS_DIR = Persistence/Migrations


migration:
	dotnet ef migrations add $(name) --project $(CONTEXT_PROJECT) --output-dir $(MIGRATIONS_DIR) --startup-project $(STARTUP_PROJECT)
	
update:
	dotnet ef database update --verbose --project $(CONTEXT_PROJECT) --startup-project $(STARTUP_PROJECT)