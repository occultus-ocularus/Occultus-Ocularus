using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    [Tooltip("Scene to be transitioned to")]
    public string nextScene;
    [Tooltip("UI image that will bridge the transition")]
    public Image fadeOutUIImage;
    [Tooltip("Audio clip to play instead of the level music during the " +
        "transition")]
    public AudioClip transitionSound;

    [HideInInspector]
    public float fadeSpeed = 0.8f;

    public enum FadeDirection { In, Out }

    private float fadeStartValue;
    private float fadeEndValue;
    private bool fadeCompleted;
    private bool loading;
    private bool fadeStarted;
    private SpriteRenderer spriteRenderer;

    private PlayerController player;
    private AudioSource musicPlayer;

    // Fade out a screen transition image and get the music
    // player AudioSource to be used when exiting the scene
    void Start()
    {
        if (GameObject.Find("Player") != null)
            player =
                GameObject.Find("Player").GetComponent<PlayerController>();
        if (GameObject.Find("MusicPlayer") != null)
            musicPlayer =
                GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        fadeOutUIImage.enabled = true;
        StartCoroutine(Fade(FadeDirection.Out));
    }

    // Fade in a screen transition image, then load next scene
    public void FadeLoadScene()
    {
        // Stop music and play level transition sound
        if (musicPlayer != null) {
            musicPlayer.clip = transitionSound;
            musicPlayer.Play();
        }

        // Start coroutine Fade, which in turn starts LoadScene
        loading = true;
        fadeCompleted = false;
        if (!fadeStarted)
            StartCoroutine(Fade(FadeDirection.In));
    }

    // Fade out a SpriteRenderer as it disappears
    public void FadeAway(SpriteRenderer sr)
    {
        spriteRenderer = sr;
        StartCoroutine(Fade(FadeDirection.Out, sr));
    }

    // Fade in a SpriteRenderer as it appears
    public void FadeAppear(SpriteRenderer sr) {
        spriteRenderer = sr;
        sr.enabled = true;
        print(sr);
        StartCoroutine(Fade(FadeDirection.In, sr));
    }

    // Coroutine to fade an Image or SpriteRenderer
    private IEnumerator Fade(FadeDirection direction, SpriteRenderer sr = null)
    {
        // Set start and end values if just beginning to fade
        if (!fadeStarted) {
            if (direction == FadeDirection.Out)
                fadeStartValue = 1;
            else
                fadeStartValue = 0;
            fadeEndValue = 1 - fadeStartValue;
        }
        fadeStarted = true;

        // Freeze player movement during scene transition fade
        if (loading && player != null) {
            player.canMove = false;
        }

        // Continue to fade in or out until done
        if (direction == FadeDirection.Out) {
            while (fadeStartValue >= fadeEndValue) {
                if (sr == null)
                    SetTransparencyImage(FadeDirection.Out);
                else
                    SetTransparencySR(FadeDirection.Out);
                yield return null;
            }

            // Disable Image/SR once it has disappeared 
            if (sr == null)
                fadeOutUIImage.enabled = false;
            else
                sr.enabled = false;
        }
        else {
            // Enable Image/SR before it appears
            if (sr == null)
                fadeOutUIImage.enabled = true;
            else
                sr.enabled = true;
            while (fadeStartValue <= fadeEndValue) {
                if (sr == null)
                    SetTransparencyImage(FadeDirection.In);
                else
                    SetTransparencySR(FadeDirection.In);
                yield return null;
            }
        }

        // Load next scene once fade is complete if in a load transition
        if (sr == null && loading && !fadeCompleted)
            StartCoroutine(LoadScene());

        fadeCompleted = true;
        fadeStarted = false;
    }

    // Coroutine to load a scene
    IEnumerator LoadScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        while (!asyncLoad.isDone)
            yield return null;
    }

    // Helper function for setting transparency on an Image (UI element)
    private void SetTransparencyImage(FadeDirection fadeDirection) {
        fadeOutUIImage.color = new Color(
            fadeOutUIImage.color.r,
            fadeOutUIImage.color.g,
            fadeOutUIImage.color.b,
            fadeStartValue
            );
        if (fadeDirection == FadeDirection.Out)
            fadeStartValue -= Time.deltaTime / fadeSpeed;
        else
            fadeStartValue += Time.deltaTime / fadeSpeed;
    }

    // Helper function for setting transparency on a SpriteRenderer
    private void SetTransparencySR(FadeDirection fadeDirection) {
        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            fadeStartValue
            );
        if (fadeDirection == FadeDirection.Out)
            fadeStartValue -= Time.deltaTime / fadeSpeed;
        else
            fadeStartValue += Time.deltaTime / fadeSpeed;
    }
}