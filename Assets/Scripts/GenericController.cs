using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GenericController
{
    void StartGame();
    void ExitGame();
    void GoToLevel(int level);
    void ContinueAction();
}
