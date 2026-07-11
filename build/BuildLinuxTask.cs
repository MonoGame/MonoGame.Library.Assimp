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
        var buildWorkingDir = "assimp/";
        context.StartProcessWithDocker("cmake", workingDirectory: buildWorkingDir, args: "-DASSIMP_BUILD_ZLIB=OFF -DASSIMP_BUILD_TESTS=OFF -DASSIMP_INSTALL=OFF CMakeLists.txt");
        context.StartProcessWithDocker("make", workingDirectory: buildWorkingDir, args: "");
        context.CopyFile(@"assimp/bin/libassimp.so", $"{context.ArtifactsDir}/libassimp.so");
    }
}
