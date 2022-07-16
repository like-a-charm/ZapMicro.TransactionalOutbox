# ZapMicrro.TransactionalOutbox

[Repository](https://github.com/like-a-charm/zapmicro-transactionaloutbox) | [Nuget Package](https://www.nuget.org/packages/ZapMicro.TransactionalOutbox/)

ZapMicro.TransactionalOutbox is an implementation of the Transactional Outbox pattern for .NET Core and Entity Framework Core.
## Table of contents

1. [Installation](#configuration)
2. [Usage](#usage)
3. [Contributing](#contributing)

## Installation

Install using the [ZapInjector package](https://www.nuget.org/packages/ZapInjector/)

```
PM> Install-Package ZapMicro.TransactionalOutbox
```

## Usage

When you install the package, it should be added to your _csproj_ file. Alternatively, you can add it directly by adding:

```xml
<PackageReference Include="ZapMicro.TransactionalOutbox" Version="1.0.0" />
```

Define your configuration in the application.json or in the application.yaml files as done in [this example](https://github.com/like-a-charm/zapinjector/tree/main/examples/ZapInjector.Examples.Main)
In the Startup.cs file, in the ConfigureServices  method, call the extension method _LoadFromConfiguration_

```c#
services.LoadFromConfiguration(Configuration);
```




## Contributing

The Contributing guide can be found [here](https://github.com/like-a-charm/zapinjector/tree/main/Contributing.md)

## Authors
- [Daniele De Francesco](https://github.com/danieledefrancesco)
