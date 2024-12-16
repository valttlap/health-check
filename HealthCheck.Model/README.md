To scaffold run
```bash
dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=5432;Database=health_check;User Id=<username>;Password=<password>;" Npgsql.EntityFrameworkCore.PostgreSQL --context-dir Context --schema health_check -o Entities --no-onconfiguring
```