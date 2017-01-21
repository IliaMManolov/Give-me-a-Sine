using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceenLoader : MonoBehaviour {

    public void OnStartGame()
    {
        SceneManager.LoadScene("main");
    }

}
