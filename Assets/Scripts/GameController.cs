using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Player player;
    new CameraController camera;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
    }

    
    void Update()
    {
        
    }

    public void ChangeLevel()
    {

    }
}
