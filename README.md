# Beneficiarios API (.NET)

API para gestionar Beneficiarios y listar Documentos de Identidad.

## Requisitos
- .NET 10 SDK
- SQL Server (localdb / instancia local)
- (Opcional) SSMS

## Clonación del Repositorio
Para clonar el repositorio, utiliza el siguiente comando:

```bash
git clone https://github.com/Andorla0/Beneficiarios-Backend.git
```

## Base de datos
1. Crear la base de datos `bene2`:

```bash
sqlcmd -S . -E -C -Q "CREATE DATABASE bene2"
```

2. Ejecutar el script para crear tablas, seed data y stored procedures:

```bash
cd database
sqlcmd -S . -E -N o -d bene2 -u -i schema_bene2.sql
```

**Nota:** Asegúrate de estar en la carpeta `database` antes de ejecutar el segundo comando.

## Configuración
Editar `Beneficiarios.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "BeneficiariosDb": "Server=.;Database=bene2;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
**Importante: Crear una base de datos con el nombre "bene2".**

## Ejecutar
Navegar a la carpeta del proyecto y ejecutar:

```bash
cd Beneficiarios.API
dotnet run
```

La API estará disponible en: `http://localhost:5063`

Swagger UI:
- `http://localhost:5063/swagger`

## Endpoints
#### Documentos
- `GET /api/documentos/activos`

#### Beneficiarios
- `GET /api/beneficiarios?Nombre=&NumeroDocumento=&DocumentoIdentidadId=&Page=&PageSize=`
- `GET /api/beneficiarios/{id}`
- `POST /api/beneficiarios`
- `PUT /api/beneficiarios/{id}`
- `DELETE /api/beneficiarios/{id}`
