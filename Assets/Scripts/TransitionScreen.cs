using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    public GenericController controller;
    
    
    void Start()
    {
        controller = GameObject.FindWithTag("GameController").GetComponent<GenericController>();
    }

    public void StartGame()
    {
        controller.StartGame();
    } 

    public void GoToLevel(int level)
    {
        controller.GoToLevel(level);
    }

    public void ExitGame()
    {
        controller.ExitGame();
    }

    public void AnimationEnded(){
        controller.ContinueAction();
    }
}
