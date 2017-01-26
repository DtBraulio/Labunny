using UnityEngine;
using System.Collections;

public class object_pooling : MonoBehaviour
{

    public GameObject prefab;
    

    public int numpooling;
    public int index;

    public GameObject[] objectPooling;

    private int framesCounter;

    public Vector3 pene;


   
    // Use this for initialization
    void Start ()
    {

        index = 0;
        framesCounter = 0;
        objectPooling = new GameObject[numpooling];

        for (int i = 0; i < numpooling; i++)
        {
            objectPooling[i] = Instantiate(prefab, this.transform.position, Quaternion.identity) as GameObject;

            objectPooling[i].name = i.ToString();

            objectPooling[i].SetActive(false);


        }


    }
	
	// Update is called once per frame
	void Update ()
    {

        framesCounter++;

        if (framesCounter == 60)
        {
            ActivateBall();
        }

        if (framesCounter == 90)
        {
            ActivateBall();
        }

        if (framesCounter ==120)
        {
            ActivateBall();
            framesCounter = 0;
        }
    }



    void ActivateBall()
    {
        objectPooling[index].transform.position = this.transform.position;

        objectPooling[index].GetComponent<Rigidbody>().AddForce(pene);


        objectPooling[index].SetActive(true);


        index++;

        if (index >= objectPooling.Length) index = 0;
    }
}
