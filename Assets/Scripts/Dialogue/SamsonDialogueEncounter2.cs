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

    //private bool lightsOut;

    private AudioSource textBlip;
    private enable_lights foregroundDec;

    void Start() {
        textBlip = GetComponent<AudioSource>();
        foregroundDec = GameObject.Find("Foreground Decorations").GetComponent<enable_lights>();
    }

    /*private void Update()
    {
        if (lightsOut && cityLight.intensity >= 8)
            cityLight.intensity -= 1f;
    }*/

    public void Talk()
    {
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
        if (action.Equals("Lights go out"))
            foregroundDec.SetLightLevel(0);
        else
             Debug.Log("DialogAction: " + action);
    }

    public void PlayTextBlip(string characterName, float rate) {
        textBlip.clip = textBlips[Random.Range(0, 4)];
        textBlip.Play();
        StartCoroutine(AudioFadeOut.FadeOut(textBlip, 4 * rate - 0.001f));
    }
}
