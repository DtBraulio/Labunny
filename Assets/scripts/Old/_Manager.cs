using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class _Manager : MonoBehaviour {

    public Scene combatZone;
    public Scene moveZone;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.C)) SceneManager.SetActiveScene(combatZone);
        if (Input.GetKeyDown(KeyCode.M)) SceneManager.SetActiveScene(moveZone);


    }
}
