﻿using Microsoft.Build.Evaluation;
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
        private readonly bool enableParallelism = false;
        private readonly string toolsVersion = null;

        public PvcMSBuild(
            string buildTarget = "Build",
            string configurationName = "Debug",
            bool enableParallelism = false,
            string toolsVersion = "12.0")
        {
            this.buildTarget = buildTarget;
            this.configurationName = configurationName;
            this.enableParallelism = enableParallelism;
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
                    this.enableParallelism ? "" : "/m"
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
