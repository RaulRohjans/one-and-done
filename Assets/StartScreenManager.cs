using UnityEngine;
using UnityEngine.InputSystem;

public class StartScreenManager : MonoBehaviour
{
    public GameObject startScreenPanel;
    public TMPro.TextMeshProUGUI pressSpaceText;
    public CanvasGroup panelCanvasGroup;

    private bool gameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // When the game starts, we want to freeze it until player
        // presses space
        Time.timeScale = 0f;
        startScreenPanel.SetActive(true);
        panelCanvasGroup.alpha = 1f;
    }

    // Update is called once per frame    
    void Update()
    {
        if (gameStarted) return;

        BlinkSpaceText();

        // When the user presses SPACE we can go with a nice fade out
        // animation to make it smoother
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            StartCoroutine(FadeAndStart());
    }

    private void BlinkSpaceText()
    {
        // To make the text blink, we can make it toggle its visibility
        // through the color in the alpha value

        float blinkSpeed = 2f;
        float alpha = (Mathf.Sin(Time.unscaledTime * blinkSpeed) + 1f) / 2f;

        Color c = pressSpaceText.color;
        c.a = alpha;
        pressSpaceText.color = c;
    }

    System.Collections.IEnumerator FadeAndStart()
    {
        gameStarted = true;

        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);

            // Wait for the next frame to continue to add smoothness
            // to the fade out animation. This is a really nice thing
            // in unity
            yield return null;
        }

        startScreenPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
