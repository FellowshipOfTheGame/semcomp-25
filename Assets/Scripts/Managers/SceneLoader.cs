using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    private IEnumerator LoadSceneDelayed(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Game");

    }
    public void LoadGameDelayed(float delay)
    {
        StartCoroutine(LoadSceneDelayed(delay));
    }

    public void QuitGame(){
        Application.Quit();
    }
}
