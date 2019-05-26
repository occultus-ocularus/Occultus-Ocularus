using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Tooltip("Name of scene to load when 'Play' is clicked")]
    [SerializeField]
    public string firstScene = "LayerSwitchingExample";

    [Tooltip("Player prefab")]
    [SerializeField]
    public GameObject player;

    [Tooltip("Sprites for the two characters")]
    [SerializeField]
    public Sprite[] sprites = new Sprite[2];

    // Start is called before the first frame update
    void Start()
    {
        SetCharacter(PlayerPrefs.GetInt("character"));
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(firstScene);
    }

    public void ChangeCharacter(int character)
    {
        PlayerPrefs.SetInt("character", character);

        SetCharacter(character);
    }

    public void SetCharacter(int character)
    {
        if (character == 0)
        {
            player.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            player.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int buildNumber)
    {
        SceneManager.LoadScene(buildNumber);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

