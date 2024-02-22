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
        //  Disable openmp so there is no dependency on VCOMP140.dll
        context.StartProcess("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "-DASSIMP_BUILD_TESTS=OFF -DASSIMP_INSTALL=OFF CMakeLists.txt"});
        context.StartProcess("make", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = ""});
        context.CopyFile("assimp/bin/assimp.dll", $"{context.ArtifactsDir}/assimp.dll");
    }
}
