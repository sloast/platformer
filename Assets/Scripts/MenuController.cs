using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour, GenericController
{

    public Animator transitionScreen;
    public bool animationEnded = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameAnimation()
    {
        StartCoroutine("StartGameCoroutine");
    }

    IEnumerator StartGameCoroutine() {
        transitionScreen.SetTrigger("StartTransition");
        yield return new WaitUntil(()=>animationEnded);
        StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }
    
    public void ExitGame()
    {

    }

    public void GoToLevel(int level)
    {

    }

    public void ContinueAction()
    {
        animationEnded = true;
    }

}
