using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public string nextScene;
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f;
    public PlayerController player;

    public enum FadeDirection { In, Out }

    private float fadeStartValue;
    private float fadeEndValue;
    private bool fadeCompleted;
    private bool loading;
    private bool fadeStarted;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        fadeOutUIImage.enabled = true;
        StartCoroutine(Fade(FadeDirection.Out));
    }

    public void FadeLoadScene()
    {
        loading = true;
        fadeCompleted = false;
        if (!fadeStarted)
            StartCoroutine(Fade(FadeDirection.In));
    }

    public void FadeAway(SpriteRenderer sr)
    {
        spriteRenderer = sr;
        StartCoroutine(FadeOut(sr));
    }

    public void FadeAppear(SpriteRenderer sr) {
        spriteRenderer = sr;
        sr.enabled = true;
        print(sr);
        StartCoroutine(FadeIn(sr));
    }

    private IEnumerator FadeOut(SpriteRenderer sr)
    {
        if (!fadeStarted)
        {
            fadeStartValue = 1;
            fadeEndValue = 0;
        }
        fadeStarted = true;

        while (fadeStartValue >= fadeEndValue)
        {
            SetTransparencySR(FadeDirection.Out);
            yield return null;
        }
        sr.enabled = false;
        fadeStarted = false;
    }

    private IEnumerator FadeIn(SpriteRenderer sr)
    {
        if (!fadeStarted) {
            fadeStartValue = 0;
            fadeEndValue = 1;
        }
        fadeStarted = true;

        while (fadeStartValue <= fadeEndValue) {
            SetTransparencySR(FadeDirection.In);
            yield return null;
        }
        fadeStarted = false;
    }

    private IEnumerator Fade(FadeDirection fadeDirection)
    {

        if (!fadeStarted)
        {
            if (fadeDirection == FadeDirection.Out)
            {
                fadeStartValue = 1;
                fadeEndValue = 0;
            }
            else
            {
                fadeStartValue = 0;
                fadeEndValue = 1;
            }
        }
        fadeStarted = true;
        if (player != null)
            player.canMove = false;

        if (fadeDirection == FadeDirection.Out)
        {
            while (fadeStartValue >= fadeEndValue)
            {
                SetTransparency(fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false;

        }
        else
        {
            fadeOutUIImage.enabled = true;
            while (fadeStartValue <= fadeEndValue)
            {
                SetTransparency(fadeDirection);
                yield return null;
            }
        }
        if (loading && !fadeCompleted)
            StartCoroutine(LoadScene());
        fadeCompleted = true;
        fadeStarted = false;
        if (!loading && player != null)
            player.canMove = true;
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        while (!asyncLoad.isDone)
            yield return null;
    }

    private void SetTransparency(FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, fadeStartValue);
        if (fadeDirection == FadeDirection.Out)
            fadeStartValue -= Time.deltaTime / fadeSpeed;
        else
            fadeStartValue += Time.deltaTime / fadeSpeed;
    }

    private void SetTransparencySR(FadeDirection fadeDirection)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeStartValue);
        if (fadeDirection == FadeDirection.Out)
            fadeStartValue -= Time.deltaTime / fadeSpeed;
        else
            fadeStartValue += Time.deltaTime / fadeSpeed;
    }
}