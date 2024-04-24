using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
   public void PlayGame()
   {
    SceneManager.LoadScene("Level 1");
   }

   public void QuitGame()
   {
    Debug.Log("QUIT!");
    Application.Quit();
   }

   public void playGameAgain()
   {
      SceneManager.LoadScene("Main Menu");
   }
}
