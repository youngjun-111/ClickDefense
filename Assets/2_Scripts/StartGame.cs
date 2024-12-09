using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Button startBtn;

    public void GameStart()
    {
        SceneManager.LoadScene("MainGame");
    }
}
