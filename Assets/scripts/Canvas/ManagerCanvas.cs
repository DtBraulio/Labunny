using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerCanvas : MonoBehaviour{

    public GameObject canvasTitle;
    public GameObject canvasUI;
    public GameObject canvasLoad;

    public CanvasRenderer rendCanvas;
    private Material materialFade;
    private bool changingScene;
    public float loadTime;
    public float restAlpha;
    private float timeCounter;

    // Use this for initialization
    void Start ()
    {
        materialFade = rendCanvas.GetPopMaterial(01);
	}
	
	// Update is called once per frame

     void Update()
    {

        if (Input.GetKey("escape")) ExitGame();

        if(changingScene)
        {

            if( timeCounter > loadTime)
            {
                materialFade.color = FadeColor(materialFade.color);

                if(materialFade.color.a <= 0)
                {
                    if(canvasLoad.activeSelf) canvasLoad.SetActive(false);
                    timeCounter = 0;
                    changingScene = false;
                }
            }
            else
            {
                if (!canvasLoad.activeSelf) canvasLoad.SetActive(true);

                timeCounter += Time.deltaTime;

            }

        }
    }

	public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene( string sceneName)
    {
        changingScene = true;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        canvasTitle.SetActive(false);
        canvasUI.SetActive(true);
    }

    Color FadeColor(Color targetColor)
    {
        Color returnColor = Color.white;


        if(targetColor.a > 0)
        {
            targetColor.a -= restAlpha * Time.deltaTime;
        }
        else
        {
            targetColor.a = 0;
        }

        returnColor = targetColor;
        return returnColor;
    }

}
