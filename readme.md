Web Api With SPA Template
===================

A dotnet core 2.0 template for Web API in C# designed for pairing with single page application. A.K.A. The common toolkit for websites I build in order to hit the ground running faster.

Getting Started
-------------

#### Install
1. dotnet new -i webapiwithspa
2. dotnet new webapiwithspa

#### Uninstall
1. dotnet new -u webapiwithspa

#### Assumptions

- I want a dotnet core web api written in C# that will be the backend for a SPA written in Typescript. 
- I want a domain project with [CQRS pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs). 
- I want to use [Autofac](https://autofac.org/) for dependency injection. 
- I want to use [Serilog](https://serilog.net/) and [Seq](https://getseq.net/) for logging. 
- I want to generate a Typescript contract for the SPA. 
- I want (x)unit tests using [NSubstitute](http://nsubstitute.github.io/) for mocking and [Shoudly](https://github.com/shouldly/shouldly) for assertions. 
- I don't know what SPA.
- I don't know what authentication scheme.
- I don't know what persistent data store. 

#### Typescript Contract
The API project runs CodeGen project every compile to regenerate the contract. To change output location modify <WebsiteApiDir>.

```
  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <SourceRoot>../</SourceRoot>
    <ApiDir>$(SourceRoot)WebApiWithSpa.Api/</ApiDir>
    <CodegenDir>$(SourceRoot)WebApiWithSpa.CodeGen/</CodegenDir>
    <WebsiteApiDir>$(SourceRoot)/website/src/api</WebsiteApiDir>
  </PropertyGroup>

  <!-- Run the codegen project after API build -->
  <Target Name="Codegen Run" AfterTargets="Build">
    <Exec Command="dotnet run &quot;$(TargetPath)&quot; &quot;$(WebsiteApiDir)&quot;" WorkingDirectory="$(CodegenDir)">
    </Exec>
  </Target>
```

#### Development
Under \scripts
 - install.ps1 - Install the template
 - nuget-pack.ps1 - Packs up the template into nuget package.
 - uninstall.ps1 - Uninstall the template
 - list.ps1 - List installed templates
 - resettheworld.ps1 - Resets templates back to default list.
 