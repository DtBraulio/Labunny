using UnityEngine;
using System.Collections;

public class Sowrd_01 : MonoBehaviour
{

    public int damage;

    public Vector3 size;
    public LayerMask mask;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void OnDrawGizmos()//todo lo k esta aki dentro se estara llamando cada frame
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size / 2);
    }
}
