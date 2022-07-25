# Multi-Tenant IdentityServer

## Getting Started

1. Create a PostgreSQL container.
   ```shell
   docker run -e POSTGRES_PASSWORD=password -p 5432:5432 -d postgres
   ```
2. Add an entry to the hosts file.
   ```text
   127.0.0.1 one.example.local
   ```
3. Run the IdentityServer project.
   ```shell
   dotnet run --project ./src/IdentityServer/IdentityServer.csproj
   ```
4. Run the Client project.
   ```shell
   dotnet run --project ./src/Client/Client.csproj
   ```
5. Generate a self-signed certificate.
   ```shell
   openssl req -x509 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -addext "subjectAltName=DNS:localhost,DNS:*.example.local"
   ```
   ```shell
   openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx -name "Multi-Tenant IdentityServer"
   ```
6. Import the self-signed certificate.
   ```shell
   certutil -f -user -importpfx Root localhost.pfx
   ```
7. [Open the Client application](https://one.example.local:5002).
8. Sign in to the IdentityServer.
   * Username: `alice` or `bob`.
   * Password: `alice` or `bob`.
9. Click on the "Sign Out" link.
10. Observe the thrown exception.
