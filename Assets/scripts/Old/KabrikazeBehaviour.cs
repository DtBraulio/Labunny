using UnityEngine;
using System.Collections;

public class KabrikazeBehaviour : MonoBehaviour {

    [Header("Ini")]
    public float counterTime;
    public int state;// 1- Casting 2-Running 3-Attacking 4-Waiting 5-Stuneado 6-Damaged 7 -Dead 8-Inactive
    private Rigidbody rb;
    private Transform enemyTransform;
    public Animator anim;
    //Checker
    public Transform bodyChecker;
    public LayerMask bodyMask;
    public Vector3 bodySize;
    public bool isTouchingWall;
    //CheckerWeakSpot
    public Transform weakSpotChecker;
    public LayerMask weakSpotMask;
    public Vector3 weakSpotSize;

    [Header("STATS")]
    public int life;
    public int damage;

    [Header("Casting")]

    public float castingTime;

    [Header("Running")]

    //Distancia extra que recorre
    public float extraDistance;

    //Velocidad de desplazamiento
    public float speed;

    //Limites de la distancia del personaje
    public float limits;

    [Header("Attacking")]

    //CheckerAttack
    public Transform attackChecker;
    public LayerMask attackMask;
    public Vector3 attackSize;

    public float attackTime;

    [Header("Waiting")]

    public float waitingTime;

    [Header("Stuned")]

    public float stunedTime;

    [Header("Damaged")]

    public float damagedTime;
    public int damagedHits;

    [Header("Dead")]

    public float deadTime;

    [Header("Inactive")]

    //CheckerTarged
    public Transform targedChecker;
    public LayerMask targedMask;
    public Vector3 targedSize;

    public Vector3 targedPosition;
    public bool isTarged;



    void Start ()
    {
        state = 8;

        rb = GetComponent<Rigidbody>();
        enemyTransform = this.transform;
    }
	
	void Update ()
    {
        switch (state)
        {
            case 1://Casting

                Casting();

                break;
            case 2://Running

                Running();

                break;
            case 3://Attacking

                Attacking();

                break;
            case 4://Waiting

                Waiting();

                break;
            case 5://Stuned

                Stuned();

                break;
            case 6://Damaged

                Damaged();

                break;
            case 7://Dead

                Dead();

                break;

            case 8://Inactive

                Inactive();

                break;
            default:

                break;
        }

        //CHECKER TRIGGER

        Collider[] collidersBody = Physics.OverlapBox(bodyChecker.position, bodySize / 2, bodyChecker.rotation, bodyMask);

        for (int i = 0; i < collidersBody.Length; i++)
        {
            if (collidersBody[i].tag == "Player")
            {
                collidersBody[i].gameObject.GetComponent<PlayerController>().SetDamage(damage);
            }
            if (collidersBody[i].tag == "Wall")
            {
                isTouchingWall = true;
            }
            else isTouchingWall = false;
        }

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
    void FixedUpdate()
    {


    }

    void Casting()
    {
        if (counterTime >= castingTime)
        {
            //cambio de estado a corriendo
            counterTime = 0;
            anim.SetTrigger("Running");
            state = 2;
        }
        else counterTime += 1 * Time.deltaTime;
    }
    void Running()
    {
        if (enemyTransform.position.x - limits > targedPosition.x + extraDistance)
        {
            if (speed > 0) speed = -speed;
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
        }
        else if (enemyTransform.position.x + limits < targedPosition.x + extraDistance)
        {
            if (speed < 0) speed = -speed;
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
            anim.SetTrigger("Attacking");
            state = 3;
        }
        if (isTouchingWall)
        {
            rb.velocity = new Vector3(0, 0, 0);
            anim.SetTrigger("Stunned");
            state = 5;
        }
    }
    void Attacking()
    {
        //CHECKER tARGED
        Collider[] colliders = Physics.OverlapBox(attackChecker.position, attackSize / 2, attackChecker.rotation, attackMask);

        if (colliders.Length > 0)
        {
            colliders[0].gameObject.GetComponent<PlayerController>().SetDamage(damage);
        }

        //TODO: TIEMPO DE LA ANIMACION DE AATACK
        if (counterTime >= attackTime )
        {
            counterTime = 0;
            anim.SetTrigger("Waiting");
            state = 4;
        }
        else counterTime += 1 * Time.deltaTime;
    }
    void Waiting()
    {
        if (counterTime >= waitingTime)
        {
            //cambio de estado a corriendo
            counterTime = 0;
            anim.SetTrigger("Inactive");
            state = 8;
        }
        else counterTime += Time.deltaTime;
    }
    void Stuned()
    {
        if (counterTime >= stunedTime)
        {
            //cambio de estado a corriendo
            counterTime = 0;
            anim.SetTrigger("Inactive");
            state = 8;
        }
        else counterTime += Time.deltaTime;
    }
    void Damaged()
    {
        life -= damagedHits;
        if (life < 0) state = 7;
        else state = 8;
    }
    void Inactive()
    {
        //CHECKER tARGED
        Collider[] colliders = Physics.OverlapBox(targedChecker.position, targedSize/2, targedChecker.rotation, targedMask);

        if (colliders.Length > 0)
        {
            targedPosition = colliders[0].gameObject.transform.position;
            isTarged = true;
            anim.SetTrigger("Casting");
            state = 1;
        }
        else isTarged =false;

    }
    void Dead()
    {
        Debug.Log("enter");
        gameObject.SetActive(false);
    }
    void OnDrawGizmos()//todo lo k esta aki dentro se estara llamando cada frame
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(targedChecker.position, targedSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bodyChecker.position, bodySize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackChecker.position, attackSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(weakSpotChecker.position, weakSpotSize);
    }
    void SetDamage(int damage)
    {
        damagedHits = damage;
        state = 6;
    }
}
