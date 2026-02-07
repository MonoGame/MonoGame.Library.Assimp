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
        BuildForArchitecture(context, "x64", "win-x64");
        BuildForArchitecture(context, "ARM64", "win-arm64");
    }

    private void BuildForArchitecture(BuildContext context, string cmakeArch, string rid)
    {
        var buildWorkingDir = $"assimp/build_{rid}";
        context.CreateDirectory(buildWorkingDir);
        
        context.StartProcess("cmake", new ProcessSettings { 
            WorkingDirectory = buildWorkingDir, 
            Arguments = $"-A {cmakeArch} -DASSIMP_BUILD_TESTS=OFF -DASSIMP_INSTALL=OFF .." 
        });
        
        context.ReplaceTextInFiles($"{buildWorkingDir}/code/assimp.vcxproj", "MultiThreadedDLL", "MultiThreaded");
        context.ReplaceTextInFiles($"{buildWorkingDir}/contrib/zlib/zlibstatic.vcxproj", "MultiThreadedDLL", "MultiThreaded");
        
        context.StartProcess("cmake", new ProcessSettings { 
            WorkingDirectory = buildWorkingDir, 
            Arguments = "--build . --config release" 
        });
        
        context.CreateDirectory($"{context.ArtifactsDir}/{rid}");
        context.CopyFile($@"{buildWorkingDir}/bin/Release/assimp-vc143-mt.dll", $"{context.ArtifactsDir}/{rid}/assimp.dll");
    }
}
