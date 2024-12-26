using UnityEngine;
using UnityEngine.UI;

public class ConsoleToUI : MonoBehaviour
{
    [SerializeField]
    private Text consoleText; // Asigna aquí el componente Text del UI en el Inspector.

    private string logMessages = ""; // Almacena los mensajes de la consola.

    void OnEnable()
    {
        // Suscribirse al evento de mensajes de consola.
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Desuscribirse del evento cuando el objeto se desactiva.
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Agrega el mensaje al texto acumulado.
        logMessages += type+": " +logString+"\n";

        // Opcional: Agrega detalles de seguimiento de pila si es un error.
        if (type == LogType.Error || type == LogType.Exception)
        {
            logMessages += stackTrace;
            logMessages += "\n";
        }

        // Limitar el tamaño del texto si es muy largo.
        if (logMessages.Length > 5000)
        {
            logMessages = logMessages.Substring(logMessages.Length - 5000);
        }

        // Actualiza el componente de texto en la interfaz de usuario.
        if (consoleText != null)
        {
            consoleText.text = logMessages;
        }
    }
}