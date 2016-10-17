#r @"../../packages/FAKE/tools/FakeLib.dll"
#r @"../../packages/FAKE.FluentMigrator/tools/Fake.FluentMigrator.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ProcessHelper
open Fake.FluentMigratorHelper

let artifactsDir = "./artifacts"
let buildDir = artifactsDir + "/build/app"
let buildMigrationsDir = artifactsDir + "/build/migrations"
let migrationsAssembly = [ buildMigrationsDir + "/NAME_REPLACE.Migrations.dll" ]
let publishDir = buildDir + "/_PublishedWebsites/"
let deployDir = artifactsDir + "/deploy/"
let version = "0.1.0"
let copyright = "Copyright ©  2016"
let company = "NAME_REPLACE"
let connection = ConnectionStringFromConfig("DevelopmentMigrateConnection", "./scripts/application/migrate.config", SqlServer(V2014))
let options = DefaultMigrationOptions

Log "fake started..."

Target "Clean" (fun _ ->
    killMSBuild()
    CleanDirs [buildMigrationsDir; buildDir; publishDir; deployDir;]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./app/NAME_REPLACE/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE"
         Attribute.Description "Camada do core da API do projeto EAD aluno digital."
         Attribute.Product "EAD.AlunoDigital"
         Attribute.Guid "f464c056-1b4a-4477-8168-9304241b9c7d"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Abstraction/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Abstraction"
         Attribute.Description "Camada de abstrações da API do projeto."
         Attribute.Product "NAME_REPLACE.Abstraction"
         Attribute.Guid "7f22fd06-01bd-4db8-b9ec-56584c88a02f"
         Attribute.Copyright copyright
         Attribute.Company company         
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Binding/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Binding"
         Attribute.Description "Camada de configuração de registros para inversão de controle e injeção de dependência da API do projeto."
         Attribute.Product "NAME_REPLACE.Binding"
         Attribute.Guid "5cd2e917-56f0-4e62-9a59-3e74ba9a1fde"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Business/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Business"
         Attribute.Description "Camada de regras de negócios da API do projeto."
         Attribute.Product "NAME_REPLACE.Business"
         Attribute.Guid "0110581d-35a7-47f1-8e54-cee77734d2db"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Common/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Common"
         Attribute.Description "Camada do core compartilhado da API do projeto."
         Attribute.Product "NAME_REPLACE.Common"
         Attribute.Guid "943f2552-6db2-40c9-a0c5-06708ea8254c"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Common.Abstraction/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Common.Abstraction"
         Attribute.Description "Camada de abstrações compartilhadas da API do projeto."
         Attribute.Product "NAME_REPLACE.Common.Abstraction"
         Attribute.Guid "0ef0726e-3e34-4362-84f6-ba1a8fcf8d14"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Common.Business/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Common.Business"
         Attribute.Description "Camada de regras de negócios compartilhadas da API do projeto."
         Attribute.Product "NAME_REPLACE.Common.Business"
         Attribute.Guid "b3b2b090-fc3b-4548-84eb-8f5298bae78a"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.DAO/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.DAO"
         Attribute.Description "Camada de acesso a dados da API do projeto."
         Attribute.Product "NAME_REPLACE.DAO"
         Attribute.Guid "7e4a0769-38f4-4213-b7e4-2fedd847f1e3"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./app/NAME_REPLACE.Owin/Properties/AssemblyInfo.cs"
        [Attribute.Title "NAME_REPLACE.Owin"
         Attribute.Description "Camada principal da API do projeto."
         Attribute.Product "NAME_REPLACE.Owin"
         Attribute.Guid "968cf1d6-3467-4103-93b7-557fe281f0b9"
         Attribute.Copyright copyright
         Attribute.Company company
         Attribute.Version version
         Attribute.FileVersion version]
)

Target "BuildApp" (fun _ ->
    !! @"./app/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "BuildMigrations" (fun _ ->
    !! @"./database/**/*.csproj"
      |> MSBuildRelease buildMigrationsDir "Build"
      |> Log "AppBuild-Output: "
)

Target "MigrateDatabaseDown" (fun _ ->
    !! @"./database/**/*.csproj"
      |> MSBuildRelease buildMigrationsDir "Build"
      |> Log "AppBuild-Output: "

    RollbackLatest connection migrationsAssembly options
)

Target "MigrateDatabaseUp" (fun _ ->
    !! @"./database/**/*.csproj"
      |> MSBuildRelease buildMigrationsDir "Build"
      |> Log "AppBuild-Output: "

    MigrateToLatest connection migrationsAssembly options
)

Target "CopyDeploy" (fun _ ->
    CopyDir deployDir publishDir (fun x -> true)
)

"Clean"
  ==> "SetVersions"
  ==> "BuildApp"
  ==> "BuildMigrations"
  ==> "CopyDeploy"

RunTargetOrDefault "CopyDeploy"