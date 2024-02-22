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
        context.StartProcess("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "-DASSIMP_BUILD_TESTS=OFF -DASSIMP_INSTALL=OFF -DCMAKE_OSX_ARCHITECTURES=\"x86_64;arm64\" CMakeLists.txt"});
        context.StartProcess("make", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = ""});
        context.CopyFile(@"assimp/bin/libassimp.5.3.0.dylib", $"{context.ArtifactsDir}/libassimp.dylib");
    }
}
