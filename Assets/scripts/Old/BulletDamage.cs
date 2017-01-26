using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

    public int damage;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == "Player")
        {
            hit.gameObject.GetComponent<PlayerController>().SetDamage(damage);

            this.gameObject.SetActive(false);
        }
        else if (hit.tag == "Sword")
        {
            this.gameObject.SetActive(false);
        }
    }
}
