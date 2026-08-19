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
        context.StartProcessWithDocker("cmake", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "-DASSIMP_BUILD_TESTS=OFF -DASSIMP_INSTALL=OFF CMakeLists.txt" });
        context.StartProcessWithDocker("make", new ProcessSettings { WorkingDirectory = buildWorkingDir, Arguments = "" });
        var artifactPath = $"{context.ArtifactsDir}/libassimp.so";
        context.CopyFile(@"assimp/bin/libassimp.so", artifactPath);

        var stripArguments = new ProcessArgumentBuilder();
        stripArguments.Append("--strip-unneeded");
        stripArguments.AppendQuoted(artifactPath);
        context.StartProcessWithDocker("strip", new ProcessSettings { WorkingDirectory = "", Arguments = stripArguments });
    }
}
