using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
     public string setActiveBackgroundMusic;
     public void PlayGame()
     {
          SceneManager.LoadScene(1);
     }

     private void Start(){
          AudioManager.Instance.PlayMusic(setActiveBackgroundMusic);
     }
}
