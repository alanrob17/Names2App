# Names application

**Names** is a console application that allows me to properly name a group of files. It removes all diacritic's, selected emoji's, unwanted text and properly cases a filename.

It has been built in C# .Net Core 9 and uses Dependency Injection.

## Usage

Basic usage.

```bash
    names
```

This will produce a report on how all filenames in a folder will be renamed.

Full usage.

```bash
    names -swp
```

Where:

**s** include all files in sub directories

**w** change all filenames. If you leave this out it will just write a report on the filenames.

**p** change all filenames to proper case. Be careful using this as it will change names that are supposed to be all uppercase.

Examples from the log file.

```bash
C:\Temp\test\Renegades Born in the USA - Ebook.epub
to
C:\Temp\test\Renegades Born in the USA.epub

C:\Temp\test\Robert Louis Stevenson - Travels with a Donkey in the Cevennes sanet.st.epub
to
C:\Temp\test\Robert Louis Stevenson - Travels with a Donkey in the Cevennes.epub

C:\Temp\test\Roonéy,_Sally_-_Conversations_with_a_Friend - a Novel..._ .epub
to
C:\Temp\test\Rooney, Sally - Conversations with a Friend.epub

C:\Temp\test\TheSamsungGalaxyBookVol3RevisedEdition2014.epub
to
C:\Temp\test\The Samsung Galaxy Book Vol3Revised Edition2014.epub

C:\Temp\test\ROBERT C. MARTIN - CLEAN CODE.epub
to
C:\Temp\test\Robert C Martin - Clean Code.epub
```

**Note:** in the second last example. **Names** gets it nearly all correct. You have to correct a couple of minor issues. Also in the last example that we have removed all period (``.``) characters. You will have to manually add these back in.

Also note that in the examples I didn't use the ``-p`` flag.

## Publishing

This is a .Net Core 9 application and because it uses Dependency Injection and logging it include a large number of assemblies and this makes it hard to deploy the application.

I can get around this issue by using the ``PublishSingleFile`` feature built into the .Net SDK. This bundles all DLLs into a single platform-specific executable.

### Modify your .csproj

This is my original ``.csproj`` file.

```bash
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.9" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.9" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.9" />
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="9.0.9" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.9" />
    <PackageReference Include="Serilog" Version="4.3.0" />
    <PackageReference Include="Serilog.Extensions.Hosting" Version="9.0.0" />
    <PackageReference Include="Serilog.Extensions.Logging" Version="9.0.2" />
    <PackageReference Include="Serilog.Sinks.Async" Version="2.1.0" />
    <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
  </ItemGroup>

</Project>
```

Change the ``PropertyGroup`` to.

```bash
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <!-- Single File Configuration -->
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <!-- Optional: Enable compression to reduce size -->
    <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
  </PropertyGroup>
```

* ``PublishSingleFile``: Enables the feature
* ``SelfContained``: Includes the .NET runtime, so the target machine doesn't need it installed
* ``RuntimeIdentifier``: Specifies the target platform (e.g., linux-x64, osx-arm64)
* ``EnableCompressionInSingleFile``: Can significantly reduce the executable size, with a trade-off of a slight startup cost for decompression

To publish.

```bash
    dotnet publish.
```

**Note:** if you don't want to change your ``.csproj`` you can publish with this command.

```bash
    dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true
```

**Note:** by default, when the application runs, its DLLs are extracted to a temporary directory in the user's profile. You can control this location by setting the ``DOTNET_BUNDLE_EXTRACT_BASE_DIR`` environment variable.
