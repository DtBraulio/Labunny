using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private CharacterController controller;
    public Transform player;
    public static Vector3 moveDirection;
    private Vector3 inputAxis;
    private Vector3 desireDirection;


    private float speed;
    public float runSpeed;
    public float speedWalk;
    public float jumpSpeed;
    //public float doubleJumpSpeed;
    private float forceToGround;
    public float gravityMagnitude;


    public float dash;

    private bool jump;
    
    //private bool doubleJump;
    //private bool ground;

    [Header("sounds")]
    
    [Header("TimeCounters")]
    private float timeStep;
    private float timeCounter;
    [Header("PlayerStats")]
    public int life;
    public bool dead;


    [Header("Animations")]
    public Animator anim;


    //float old_position;

    //public static bool animrun;

    bool rotate;
    [Header("UI")]
   


    int time;

    //public GameObject player;

    //public float radius;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        /* controller = player.GetComponent<CharacterController>(); // ejemplo
         radius = controller.radius;*/

        forceToGround = Physics.gravity.y;

        
        //old_position = transform.position.x;
        //animrun = true;

        rotate = false;

        time = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!dead)
        {
            ReadInput();
            ControlSpeed();
            RotationControl();
            PlayFootSteps();

            if (Input.GetKeyDown("q"))
            {
                Debug.Log("Q pressed");
                anim.Play("weapon");
                
            }

         

            if ((controller.isGrounded) && (!jump))
            {
                moveDirection.y = forceToGround;
            }

            else
            {
                moveDirection += gravityMagnitude * Physics.gravity * Time.deltaTime;
                jump = false;
                //doubleJump = false;

            }

            controller.Move(moveDirection * Time.deltaTime);

            if (rotate)
            {
                //anim.transform.rotation = Quaternion.Euler(anim.transform.rotation.x, 180, anim.transform.rotation.z);
                
            }

            if (!rotate)
            {
                //anim.transform.rotation = Quaternion.Euler(anim.transform.rotation.x, 0, anim.transform.rotation.z);
             
            }
            if (player.position.y <= -20)
            {
                //SetDead();              
            }     

        }
        
        //moving Left
        //if(moveDirection.x != 0)
        //{
        //    if ((moveDirection.x < 0) && (animrun))
        //    {
        //        anim.SetBool("moverse", true);
        //        anim.Play("Run_01");
        //        rotate = true;

        //    }
        //    //movingRight
        //    if ((moveDirection.x > 0) && (animrun))
        //    {
        //        anim.SetBool("moverse", true);
        //        anim.Play("Run_01");

        //        rotate = false;

        //    }
            

            
        //}
        //else anim.SetBool("moverse", false);


        if(dead)
        {
            time++;
            if (time >= 30)
            {
                //SceneManager.LoadScene("Menu");

                time = 0;
            }
        }

    }

    void ReadInput()
    {
       

        if (controller.isGrounded)
        {


            if (Input.GetButtonDown("Jump")) // = input.getKey (keycode.space)
            {
                jump = true;

                moveDirection.y = jumpSpeed;

                //ground = true;

                
            }

            
        }
       /* else if (((!controller.isGrounded) && (!jump) && (!doubleJump) && (ground)))
        {
            

            if (Input.GetButtonDown("Jump")) // = input.getKey (keycode.space)
            {                
                doubleJump = true;

                moveDirection.y = doubleJumpSpeed;

                ground = false;

                sound.Play(0, 100, Random.Range(1.3f, 1.5f));
            }
            
            
         }*/
        ReadTestingInputs();
    }
    void ReadTestingInputs()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SetDamage(1);
    }

    void ControlSpeed()
    {

        if (Input.GetButton("Fire3"))
        {
            speed = runSpeed;
            timeStep = 0.3f;
            
        }
        else
        {
            speed = speedWalk;
            timeStep = 0.5f;
        }
        desireDirection = inputAxis.x * transform.right + inputAxis.z * transform.forward;
        //moveDirection.x = Input.GetAxis("Horizontal") * speed; 
       

        inputAxis.x = Input.GetAxis("Horizontal");
        //moveDirection.z = Input.GetAxis("Vertical") * speed;
        
        inputAxis.z = Input.GetAxis("Vertical");

        moveDirection.x = desireDirection.x * speed; 
        //moveDirection.z =  desireDirection.z * speed;

        controller.Move(moveDirection * Time.deltaTime);

    


    }

    void RotationControl()
    {
       /* if (Input.GetButton("Fire1"))
        {
            transform.rotation = Random.rotation;
        }*/
        
    }

    bool IsMoving()
    {

        if ((inputAxis.x != 0) || (inputAxis.z != 0)) return true;

        else return false;



    }

    void PlayFootSteps()
    {

        if (IsMoving () && controller.isGrounded)
        {
            if (timeCounter >= timeStep)
            {
                
                timeCounter = 0;
            }

            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        else
        {
            timeCounter = timeStep;
        }
       

    }

    public void SetDamage(int damage)
    {
        Debug.Log("recive");

        if (!dead)
        {
            life -= damage;
            
            if (life <= 0)
            {
                SetDead();
               
            }
            else
            {
                
            }
        }
        
    }
    public void SetDead()
    {
       
        dead = true;
        inputAxis = Vector3.zero;



    }


}
