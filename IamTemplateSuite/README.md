# IAM Template Suite

The **IAM Template Suite** provides a collection of integrated templates designed to streamline the provisioning of IAM infrastructure.

📘 To provision the full IAM infrastructure using the suite, follow the instructions in [`GUIDE.md`](./GUIDE.md).

---

## 📥 Installation

To install the template suite globally on your machine:

```bash
dotnet new install <package-name>
```

To view the installed templates:

```bash
dotnet new --list
```

---

## 🚀️ Usage

After installation, create a new project using one of the templates:

```bash
dotnet new <shortName> [options]
```

- Replace `<shortName>` with the template's short name (see `dotnet new --list`).
- Include any parameters defined by the template.
  - Optionally use `--dry-run` to preview the output without creating files.

This will generate a new project directory based on the selected template's structure.

---

## 🤝 Contributing

Interested in improving or adding new templates? Follow these steps:

### 1. Create a New Template

```bash
cd Templates
mkdir <NewTemplateName>
cd <NewTemplateName>
```

Set up your project inside this directory as usual, including all files, folders, and configuration you want included in the template.

### 2. Configure Template Metadata

Inside your template root, create a `.template.config` directory with a `template.json` file:

```bash
mkdir .template.config
touch .template.config/template.json
```

Populate `template.json` with the required metadata. Refer to other templates in the suite for guidance.

### 3. Test Your Template

Install the template locally to test:

```bash
dotnet new install .
```

Generate a new project to validate it:

```bash
dotnet new <your-short-name>
```


### 📦 Dependencies (NuGet Packages)

The templates depend on each other via NuGet packages. You can set up the required development environment using the provided scripts:

- **Setup**: `LocalDev/SetupLocalEnv.sh`
  - Creates a local NuGet feed
  - Packs and publishes specified packages into the feed
- **Teardown**: `LocalDev/DestroyLocalEnv.sh`
  - Destroys everything created by the Setup script

### ➕ Adding New NuGet Packages

#### 1. Add a Symbol in `template.json`

```json
"DefaultFalse": {
  "type": "parameter",
  "datatype": "bool",
  "defaultValue": "false",
  "description": "Include template development configuration"
}
```

#### 2. Configure `.csproj` for Local Packaging

In each NuGet package project:

```xml
<!--#if DefaultFalse-->
<PropertyGroup>
  <PackageOutputPath>../path/to/LocalDev/LocalNuGetFeed</PackageOutputPath>
</PropertyGroup>
<!--#endif -->
```

Build and deploy to the local feed:

```bash
dotnet build
```

Finally add the package to the `PROJECTS` list in `LocalDev/SetupLocalEnv.sh`.

### 📥 Consuming NuGet Packages

In consuming projects, add a package reference in your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="TOrgName.T*RepositoryName.Core" Version="*-*" />
</ItemGroup>
```

Then, configure `nuget.config` (ensure the filename is lowercase):

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <!--#if DefaultFalse-->
    <add key="LocalNuGetFeed" value="../../../LocalDev/LocalNuGetFeed" />
    <!--#else-->
    <add key="TOrgPackageSourceKey" value="TOrgPackageSourceValue" />
    <!--#endif -->
  </packageSources>
  <packageSourceMapping>
    <packageSource key="nuget.org">
      <package pattern="*" />
    </packageSource>
    <!--#if DefaultFalse-->
    <packageSource key="LocalNuGetFeed">
      <package pattern="TOrgName.*" />
    </packageSource>
    <!--#else-->
    <packageSource key="TOrgPackageSourceKey">
      <package pattern="TOrgName.*" />
    </packageSource>
    <!--#endif -->
  </packageSourceMapping>
  <!--#if DefaultFalse-->
  <disabledPackageSources>
    <add key="TOrgPackageSourceKey" value="true" />
  </disabledPackageSources>
  <!--#endif -->
</configuration>
```
