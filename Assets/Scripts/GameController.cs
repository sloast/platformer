using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Player player;
    new CameraController camera;
    public int current_level = 0;
    List<LevelData> levels = new List<LevelData>();

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        levels.Add(GameObject.Find("0").GetComponent<LevelData>());
        levels.Add(GameObject.Find("1").GetComponent<LevelData>());
    }

    
    void Update()
    {
        
    }

    public void ChangeLevel(GameObject trigger)
    {
        LevelTransition lt = trigger.gameObject.GetComponent<LevelTransition>();
        current_level = current_level == lt.a ? lt.b : lt.a;
        camera.SetTarget(levels[current_level].gameObject.transform.position);
        player.SetStartCoordinates(levels[current_level].startPos);
    }


}
