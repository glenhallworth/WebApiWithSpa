Web Api With SPA Template
===================

A dotnet core 2.0 template for Web API in C# designed for pairing with single page application. 

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
- I don't know what (or if) persistent data store. 

#### Development
Under \scripts
 - install.ps1 - Install the template
 - nuget-pack.ps1 - Packs up the template into nuget package.
 - uninstall.ps1 - Uninstall the template
 - list.ps1 - List installed templates
 - resettheworld.ps1 - Resets templates back to default list.
 