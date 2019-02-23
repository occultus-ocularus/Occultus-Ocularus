using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public static bool messageComplete = true;

    const char SPLIT_SYMBOL = '|';
    const float NORMAL_SCROLL_RATE = 0.06f;
    const float FAST_SCROLL_RATE = 0.03f;

    public Text text;

    public string[] phrases;
    private float lastUpdateTime;
    private float currentScrollRate;

    private int phraseIndex = 0;
    private int charIndex = -1;
    private bool awaitingUser = false;

    PlayerController player;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void Setup()
    {
        player.canMove = false;
        lastUpdateTime = Time.time;
        currentScrollRate = NORMAL_SCROLL_RATE;
        phrases = new string[1];
    }

    void Update()
    {
        if (text != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentScrollRate = FAST_SCROLL_RATE;
                awaitingUser = false;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                currentScrollRate = NORMAL_SCROLL_RATE;
            }

            //Add a new letter after each interval defined by SCROLL_RATE
            while (Time.time - lastUpdateTime > currentScrollRate && phraseIndex < phrases.Length && awaitingUser == false)
            {
                string phrase = phrases[phraseIndex];
                lastUpdateTime = Time.time;
                text.text = phrase.Substring(0, 1 + charIndex);

                charIndex++;

                if (charIndex == phrase.Length)
                {
                    charIndex = -1;
                    phraseIndex++;
                    awaitingUser = true;
                }
            }
            if (phraseIndex >= phrases.Length && Input.GetKeyUp(KeyCode.Space))
            {
                Destroy(transform.parent.gameObject);
                player.canMove = true;
            }
        }
    }

    public void ParseMessage(string message)
    {
        phrases = message.Split(SPLIT_SYMBOL);
    }

    public Dialogue ConstructDialogueBox()
    {
        GameObject dialogueBox = Instantiate(Resources.Load("Prefabs/DialogueBox") as GameObject);
        dialogueBox.transform.parent = transform;
        dialogueBox.transform.position = transform.GetChild(0).position;
        GameObject textBox = dialogueBox.transform.GetChild(1).gameObject;
        Dialogue dialogue = textBox.GetComponent<Dialogue>();
        dialogue.Setup();
        return dialogue;
    }
}
