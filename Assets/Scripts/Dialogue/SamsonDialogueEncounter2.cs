using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter2 : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

    public LevelTransition levelTransition;
    //public Light cityLight;
    public AudioClip[] textBlips = new AudioClip[5];

    private bool talkedToSamson;
    //private bool lightsOut;

    private AudioSource textBlip;

    void Start() {
        textBlip = GetComponent<AudioSource>();
    }

    /*private void Update()
    {
        if (lightsOut && cityLight.intensity >= 8)
            cityLight.intensity -= 1f;
    }*/

    public void Talk()
    {
        talkedToSamson = true;
        Dialogue dialogueInstance = dialogueSetup.ActivateDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage(dialogueText.ToString());
    }

    public void DialogueFinished()
    {
        levelTransition.FadeAway(GetComponent<SpriteRenderer>());
        foreach (BoxCollider2D bc in GetComponents<BoxCollider2D>())
            bc.enabled = false;
    }

    public void DialogueAction(string action)
    {
        /*if (action.Equals("Lights go out"))
            lightsOut = true;
        else*/
            Debug.Log("DialogAction: " + action);
    }

    public void PlayTextBlip(string characterName) {
        textBlip.clip = textBlips[Random.Range(0, 4)];
        textBlip.Play();
    }
}
