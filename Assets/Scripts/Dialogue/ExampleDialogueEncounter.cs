using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDialogueEncounter : MonoBehaviour
{
    public Dialogue dialogueSetup;

    public void Talk()
    {
        Dialogue dialogueInstance = dialogueSetup.ConstructDialogueBox(); ;
        dialogueInstance.ParseMessage("This is a test.|This is the second phrase.|This is the third phrase.");
    }
}
