pvc-msbuild
===========

PVC Plugin to execute MSBuild against solution files and project files. Multiple projects and/or solutions can be streamed into the plugin. They will be run in the order passed.

```
/// Execute a build with the default options:
///   buildTarget: "Build"
///   configuration: "Debug"
///   defineConstants: ""
///   enableParallelism: false
///   outputPath: @"bin\Debug"
///   targetFrameworkVersion: "v4.5"
///   toolsVersion: "12.0"
pvc.Source("SolutionFile.sln")
   .Pipe(new PvcMSBuild());

/// Execute a build of multiple solutions using
/// named parameters overriding default values
pvc.Source("SolutionFile.sln", "OtherSolutionFile.sln")
   .Pipe(new PvcMSBuild(
        buildTarget: "Clean;Build"
        enableParallelism: true
   ))
```
