using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameEditor.Debugging
{
    public class CheckHttpSettingPostBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.WebGL)
                return;

            var allowHttp = PlayerSettings.insecureHttpOption != InsecureHttpOption.NotAllowed;
            if (!allowHttp)
            {
                Debug.LogWarning("[HTTP Warning] 🚫 'Allow HTTP' is disabled in Player Settings. HTTP requests will fail at runtime. Enable it in Player → Other Settings → Allow 'HTTP' connections.");
            }
            else
            {
                Debug.Log("[HTTP Check] ✅ 'Allow HTTP' is enabled in Player Settings.");
            }
        }
    }
}