using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{

    public void LoadLevel (string levelName) {
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
    }

    // Reload the scene we're already on
    public void ReloadLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }


}
