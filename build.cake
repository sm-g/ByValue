var _target = Argument("target", "Default");
var _configuration = Argument("configuration", "Release");
var _versionSuffix = BuildVersionSuffix();

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var SRC_DIR = PROJECT_DIR + "src/";
var TEST_DIR = PROJECT_DIR + "test/";
var ARTIFACTS_DIR = PROJECT_DIR + "artifacts/";
var PACKAGES_DIR = ARTIFACTS_DIR + "packages/";
var TEST_RESULTS_DIR = ARTIFACTS_DIR + "test-results/";
var CSPROJ = SRC_DIR + "ByValue/ByValue.csproj";

string BuildVersionSuffix() {
    var buildNumber =
        HasArgument("BuildNumber") ? Argument("BuildNumber", "0") :
        AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number.ToString() :
        TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.BuildNumber.ToString() :
        EnvironmentVariable("BUILD_NUMBER") ?? "localbuild";

    if (!BuildSystem.IsRunningOnAppVeyor)
        return buildNumber;

    var tag = AppVeyor.Environment.Repository.Tag;
    if (tag.IsTag)
        return null; // only version prefix used for NuGet.org

    var dbgSuffix = _configuration == "Debug" ? "-dbg" : "";
    var result = "ci-" + buildNumber + dbgSuffix;

    var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
    var branch = AppVeyor.Environment.Repository.Branch;

    if (isPullRequest)
        result += "-pr-" + AppVeyor.Environment.PullRequest.Number;
    else
        result += "-" + System.Text.RegularExpressions.Regex.Replace(branch, "[^0-9A-Za-z-]+", "-");

    // Nuget limits
    if (result.Length > 20)
        result = result.Substring(0, 20);

    return result;
}

Setup(context =>
{
    var versionPrefix = XmlPeek("Directory.Build.props", "//VersionPrefix");
    var packageVersion = _versionSuffix == null ? versionPrefix : versionPrefix + "-" + _versionSuffix;

    Information("Building {0} version {1}", _configuration, packageVersion);
});

Task("Clean")
    .Does(() => {
        CleanDirectory(ARTIFACTS_DIR);

        // dotnet clean does not work on root dir: https://github.com/dotnet/cli/issues/7240
        var projects = GetFiles("./**/*.csproj");
        foreach (var project in projects)
        {
            DotNetCoreClean(project.FullPath,
                new DotNetCoreCleanSettings() {
                    Configuration = _configuration
                }
            );
        }
    });

Task("Build")
    .Does(() => {
        DotNetCoreBuild(".",
            new DotNetCoreBuildSettings() {
                Configuration = _configuration
            }
        );
    });

Task("Test")
    .Does(() => {
        var testProjects = GetFiles("test/**/*.Tests.csproj").ToList();
        for(int i = 0; i < testProjects.Count; i++) {
            var testProject = testProjects[i];
            var testResultsFileName = $"test-result-{i+1}.xml";

            // wait nunit support for xml results https://github.com/nunit/nunit3-vs-adapter/issues/323
            // use NunitXml.TestLogger at now

            DotNetCoreTest(
                testProject.FullPath,
                new DotNetCoreTestSettings() {
                    Configuration = _configuration,
                    NoBuild = true,
                    NoRestore = true,
                    Logger = $"nunit;LogFilePath={TEST_RESULTS_DIR + testResultsFileName}"
                }
            );
        }
    })
    .Finally(() =>
    {
        if (BuildSystem.IsRunningOnAppVeyor)
        {
            Information("Uploading test results...");
            var testResults = GetFiles(TEST_RESULTS_DIR + "test-result*").ToList();
            foreach (var file in testResults)
            {
                AppVeyor.UploadTestResults(file.FullPath, AppVeyorTestResultsType.NUnit);
            }
        }
    });

Task("Package")
    .Does(() => {
        DotNetCorePack(CSPROJ,
            new DotNetCorePackSettings {
                Configuration = _configuration,
                NoBuild = true,
                NoRestore = true,
                OutputDirectory = PACKAGES_DIR,
                VersionSuffix = _versionSuffix
            }
        );
    });

Task("Publish-MyGet")
    .Does(() =>
    {
        var apiKey = EnvironmentVariable("MYGET_API_KEY");
        var apiUrl = EnvironmentVariable("MYGET_API_URL");
        if(string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiUrl)) {
            Information("Could not resolve key and url for MyGet.");
            return;
        }

        DotNetCoreNuGetPush($"{PACKAGES_DIR}*.nupkg",
            new DotNetCoreNuGetPushSettings {
                WorkingDirectory = PACKAGES_DIR,
                Source = EnvironmentVariable("MYGET_API_URL"),
                ApiKey = EnvironmentVariable("MYGET_API_KEY"),
            }
        );
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Travis")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    ;

Task("AppVeyor")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
    .IsDependentOn("Publish-MyGet")
    ;

RunTarget(_target);