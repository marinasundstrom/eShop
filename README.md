# eShop

e-Commerce solution with a modern service-oriented architecture, using Blazor for UI.

Sites built with Bootstrap. Admin Portal with MudBlazor.

[Video 1](https://youtu.be/BbAThgEa5k8) [Video 2](https://youtu.be/NVqLKeuNO_w) [Video 3](https://youtu.be/rmg41zHW3Nw) [Video 4](https://youtu.be/nHbQ1a7WyyM) [Video 5](https://youtu.be/eHalPncX5W0)

Based on [YourBrand](https://github.com/marinasundstrom/YourBrand) project.

## Goal

Build a modern e-commerce solution that can handle multiple stores (tenants). Using technology such as Blazor Web Assembly.

## Running the project

Requires .NET 7 SDK, Tye (Dev orchestrator), and Docker Desktop.

To run the app, execute this when in the solution directory:

```sh
tye run
```

Add ```--watch``` to make it recompile on changes to any project.

Dashboard served on ```http://localhost:8000```

Demo site: ```https://localhost:6001```

Admin portal: ```https://localhost:5001```

### Seeding the databases

Web

```sh
dotnet run -- --seed
```

IdentityService

```sh
dotnet run -- /seed
```

### Login credentials

```
Username: alice 
Password: alice

Username: bob 
Password: bob
```
