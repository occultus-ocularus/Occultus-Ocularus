using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueEncounter : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

    public AudioClip[] textBlips = new AudioClip[5];
    public AudioClip[] samsonTextBlips = new AudioClip[5];

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

    public void DialogueAction(string action)
    {
        Debug.Log("DialogAction: " + action);
    }

    public void DialogueFinished() {}

    public void PlayTextBlip(string characterName, float rate) {
        textBlip = gameObject.AddComponent<AudioSource>();
        if (characterName.Equals("ARIEL") || characterName.Equals("???"))
            textBlip.clip = textBlips[Random.Range(0, 4)];
        else if (characterName.Equals("SAMSON"))
            textBlip.clip = samsonTextBlips[Random.Range(0, 4)];
        textBlip.Play();
        StartCoroutine(AudioFadeOut.FadeOut(textBlip, 4 * rate - 0.001f));
        Destroy(GetComponent<AudioSource>());
    }
}
