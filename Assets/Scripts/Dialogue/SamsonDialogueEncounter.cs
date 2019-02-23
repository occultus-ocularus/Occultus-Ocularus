using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsonDialogueEncounter : MonoBehaviour
{
    public Dialogue dialogueSetup;

    public void Talk()
    {
        Dialogue dialogueInstance = dialogueSetup.ConstructDialogueBox();
        dialogueInstance.ParseMessage("Hey, kid. Come here.|" +
            "You’re new, aren’t you?|" +
            "Neat. Mind fetching me a cup of water from the\nfountain " +
            "you just crawled out of?|Thanks.|You know, you look like you’ve " +
            "got potential.\nHow about I show you around?|" +
            "Don’t talk much, do you? That’s fine, most\nNewbies don’t. Hell, " +
            "I didn’t talk much at all until I was at least an Apprentice.|" +
            "Exactly. Okay, meet me outside — I’ll help you out.|" +
            "Oh, by the way, I’m Samson. If it matters.");
    }
}
