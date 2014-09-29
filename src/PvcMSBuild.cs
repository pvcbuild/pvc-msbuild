using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using PvcCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace PvcPlugins
{
    public class PvcMSBuild : PvcPlugin
    {
        private readonly string buildTarget = null;
        private readonly string configurationName = null;
        private readonly string defineConstants = null;
        private readonly bool enableParallelism = false;
        private readonly string outputPath = null;
        private readonly string targetFrameworkVersion = null;
        private readonly string toolsVersion = null;

        public PvcMSBuild(
            string buildTarget = "Build",
            string configurationName = "Debug",
            string defineConstants = "",
            bool enableParallelism = false,
            string outputPath = @"bin\Debug",
            string targetFrameworkVersion = "v4.5",
            string toolsVersion = "12.0")
        {
            this.buildTarget = buildTarget;
            this.configurationName = configurationName;
            this.defineConstants = defineConstants;
            this.enableParallelism = enableParallelism;
            this.outputPath = outputPath;
            this.targetFrameworkVersion = targetFrameworkVersion;
            this.toolsVersion = toolsVersion;
        }

        public override string[] SupportedTags
        {
            get
            {
                return new[] { ".csproj", ".vbproj", ".vcxproj", ".proj", ".sln" };
            }
        }

        public override IEnumerable<PvcStream> Execute(IEnumerable<PvcStream> inputStreams)
        {
            var msBuildPath = ToolLocationHelper.GetPathToBuildToolsFile("msbuild.exe", this.toolsVersion, DotNetFrameworkArchitecture.Current);

            foreach (var projectSolutionStream in inputStreams)
            {
                var workingDirectory = Path.GetDirectoryName(projectSolutionStream.StreamName);
                var args = new [] {
                    "/target:" + this.buildTarget,
                    "/property:Configuration=" + this.configurationName,
                    "/verbosity:minimal",
                    this.enableParallelism ? "" : "/m",
                    "/property:TargetFrameworkVersion=" + this.targetFrameworkVersion,
                    "/property:OutputPath=" + this.outputPath,
                    (string.IsNullOrWhiteSpace(this.defineConstants) ? "" : "/property:DefineConstants=" + this.defineConstants)
                };

                var resultStreams = PvcUtil.StreamProcessExecution(msBuildPath, workingDirectory, args);

                // TODO - Implement plugin artifact generation to make this information available for logging in other plugins
                // blocking read until end
                new StreamReader(resultStreams.Item1).ReadToEnd();
            }

            return new PvcStream[] { };
        }
    }
}

