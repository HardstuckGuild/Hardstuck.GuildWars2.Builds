# Hardstuck.GuildWars2.Builds

[![Nuget](https://img.shields.io/nuget/v/Hardstuck.GuildWars2.Builds?style=for-the-badge)](https://www.nuget.org/packages/Hardstuck.GuildWars2.Builds/)

GW2 build code & link generator for Hardstuck Builds ([builds.hardstuck.gg](https://builds.hardstuck.gg)).

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

### Quickly generate a build link:

```csharp
try
{
    using (GW2BuildParser parser = new GW2BuildParser("My API key"))
    {
        APIBuild build = await parser.GetAPIBuildAsync("My Amazing Character", GameMode.PvE);
        Console.WriteLine(build.GetBuildLink());
    }
}
catch (NotEnoughPermissionsException e)
{
    Console.Write("The API request failed due to low API key permissions, main reason: ");
    switch(e.MissingPermission)
    {
        case NotEnoughPermissionsReason.Characters:
            Console.WriteLine("the API key is missing \"characters\" permission");
            break;
        case NotEnoughPermissionsReason.Builds:
            Console.WriteLine("the API key is missing \"builds\" permission");
            break;
        default:
            Console.WriteLine("the API key is invalid");
            break;
    }
}
```

Note that `GW2BuildParser` implements `IDisposable`, henceforth `Dispose()` method or `using` statement is required to release resources held by the class.

Additionally, `GW2BuildParser` can throw an exception of type `NotEnoughPermissionsException`, which should be properly handled. You can use a second optional parameter for the `GW2BuildParser` constructor to disable API key check.

---

### Example method to extract the build link with `using` statement:

```csharp
private async Task<string> GetBuildLinkAsync(string apiKey, string characterName, GameMode gameMode)
{
    try
    {
        using (GW2BuildParser parser = new GW2BuildParser(apiKey))
        {
            APIBuild build = await parser.GetAPIBuildAsync(characterName, gameMode);
            return build.GetBuildLink();
        }
    }
    catch (NotEnoughPermissionsException e)
    {
        throw;
    }
}
```

### Alternative way with `Dispose()`:

```csharp
private async Task<string> GetBuildLinkAsync(string apiKey, string characterName, GameMode gameMode)
{
    try
    {
        GW2BuildParser parser = new GW2BuildParser(apiKey);

        APIBuild build = await parser.GetAPIBuildAsync(characterName, gameMode);

        parser.Dispose();

        return build.GetBuildLink();
    }
    catch (NotEnoughPermissionsException e)
    {
        throw;
    }
}
```
