using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueEncounter2 : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

    public SpriteRenderer samson;
    public LevelTransition fadeEffect;

    public AudioClip[] textBlips = new AudioClip[5];
    public AudioClip[] samsonTextBlips = new AudioClip[5];

    private AudioSource textBlip;
    private ParticleSystem playerParticles;

    void Start() {
        textBlip = GetComponent<AudioSource>();
        playerParticles = GameObject.Find("Player").GetComponent<ParticleSystem>();
    }

    public void Talk()
    {
        Dialogue dialogueInstance = dialogueSetup.ActivateDialogueBox();
        dialogueInstance.Setup(this);
        dialogueInstance.ParseMessage(dialogueText.ToString());
    }

    public void DialogueAction(string action)
    {
        if (action.Equals("Samson appears"))
            fadeEffect.FadeAppear(samson);
        else if (action.Equals("Samson disappears"))
            fadeEffect.FadeAway(samson);
        else if (action.Equals("Player grows eyes on shoulders") || action.Equals("Player grows new set of eyes, on knees"))
            playerParticles.Play();
        else
            Debug.Log("DialogAction: " + action);
    }

    public void DialogueFinished()
    {
        foreach (BoxCollider2D bc in GetComponents<BoxCollider2D>())
            bc.enabled = false;
    }

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
