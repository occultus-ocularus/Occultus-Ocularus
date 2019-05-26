using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDialogueEncounter : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

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

    public void DialogueFinished() {}

    public void DialogueAction(string action)
    {
        Debug.Log("DialogAction: " + action);
    }

    public void PlayTextBlip(string characterName) {
        textBlip.clip = textBlips[Random.Range(0, 4)];
    }

}
