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

    public void Setup()
    {
        player = (PlayerController)GameObject.Find("Player").GetComponent("PlayerController");
        player.canMove = false;
        lastUpdateTime = Time.time;
        currentScrollRate = NORMAL_SCROLL_RATE;
        phrases = new string[1];
    }

    void Update()
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

    public void ParseMessage(string message)
    {
        phrases = message.Split(SPLIT_SYMBOL);
    }

    public static Dialogue ConstructDialogueBox()
    {
        Vector2 position = GameObject.Find("DialogueBoxPos").transform.position;
        GameObject dialogueBox = Instantiate(Resources.Load("DialogueBox") as GameObject);
        dialogueBox.transform.parent = GameObject.Find("Canvas").transform;
        dialogueBox.transform.position = position;
        GameObject text = dialogueBox.transform.GetChild(1).gameObject;
        text.BroadcastMessage("Setup");
        Dialogue dialogue = (Dialogue)text.GetComponent((typeof(Dialogue))) as Dialogue;
        return dialogue;
    }
}
