using System.Runtime.InteropServices;

namespace BuildScripts;

[TaskName("Build Linux")]
[IsDependentOn(typeof(PrepTask))]
[IsDependeeOf(typeof(BuildLibraryTask))]
public sealed class BuildLinuxTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.IsRunningOnLinux();

    public override void Run(BuildContext context)
    {
        context.StartProcess("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "CMakeLists.txt"});
        context.StartProcess("make", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = ""});
        context.CopyFile(@"assimp/bin/libassimp.so", $"{context.ArtifactsDir}/libassimp.so");
    }
}
