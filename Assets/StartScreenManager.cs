using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState
{
    ToStart,
    Started,
    Over,
    Complete
}

public class StartScreenManager : MonoBehaviour
{
    public GameObject startScreenPanel;
    public TextMeshProUGUI pressSpaceText, mainMessageText, gameOverMessageText, gameCompleteMessageText;
    public CanvasGroup panelCanvasGroup;

    private GameState gameState = GameState.ToStart;
    private MusicManager musicManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicManager = FindFirstObjectByType<MusicManager>();

        ShowStartScreen(GameState.ToStart);
    }

    // Update is called once per frame    
    void Update()
    {
        if (gameState == GameState.Started) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Restart game if gameover or complete, otherwise just fade out
            if (gameState != GameState.ToStart)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else StartCoroutine(FadeAndStart());
        }

        BlinkSpaceText();
    }

    private void SetTextVisibility(TextMeshProUGUI text, bool isVisible)
    {
        Color c = pressSpaceText.color;
        c.a = isVisible ? 1f : 0f;
        pressSpaceText.color = c;

        text.color = c;
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

    public void ShowStartScreen(GameState state)
    {
        gameState = state;

        // This here freezes game execution
        Time.timeScale = 0f;
        startScreenPanel.SetActive(true);
        panelCanvasGroup.alpha = 1f;

        if (gameState == GameState.Over)
        {
            SetTextVisibility(mainMessageText, false);
            SetTextVisibility(gameCompleteMessageText, false);
            SetTextVisibility(gameOverMessageText, true);

            // Play death music
            musicManager.PlayDeathMusic();
        }
        else if (gameState == GameState.Complete)
        {
            SetTextVisibility(gameOverMessageText, false);
            SetTextVisibility(mainMessageText, false);
            SetTextVisibility(gameCompleteMessageText, true);

            // Play death music
            musicManager.PlayWinMusic();
        }
        else
        {
            SetTextVisibility(gameOverMessageText, false);
            SetTextVisibility(gameCompleteMessageText, false);
            SetTextVisibility(mainMessageText, true);

            // Stop all music
            musicManager.StopMusic();
        }

        // This is a bit nasty to have here, in theory there should be separate entities
        // in the hierarchy and we just toggle their visibility, but for the sake of simplicity
        // I'll stick to this solution
        pressSpaceText.text = gameState != GameState.ToStart ? "Press SPACE to restart." : "Press SPACE to start.";
    }

    System.Collections.IEnumerator FadeAndStart()
    {
        gameState = GameState.Started;

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

        // Start game music
        musicManager.PlayGameMusic();
    }
}
