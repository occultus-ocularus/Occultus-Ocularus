using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContinueMusic : MonoBehaviour {
    public string[] scenesToEndAt;

    private GameObject duplicate;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        for (int i = 0; i < scenesToEndAt.Length; i++) {
            if (SceneManager.GetActiveScene().name.Equals(scenesToEndAt[i])) {
                Destroy(this.gameObject);
                return;
            }
               

        }

        duplicate = GameObject.Find("MusicPlayer");
        if (duplicate == this.gameObject)
            duplicate = GameObject.Find("MusicPlayerEnd");

        if (duplicate != null && duplicate != this.gameObject)
            Destroy(duplicate);
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
