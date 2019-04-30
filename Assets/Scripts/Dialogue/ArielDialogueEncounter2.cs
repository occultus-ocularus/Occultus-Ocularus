using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueEncounter2 : MonoBehaviour, IDialogueEncounter
{
    public TextAsset dialogueText;
    public Dialogue dialogueSetup;

    public SpriteRenderer samson;
    public LevelTransition fadeEffect;

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
        else
            Debug.Log("DialogAction: " + action);
    }

    public void DialogueFinished()
    {
        foreach (BoxCollider2D bc in GetComponents<BoxCollider2D>())
            bc.enabled = false;
    }
}
