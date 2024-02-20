using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool isp=false;
    public GameObject pausemenu;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if( isp )
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1.0f;
        isp = false;
    }

    public void Pause()
    {
        pausemenu.SetActive(true);
        Time.timeScale = 0f;
        isp=true;
    }

    public void gotomenu()
    {
        Resume();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
