using System.Runtime.InteropServices;

namespace BuildScripts;

[TaskName("Build macOS")]
[IsDependentOn(typeof(PrepTask))]
[IsDependeeOf(typeof(BuildLibraryTask))]
public sealed class BuildMacOSTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.IsRunningOnMacOs();

    public override void Run(BuildContext context)
    {
        var buildWorkingDir = "assimp/";
        context.StartProcess("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "CMakeLists.txt"});
        context.StartProcess("make", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = ""});
        context.CopyFile(@"assimp/bin/libassimp.5.3.0.dylib", $"{context.ArtifactsDir}/libassimp.dylib");
    }
}
