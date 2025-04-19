using System.Diagnostics;
using System.IO;
using UnityEditor;

namespace RemBug
{
public static class LocalHttpServerRunner
{
    private const string ProjectRelativePath = "Packages/com.gexetr.rembug/.Extern/Server/LocalHttpServer";
    private const string ExeName = "LocalHttpServer.exe";

    [MenuItem("Tools/GCL/Run Local HTTP Server")]
    public static void RunServer()
    {
        string projectPath = Path.Combine(Directory.GetCurrentDirectory(), ProjectRelativePath);
        string csprojPath = Path.Combine(projectPath, "LocalHttpServer.csproj");
        string exePath = Path.Combine(projectPath, "bin", "Release", ExeName);

        if (!File.Exists(csprojPath))
        {
            EditorUtility.DisplayDialog("Error", $"Project file not found:\n{csprojPath}", "OK");
            UnityEngine.Debug.LogError($"Project file not found:\n{csprojPath}");
            return;
        }

        if (File.Exists(exePath))
        {
            TryStartExe(exePath);
            return;
        }

        var buildProcess = new Process
        {
            StartInfo = new ProcessStartInfo
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
            UnityEngine.Debug.LogError("Build failed:\n" + error);
            return;
        }

        UnityEngine.Debug.Log("Build succeeded:\n" + output);

        if (!File.Exists(exePath))
        {
            UnityEngine.Debug.LogError($"Executable not found:\n{exePath}");
            return;
        }

        TryStartExe(exePath);
    }

    private static void TryStartExe(string exePath)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Verb = "runas", // run as administrator
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(exePath)
            };

            Process.Start(startInfo);
            UnityEngine.Debug.Log("Server started!");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to start LocalHttpServer:\n" + e.Message);
        }
    }
}
}