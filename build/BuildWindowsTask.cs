using Cake.Common.Tools.MSBuild;

namespace BuildScripts;

[TaskName("Build Windows")]
[IsDependentOn(typeof(PrepTask))]
[IsDependeeOf(typeof(BuildLibraryTask))]
public sealed class BuildWindowsTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.IsRunningOnWindows();

    public override void Run(BuildContext context)
    {
        var buildWorkingDir = "assimp/";
        context.StartProcess("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "-DASSIMP_BUILD_TESTS=OFF -DASSIMP_INSTALL=OFF CMakeLists.txt" });
        context.ReplaceTextInFiles("assimp/code/assimp.vcxproj", "MultiThreadedDLL", "MultiThreaded");
        context.ReplaceTextInFiles("assimp/contrib/zlib/zlibstatic.vcxproj", "MultiThreadedDLL", "MultiThreaded");
        context.StartProcess("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "--build . --config release" });
        context.CopyFile(@"assimp/bin/Release/assimp-vc143-mt.dll", $"{context.ArtifactsDir}/assimp.dll");
    }
}
