using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f;
    private Text textComponent;
    private Color originalColor;
    private float fadeTimer;
    private System.Action onFadeComplete;
    private bool isFading;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    public void Run(System.Action fadeCompleteCallback)
    {
        if (textComponent == null)
        {
            return;
        }

        isFading = true;
        fadeTimer = 0f;
        originalColor = textComponent.color;
        onFadeComplete = fadeCompleteCallback;
    }

    private void Update()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;
        if (fadeTimer >= fadeDuration)
        {
            isFading = false;
            if (onFadeComplete != null) // Evitar operador '?.'
            {
                onFadeComplete.Invoke();
            }
        }
        else
        {
            float alpha = Mathf.Lerp(originalColor.a, 0, fadeTimer / fadeDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
    }
}