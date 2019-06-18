using UnityEngine;
using System.Collections;

public static class AudioFadeOut {

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
        float startTime = FadeTime;


        yield return new WaitForSeconds(FadeTime * 0.5f);

        FadeTime *= 0.5f;

        while (audioSource != null && audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        if (audioSource != null) {
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }
}