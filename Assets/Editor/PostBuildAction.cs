using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics;

public class PostBuildAction
{
    private static string externalAppPath = "C:\\Users\\Andriu\\AppData\\Local\\Citra\\canary\\citra-qt.exe";
    [PostProcessBuild(1)]
    public static void OnPostBuild(BuildTarget target, string pathToBuiltProject)
    {
        // Pregunta al usuario si desea ejecutar la aplicación externa
        bool shouldRunExternalApp = EditorUtility.DisplayDialog(
            "Ejecutar aplicación externa",
            "¿Quieres ejecutar la aplicación externa después de la build?",
            "Sí",
            "No"
        );

        if (shouldRunExternalApp)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = externalAppPath,
                Arguments = "\"" + pathToBuiltProject + "\"", // Pasa la ruta del ejecutable del build como argumento
                UseShellExecute = true
            };

            try
            {
                Process.Start(processInfo);
                UnityEngine.Debug.Log("Aplicación externa lanzada después del build.");
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Error al ejecutar la aplicación externa: " + e.Message);
            }
        }
        else
        {
            UnityEngine.Debug.Log("El usuario decidió no ejecutar la aplicación externa.");
        }
    }
}