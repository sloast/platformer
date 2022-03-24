using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, GenericController
{
    Player player;
    new CameraController camera;
    public Animator transitionScreen;
    public int current_level = 0;
    List<LevelData> levels = new List<LevelData>();
    [SerializeField]
    GameObject pauseMenu = null;
    bool paused = false;
    [SerializeField]
    Button primaryButton = null;
    bool animationEnded = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        levels.Add(GameObject.Find("0").GetComponent<LevelData>());
        levels.Add(GameObject.Find("1").GetComponent<LevelData>());
        //pauseMenu = GameObject.FindWithTag("PauseMenu");
        SetCursorVisible(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPaused(!paused);
        }
    }

    public void ChangeLevel(GameObject trigger)
    {
        LevelTransition lt = trigger.gameObject.GetComponent<LevelTransition>();
        current_level = current_level == lt.a ? lt.b : lt.a;
        camera.SetTarget(levels[current_level].gameObject.transform.position);
        player.SetStartCoordinates(levels[current_level].startPos);
    }

    public void SetPaused(bool value)
    {
        Time.timeScale = value ? 0f : 1f;
        pauseMenu.SetActive(value);
        paused = value;
        if (paused) {
            SetCursorVisible(true);
            primaryButton.Select();
        } else
        {
            SetCursorVisible(false);
        }
    }

    public void StartExitAnimation()
    {
        StartCoroutine("ExitCoroutine");
    }
    IEnumerator ExitCoroutine()
    {
        transitionScreen.SetTrigger("StartTransition");
        yield return new WaitUntil(() => animationEnded);
        ExitGame();
    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SetCursorVisible(true);
        SceneManager.LoadScene(0);
    }

    public void StartGame() { } // Not used
    public void GoToLevel(int level) { }

    public void ContinueAction()
    {
        animationEnded = true;
    }

    void SetCursorVisible(bool visible)
    {
        if (visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
