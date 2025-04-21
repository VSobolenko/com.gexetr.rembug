using System.IO;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace RemBug
{
public static class LocalHttpServerRunner
{
    private const string ExeName = "LocalHttpServer.exe";

    [MenuItem("Tools/GCL/Run Local HTTP Server")]
    public static void RunServer()
    {
        if (IsPackageFound("Packages/com.gexetr.rembug/Editor/Gexetr.RemBug.Editor.asmdef", out var packageInfo) == false)
            return;
        string projectPath = Path.Combine(packageInfo.resolvedPath, ".Extern/Server/LocalHttpServer");
        string csprojPath = Path.Combine(projectPath, "LocalHttpServer.csproj");
        string exePath = Path.Combine(projectPath, "bin", "Release", ExeName);

        if (!File.Exists(csprojPath))
        {
            EditorUtility.DisplayDialog("Error", $"Project file not found:\n{csprojPath}", "OK");
            Debug.LogError($"Project file not found:\n{csprojPath}");
            return;
        }

        if (File.Exists(exePath))
        {
            TryStartExe(exePath);
            return;
        }

        var buildProcess = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{csprojPath}\" -c Release",
                WorkingDirectory = projectPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        buildProcess.Start();
        string output = buildProcess.StandardOutput.ReadToEnd();
        string error = buildProcess.StandardError.ReadToEnd();
        buildProcess.WaitForExit();

        if (buildProcess.ExitCode != 0)
        {
            Debug.LogError("Build failed:\n" + error);
            return;
        }

        Debug.Log("Build succeeded:\n" + output);

        if (!File.Exists(exePath))
        {
            Debug.LogError($"Executable not found:\n{exePath}");
            return;
        }

        TryStartExe(exePath);
    }

    private static void TryStartExe(string exePath)
    {
        try
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = exePath,
                Verb = "runas", // run as administrator
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(exePath)
            };

            System.Diagnostics.Process.Start(startInfo);
            Debug.Log("Server started!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to start LocalHttpServer:\n" + e.Message);
        }
    }
    
    private static bool IsPackageFound(string packageAnchor, out PackageInfo packageInfo)
    {
        packageInfo = PackageInfo.FindForAssetPath(packageAnchor);
        if (packageInfo == null)
            Debug.LogWarning($"[RemBug] Package not found in path: " + packageAnchor);

        return packageInfo != null;
    }
}
}