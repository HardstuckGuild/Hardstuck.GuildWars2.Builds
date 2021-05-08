# Hardstuck.GuildWars2.Builds

[![Nuget](https://img.shields.io/nuget/v/Hardstuck.GuildWars2.Builds?style=for-the-badge)](https://www.nuget.org/packages/Hardstuck.GuildWars2.Builds/)

GW2 build code generator for Hardstuck Builds.

Maintained by @mightyteapot & @Plenyx.

---

# Quick start

### Install the NuGet package "[Hardstuck.GuildWars2.Builds](https://www.nuget.org/packages/Hardstuck.GuildWars2.Builds/)" into your .NET Framework or .NET Core application

---

### Include the namespace in the header

```csharp
using Hardstuck.GuildWars2.Builds;
```

---

### Quickly generate a build code:

```csharp
using (GW2BuildParser parser = new GW2BuildParser("My API key"))
{
    APIBuild build = await parser.GetAPIBuildAsync("My Amazing Character", GW2GameMode.PvE);
    Console.WriteLine(build.GetBuildCode());
}
```

Note that `GW2BuildParser` implements `IDisposable`, henceforth `Dispose()` method or `using` statement is required to release resources held by the class.

---

### Example method to extract the build code with `using` statement:

```csharp
private async Task<string> GetBuild(string apiKey, string characterName, GW2GameMode gameMode)
{
    using (GW2BuildParser parser = new GW2BuildParser(apiKey))
    {
        APIBuild build = await parser.GetAPIBuildAsync(characterName, gameMode);
        return build.GetBuildCode();
    }
}
```

### Alternative way with `Dispose()`:

```csharp
private async Task<string> GetBuild(string apiKey, string characterName, GW2GameMode gameMode)
{
    GW2BuildParser parser = new GW2BuildParser(apiKey);

    APIBuild build = await parser.GetAPIBuildAsync(characterName, gameMode);

    parser.Dispose();

    return build.GetBuildCode();
}
```