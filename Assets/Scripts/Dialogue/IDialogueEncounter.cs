using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueEncounter
{
    void Talk();
    void DialogueFinished();
    void DialogueAction(string action);
    void PlayTextBlip(string characterName,float rate);
}
