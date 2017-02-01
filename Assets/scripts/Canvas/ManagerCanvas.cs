using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerCanvas : MonoBehaviour{

    public GameObject canvasTitle;
    public GameObject canvasUI;
    public GameObject canvasLoad;

    private bool changingScene;
    public float loadTime;
    private float timeCounter;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame

     void Update()
    {
        if (Input.GetKey("escape")) ExitGame();

        if(changingScene)
        {
            if( timeCounter > loadTime)
            {
                if (canvasLoad.activeSelf) canvasUI.SetActive(false);
            }
            else
            {
                if (!canvasLoad.activeSelf) canvasUI.SetActive(true);

                timeCounter += timeCounter * Time.deltaTime;
            }

        }
    }

	public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene( string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        canvasTitle.SetActive(false);
        canvasUI.SetActive(true);
    }

}
