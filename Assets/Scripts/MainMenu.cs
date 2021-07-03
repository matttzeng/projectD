using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace ProjectD
{
    public class MainMenu : MonoBehaviour
    {

        public static bool isContinue;
        // Start is called before the first frame update

        

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void ContinueGame()
        {
            isContinue = true;
            PlayGame();
        }

        public void QuitGame()
        {
            Application.Quit();
        }



    }

}
