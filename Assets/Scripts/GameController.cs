using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Player player;
    new CameraController camera;
    public int current_level = 0;
    List<LevelData> levels = new List<LevelData>();
    [SerializeField]
    GameObject pauseMenu = null;
    bool paused = false;
    [SerializeField]
    Button primaryButton = null;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        levels.Add(GameObject.Find("0").GetComponent<LevelData>());
        levels.Add(GameObject.Find("1").GetComponent<LevelData>());
        //pauseMenu = GameObject.FindWithTag("PauseMenu");
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
            primaryButton.Select();
        }
    }



}
