using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Script to record the amount of time taken for the player to reach various
 * checkpoints in the game. You can call CheckPointReached(string) from a UnityEvent
 * (e.g. on a LaserReceiver) or from another script, parameterized with a string
 * representing the name of the checkpoint. Sends data to a Google Form
 * once the game is restarted or quitted, as long as at least one checkpoint
 * has been reached. */

public class PlayStats : MonoBehaviour
{
    public List<float> checkpointTimes = new List<float>();
    public List<string> checkpointNames = new List<string>();

    private float prevCheckpointTime;
    private string gameOutput;
    private PlayStats original;

    /* At the start of the scene, sees if there's an older
     * PlayStats instance from the previous scene, and if there
     * is, takes its data and destroys its GameObject */
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Mall":
                original = GameObject.Find("PlaytestStatsMain").GetComponent<PlayStats>();
                break;
            case "Outside Mall+TTD, MPT":
                original = GameObject.Find("PlaytestStatsMall").GetComponent<PlayStats>();
                break;
            case "ArielIntro":
                original = GameObject.Find("PlaytestStatsOutside").GetComponent<PlayStats>();
                break;
            case "SuburbPuzzles":
                original = GameObject.Find("PlaytestStatsAriel").GetComponent<PlayStats>();
                break;
            case "End Menu":
                original = GameObject.Find("PlaytestStatsSuburb").GetComponent<PlayStats>();
                break;
            case "Main Menu":
                original = GameObject.Find("PlaytestStatsEnd").GetComponent<PlayStats>();
                break;
        }

        if (original != null)
        {
            checkpointTimes = original.checkpointTimes;
            checkpointNames = original.checkpointNames;
            Destroy(original.gameObject);
        }
    }

    // Adds name of checkpoint & time since last checkpoint to the lists
    public void CheckpointReached(string name)
    {
        if (checkpointNames.IndexOf(name) != -1 || !enabled) {
            return;
        }

        checkpointNames.Add(name);
        if (checkpointTimes.Count > 0)
            checkpointTimes.Add(Time.timeSinceLevelLoad - prevCheckpointTime);
        else
            checkpointTimes.Add(Time.timeSinceLevelLoad);
        prevCheckpointTime = Time.timeSinceLevelLoad;
    }

    // Called when the game is restarted: uploads checkpoint data & resets lists
    public void RestartGame()
    {
        if (checkpointTimes.Count != 0 && !Application.isEditor)
        {
            FormatOutput();
            PostToGoogleForm();
            checkpointNames = new List<string>();
            checkpointTimes = new List<float>();
        }
    }

    // When the game is quit, uploads the current checkpoint data
    private void OnApplicationQuit()
    {
        if (checkpointTimes.Count != 0 && !Application.isEditor)
        {
            FormatOutput();
            PostToGoogleForm();
        }
    }

    // Formats text output before it is posted to the Google Form
    void FormatOutput()
    {
        // Makes a line of comma-separated values (CSV) for checkpoint names
        for (int i = 0; i < checkpointNames.Count; i++)
        {
            gameOutput += checkpointNames[i];
            if (i != checkpointNames.Count - 1)
                gameOutput += ", ";
        }
        gameOutput += "\n";

        // Makes a line of CSV for checkpoint times (rounded to tenth of a sec)
        for (int i = 0; i < checkpointTimes.Count; i++)
        {
            gameOutput += Mathf.Round(checkpointTimes[i] * 10) / 10;
            if (i != checkpointTimes.Count - 1)
                gameOutput += ", ";
        }
        gameOutput += "\n";
    }

    private void PostToGoogleForm()
    {
        StartCoroutine(Post());
    }

    // Posts playtest data that has been collected to the specified Google Form
    IEnumerator Post()
    {
        WWWForm form = new WWWForm();

        // Google Form field ids can be found by making a prefilled form and getting them out of the URL
        form.AddField("entry.1740890139", gameOutput);

        // URL for the Google Form with "formResponse" after form id instead of "viewForm"
        string url = "https://docs.google.com/forms/d/e/1FAIpQLSd4kWrFGVorQFOsplGaWgBrCn5mdSfHLXNhkUMf7003FeNdnw/formResponse";

        // Post a request to the URL
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
            Debug.Log("Playtest data upload complete");
    }
}
