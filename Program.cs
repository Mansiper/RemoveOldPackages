var profilePath = Environment.GetEnvironmentVariable("USERPROFILE");

foreach (var packadge in Directory.EnumerateDirectories($"{profilePath}\\.nuget\\packages"))
{
    var versionsDirs = Directory.EnumerateDirectories(packadge).ToList();
    var versions = versionsDirs.Select(e => (e, GetVersion(e))).ToList();
    if (versions.Count == 1 || versions.Any(v => v.Item2 == 0))
        continue;
    var maxVersionDir = versions.MaxBy(e => e.Item2).Item1;

    foreach (var dir in versionsDirs.Where(dir => dir != maxVersionDir))
        try
        {
            Console.WriteLine($"Delete: {dir}");
            Directory.Delete(dir, true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    Console.WriteLine($"Saved: {maxVersionDir}");
}

return;

static long GetVersion(string path)
{
    //C:\Users\{username}\.nuget\packages\{package}\1.1.0-beta.4
    var last = path.Split('\\').Last();
    var version = last.Split('-').First();
    var nums = version.Split('.');
    if (nums.Length != 3)
    {
        Console.WriteLine($"Wrong version: {path}");
        return 0;
    }
    return int.Parse(nums[0]) * 1000000 + int.Parse(nums[1]) * 1000 + int.Parse(nums[2]);
}