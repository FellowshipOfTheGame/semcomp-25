using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameOver gameOver;

    [SerializeField] private Slider levelProgressSlider;
    [SerializeField] private TMP_Text currLevelView;
    [SerializeField] private TMP_Text nextLevelView;
    [SerializeField] private GameObject hud;
    [SerializeField] PlayerInputManager inputManager;
    [SerializeField] BallController ball;
    [SerializeField] float startTime;
    [SerializeField] private GameObject startAnimation;

    private int level;
    public int Level => level;

    // Start is called before the first frame update
    private void Start()
    {
        gameOver = GameOver.Instance;
        SetLevelView();
        AudioManager.instance.PlayMusic("GameMusic");
        AudioManager.instance.PlaySFX("Bora");
    }

    public IEnumerator StartGameDelay()
    {
        startAnimation.SetActive(true);
        hud.SetActive(false);
        inputManager.SetCanMove(false);
        //ball.SetCanAim(false);
        yield return new WaitForSecondsRealtime(startTime);
        //ball.SetCanAim(true);
        inputManager.SetCanMove(true);
        hud.SetActive(true);
    }

    IEnumerator ProgressAnim(float p)
    {
        float x = levelProgressSlider.value;
        if (x > p)
            x = 0;
        const float speed = .7f;
        while (x <= p)
        {
            x += Time.deltaTime * speed;
            x = Mathf.Min(x, p);
            levelProgressSlider.value = x;

            yield return new WaitForEndOfFrame();
        }
        if (Math.Abs(p - 1f) < 0.001f)
        {
            levelProgressSlider.value = 1;
        }
    }
    
    public void SetLevelProgress(float p)
    {
        StartCoroutine(ProgressAnim(p));
    }
    
    public void PassLevel(bool changeUI)
    {
        level++;
        if (changeUI)
            SetLevelView();
    }

    public void SetLevelView()
    {
        currLevelView.text = level + "";
        nextLevelView.text = (level + 1) + "";
    }

    public void GameOverScene(int highscore)
    {
        gameOver.OnGameOver(highscore);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
