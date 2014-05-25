pvc.Task("nuget-push", () => {
	pvc.Source("src/Pvc.MSBuild.csproj")
	   .Pipe(new PvcNuGetPack(
			createSymbolsPackage: true
	   ))
	   .Pipe(new PvcNuGetPush());
});