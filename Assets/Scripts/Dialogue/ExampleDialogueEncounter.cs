using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDialogueEncounter : MonoBehaviour
{
    void Start()
    {
        Dialogue dialogueInstance = Dialogue.ConstructDialogueBox();
        dialogueInstance.ParseMessage("This is a test.|This is the second phrase.|This is the third phrase.");
    }
}
