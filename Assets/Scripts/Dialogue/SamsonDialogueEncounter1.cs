using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter1 : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

    public bool moveWhenFinished = true;
    public AudioClip[] textBlips = new AudioClip[5];

    private AudioSource textBlip;

    void Start() {
        textBlip = GetComponent<AudioSource>();
    }

    public void Talk()
    {
        Dialogue dialogueInstance = dialogueSetup.ActivateDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage(dialogueText.ToString());
    }

    public void DialogueFinished()
    {
        if (moveWhenFinished) {
            foreach (BoxCollider2D bc in GetComponents<BoxCollider2D>())
                bc.enabled = false;
            GetComponent<MovingPlatform>().Extend();
        }

    }

    public void DialogueAction(string action)
    {
        Debug.Log("DialogAction: " + action);
    }

    public void PlayTextBlip(string characterName, float rate) {
        textBlip = gameObject.AddComponent<AudioSource>();
        textBlip.clip = textBlips[Random.Range(0, 4)];
        textBlip.Play();
        StartCoroutine(AudioFadeOut.FadeOut(textBlip, 4 * rate - 0.001f));
        Destroy(GetComponent<AudioSource>());
    }
}
