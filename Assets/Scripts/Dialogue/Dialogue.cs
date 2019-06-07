using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour, IDialogActions {
    public PlayerInputMapping playerInput;
    
    public static bool messageComplete = true;

    const char SPLIT_SYMBOL = '|';
    const float NORMAL_SCROLL_RATE = 0.02f;
    const float FAST_SCROLL_RATE = 0.01f;

    public Text characterName;
    public Text text;
    public PlayerController player;
    public GameObject dialogueBox;
    public Color samsonColor;
    public Color arielColor;

    private string[] phrases;
    private string[] actions;
    private bool actionPerformed;
    private bool foundName;

    private IDialogueEncounter dialogueEncounter;
    private bool playerPositionSet;
    private Transform arielOrSamsonTransform;
    private float lerpTimer;

    private float lastUpdateTime;
    private float currentScrollRate;

    private int phraseIndex;
    private int charIndex;
    private bool awaitingUser;
    private bool skipToEndOfPhrase;

    public void Start() {
        playerInput.Dialog.SetCallbacks(this);
    }
    public void Setup(IDialogueEncounter de) {
        BeginDialog(de);
    }
    public void BeginDialog(IDialogueEncounter de) {
        player.EnterUIOrDialog();
        player.body.velocity = Vector2.zero;
        dialogueEncounter = de;
        lastUpdateTime = Time.time;
        currentScrollRate = NORMAL_SCROLL_RATE;
        phraseIndex = 0;
        charIndex = -1;
        awaitingUser = false;
        skipToEndOfPhrase = false;

        if (dialogueEncounter is ArielDialogueEncounter2) {
            arielOrSamsonTransform = GameObject.Find("NPC Ariel").transform;
        }
        else {
            arielOrSamsonTransform = GameObject.Find("NPC Samson").transform;
        }
    }
    public void EndDialog() {
        player.ExitUIOrDialog();
        dialogueEncounter.DialogueFinished();
        dialogueBox.SetActive(false);
        text.text = "";
    }
    
    // Input action: user pressed space, "A" (gamepad), etc.
    public void OnNext(InputAction.CallbackContext context) {
        if (context.performed && text != null) {
            // Skip to end of phrase if button is pressed while text is still appearing
            if (!awaitingUser)
                skipToEndOfPhrase = true;

            // Resume text scroll & increase scroll rate if space key is down
            awaitingUser = false;
            currentScrollRate = FAST_SCROLL_RATE;

            // End dialogue if it all has already appeared
            if (phrases != null && phraseIndex >= phrases.Length && charIndex == -1) {
                EndDialog();
            }
        } else if (!context.performed) {
            // Make scroll rate normal again if space key is released
            currentScrollRate = NORMAL_SCROLL_RATE;
        }
    }

    void Update() {
        if (text != null && phrases != null) {
            // Do dialogue action for this phrase if there is one
            if (phraseIndex < phrases.Length &&
                actions[phraseIndex] != null &&
                !actionPerformed &&
                !awaitingUser) {
                dialogueEncounter.DialogueAction(actions[phraseIndex]);
                actionPerformed = true;
            }


            // Position player at a set distance from the NPC
            if (!playerPositionSet) {
                if (System.Math.Abs(arielOrSamsonTransform.position.x - 1.8f - transform.position.x) < 0.05) {
                    lerpTimer = 0;
                    playerPositionSet = true;
                }
                else {
                    player.transform.position = Vector2.Lerp(player.transform.position,
                        new Vector2(arielOrSamsonTransform.position.x - 1.8f, player.transform.position.y),
                        1.0f / 60 * lerpTimer++);
                }
            }

            // Set text box color
            if (characterName.text.Equals("SAMSON") || characterName.text.Equals("????")) {
                transform.parent.GetChild(0).GetChild(0).GetComponent<Image>().color = samsonColor;
            }
            else if (characterName.text.Equals("ARIEL") || characterName.text.Equals("???")) {
                transform.parent.GetChild(0).GetChild(0).GetComponent<Image>().color = arielColor;
            }

            // Make a new letter appear after each interval defined by SCROLL_RATE
            while (Time.time - lastUpdateTime > currentScrollRate &&
                   phraseIndex < phrases.Length &&
                   !awaitingUser) {
                string phrase = phrases[phraseIndex];
                lastUpdateTime = Time.time;

                // Output name of character who's speaking
                int nameDelimiterPoint = phrases[phraseIndex].IndexOf(':');
                if (!foundName && nameDelimiterPoint != -1) {
                    characterName.text = phrases[phraseIndex].Substring(0, phrases[phraseIndex].IndexOf(':'));
                    phrases[phraseIndex] = phrases[phraseIndex].Substring(phrases[phraseIndex].IndexOf(':') + 2);
                    foundName = true;
                }

                // Fill in the rest of the current phrase
                if (skipToEndOfPhrase) {
                    text.text = phrase;
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                    skipToEndOfPhrase = false;
                    actionPerformed = false;
                    foundName = false;
                }
                else {
                    text.text = phrase.Substring(0, 1 + charIndex);
                    charIndex++;
                }

                // Play text blip once every four characters
                if (charIndex % 4 == 0)
                    dialogueEncounter.PlayTextBlip(characterName.text);   

                // Prep for next phrase when the end of current phrase is reached
                if (charIndex == phrase.Length) {
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                    actionPerformed = false;
                    foundName = false;
                }
            }
        }
    }

    public void ParseMessage(string message)
    {
        string[] phrasesAndActions = message.Split(SPLIT_SYMBOL);

        int numPhrases = phrasesAndActions.Length;

        for (int i = 0; i < phrasesAndActions.Length; i++) {
            string phrase = phrasesAndActions[i].Trim();
            if (phrase.StartsWith("{") && phrase.EndsWith("}"))
                numPhrases--;
        }
        actions = new string[numPhrases + 1];
        phrases = new string[numPhrases];

        int diff = 0;
        for (int i = 0; i < phrasesAndActions.Length; i++) {
            string phrase = phrasesAndActions[i].Trim();
            if (phrase.StartsWith("{") && phrase.EndsWith("}")) {
                actions.SetValue(phrase.Trim(new char[2] { '{', '}' }), i - diff);
                diff++;
            }
            else
                phrases.SetValue(phrase, i - diff);
        }
    }

    public Dialogue ActivateDialogueBox() {
        dialogueBox.SetActive(true);
        Dialogue dialogue = dialogueBox.transform.GetChild(1).gameObject.GetComponent<Dialogue>();
        dialogue.characterName = dialogueBox.transform.GetChild(2).gameObject.GetComponent<Text>();
        return dialogue;
    }
}
