﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueEncounter : MonoBehaviour
{
    public void Talk()
    {
        Dialogue dialogueInstance = Dialogue.ConstructDialogueBox();
        dialogueInstance.ParseMessage("Hey there. I'm Ariel.|" +
            "I don't have very much to say to you at the\nmoment.|" +
            "But I'm sure I'll have more to say in the final\ndemo." +
            "|'Later!");
    }
}
