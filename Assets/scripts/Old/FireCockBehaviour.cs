using UnityEngine;
using System.Collections;

public class FireCockBehaviour : MonoBehaviour {

    public int life;
    public Transform weakSpotChecker;
    public LayerMask weakSpotMask;
    public Vector3 weakSpotSize;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Collider[] collidersWeakSpot = Physics.OverlapBox(weakSpotChecker.position, weakSpotSize / 2, weakSpotChecker.rotation, weakSpotMask);

        for (int i = 0; i < collidersWeakSpot.Length; i++)
        {
            if (collidersWeakSpot[i].tag == "Sword")
            {
                SetDamage(collidersWeakSpot[i].gameObject.GetComponent<Sowrd_01>().damage);
                break;
            }
        }

    }
    void SetDamage(int damage)
    {
        life -= damage;
        if (life < 0) SetDead();
    }
    void SetDead()
    {
        this.gameObject.SetActive(false);
    }
    void OnDrawGizmos()//todo lo k esta aki dentro se estara llamando cada frame
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(weakSpotChecker.position, weakSpotSize);
    }
}
