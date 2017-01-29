using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [Header("STATS")]

    //current STATS
    public int lifes;
    public int lifePoints;
    public int score;
    private bool IsDead;

    //Inicial STATS
    public int iniScore;
    public int iniLifes;
    public int maxLifes;
    public int iniLifePoints;
    public int maxLifePoints;


    [Header("Chequers")]
    public Transform groundChecker;
    public LayerMask groundMask;
    public Vector3 groundSize;
    public bool isGrounded;
    public Transform wallChecker;
    public LayerMask wallMask;
    public Vector3 wallSize;
    public bool isTouchingWall;
    public bool isRight;


    [Header("EXTRAS")]
    public float horizontalAxis;
    public float gravityMagnitude;
    private Rigidbody rbPlayer;
    private Transform transPlayer;
    private Vector3 respawnPosition;
    private bool gameOver;

    [Header("Animation")]
    public Animator animMov;
    public Animator animAtck;
    public Transform graphics;



    [Header("Desplaçamiento")]

    public int statesMovement;// 0- STOP; 1- Walk +; 2- Acc +; 3-Run +; 4-Drift +;
    public float currentSpeed;
    public float speedWalk;
    public float aceleration;
    public float speedMax;
    public float limitAxisToAccelerate;
    private float multiplierSpeed;

    public float slowDrift;



    [Header("Salto")]

    public float jumpForce;
    public float jumpForceCrt;
    public float invertGravity;
    private bool IsDoubleJumping;
    public float airTime;
    public float maxAirTime;
    public float jumpDobleForce;

    [Header("Dash")]

    public float dashForce;
    public bool IsDashing;
    public float dashColdDown;

    [Header("Ataque")]

    public int damage;
    public int IniDamage;

    void Start ()
    {
        rbPlayer = GetComponent<Rigidbody>();
	}

    void Update()
    {
        Desplazamiento();

        Locomotion();

        Flip();

        Checkers();

        Jump();

        Attack();



    }

    private void Desplazamiento() 
    {

        //COJEMOS INPUTS
        horizontalAxis = Input.GetAxis("Horizontal");

        switch (statesMovement)
        {
            case 0://STOP

                Stop(horizontalAxis);

                break;
            case 1://WALK

                Walk(horizontalAxis);

                break;

            case 2://ACCELERATION

                Acceleration(horizontalAxis);

                break;
            case 3://RUN

                Run(horizontalAxis);

                break;
            case 4://DRIFT

                Drift(horizontalAxis);

                break;
        }

        CorrectionSpeedTouchingWall();

        //APLICACAMOS LA VELOCIDAD
        rbPlayer.velocity = new Vector3(currentSpeed, rbPlayer.velocity.y, rbPlayer.velocity.z);
    }

    private void Stop(float horizontalAxis)
    {

        Debug.Log("STOP");
        currentSpeed = 0;

        //LO ENVIAMOS A WALK
        if (horizontalAxis != 0) statesMovement = 1;


    }

    private void Walk(float horizontalAxis)
    {
        Debug.Log("WALK");

        currentSpeed = (speedWalk * horizontalAxis);


        //LO ENVIAMOS AL ACCELERATION
        if ((horizontalAxis >= limitAxisToAccelerate) || (horizontalAxis <= -limitAxisToAccelerate)) statesMovement = 2;
        else if (horizontalAxis == 0) statesMovement = 0;
    }

    private void Acceleration(float horizontalAxis)
    {
        //COMPROVAMOS IZQ O DER

        if (isRight)// DER
        {
            Debug.Log("ACC DER");

            //INCREMENTAMOS EL MULTIPIES DE LA VELOCIDAD
            //Movimiento Rectilineo Uniformemente Acelerado
            multiplierSpeed += Time.deltaTime * aceleration;
            currentSpeed = (speedWalk * horizontalAxis) + multiplierSpeed;

            //LO EVNIAMOS AL RUN
            if (currentSpeed >= speedMax)
            {
                multiplierSpeed = 0;
                statesMovement = 3;
            }

            //LO ENVIAMOS AL WALK
            if (horizontalAxis < limitAxisToAccelerate)
            {
                multiplierSpeed = 0;
                statesMovement = 1;
            }

        }
        else if (!isRight)//IZQ
        {

            Debug.Log("ACC IZQ");

            //INCREMENTAMOS EL MULTIPIES DE LA VELOCIDAD
            //Movimiento Rectilineo Uniformemente Acelerado
            multiplierSpeed += Time.deltaTime * -aceleration;
            currentSpeed = (speedWalk * horizontalAxis) + multiplierSpeed;


            //LO EVNIAMOS AL RUN
            if (currentSpeed <= -speedMax)
            {
                multiplierSpeed = 0;
                statesMovement = 3;
            }

            //LO ENVIAMOS AL WALK
            if (horizontalAxis > -limitAxisToAccelerate)
            {
                multiplierSpeed = 0;
                statesMovement = 1;
            }
        }

    }

    private void Run(float horizontalAxis)
    {
        Debug.Log("RUN");

        //******* (SOLO funcionara con teclado)

        //COMPROVAMOS IZQ O DER

        if (horizontalAxis > 0)// DER
        {
            Debug.Log("RUN DER");
            currentSpeed = speedMax;
        }
        else if (horizontalAxis < 0)//IZQ
        {
            Debug.Log("RUN IZQ");
            currentSpeed = -speedMax;
        }


        if (currentSpeed > 0)// DER
        {

            Debug.Log("RUN DER RED");

            // LO ENVIAMOS AL DRIFT
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) statesMovement = 4;
            else if (horizontalAxis < limitAxisToAccelerate) statesMovement = 2;

        }
        else if (currentSpeed < 0)//IZQ
        {
            Debug.Log("RUN IZQ RED");

            // LO ENVIAMOS AL DRIFT
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) statesMovement = 4;
            else  if (horizontalAxis > -limitAxisToAccelerate) statesMovement = 2;
        }

    }

    private void Drift(float horizontalAxis)
    {

        Debug.Log("DRIFT");
        
        //COMPROVAMOS IZQ O DER
        if (currentSpeed > 0)// DER
        {
            Debug.Log("DRIFT DER");
            multiplierSpeed += Time.deltaTime * slowDrift;
            currentSpeed -= multiplierSpeed;

            //LO EVNIAMOS AL STOP
            if (currentSpeed <= 0)
            {
                multiplierSpeed = 0;
                statesMovement = 0;
            }

            animMov.Play("PlantNTurnRight180");

        }
        else if (currentSpeed < 0)//IZQ
        {
            Debug.Log("DRIFT IZQ");

            multiplierSpeed += Time.deltaTime * slowDrift;
            currentSpeed += multiplierSpeed;

            //LO EVNIAMOS AL STOP
            if (currentSpeed >= 0)
            {
                multiplierSpeed = 0;
                statesMovement = 0;
            }
        }

    }

    private void Jump () 
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Debug.Log("1rJump");
                airTime = 0;
                rbPlayer.AddForce(new Vector3 (0,jumpForce,0), ForceMode.Acceleration);
            }
            else if(!isGrounded && !IsDoubleJumping)
            {

                Debug.Log("2nJump");
                airTime = 0;
                IsDoubleJumping = true;
                rbPlayer.AddForce(new Vector3(0, jumpDobleForce, 0), ForceMode.Acceleration);
            }

        }
        if(Input.GetButton("Jump") && (!isGrounded))
        {
            if (!IsDoubleJumping)
            {
                if (airTime <= maxAirTime)
                {
                    Debug.Log("subiendo");
                    airTime += 1f * Time.deltaTime;
                    jumpForceCrt += invertGravity * airTime;

                    rbPlayer.AddForce(new Vector3(0, jumpForceCrt, 0), ForceMode.Force);
                }
            }

        }

            /* if (Input.GetButton("Jump") && (isGrounded))
     {
          jumpForceCrt = jumpForce + jumpForceIncreaser * Time.deltaTime;


     }
     else if(Input.GetButtonUp("Jump") && (isGrounded))
     {
         rbPlayer.AddForce(new Vector3(0, jumpForceCrt, 0), ForceMode.Acceleration);
         jumpForceCrt = jumpForce;
     }
     else if (Input.GetButtonDown("Jump") && (!isGrounded))
     {
         if (!IsDoubleJumping)
         {
             IsDoubleJumping = true;
             rbPlayer.AddForce(new Vector3(0, jumpDobleForce, 0), ForceMode.Acceleration);
         }
     }*/

        }

    private void CorrectionSpeedTouchingWall()
    {
        if( isTouchingWall)
        {
            if(isRight && horizontalAxis > 0)
            {
                currentSpeed = 0;
                horizontalAxis = 0;
                statesMovement = 0;

            }
            else if(!isRight && horizontalAxis < 0)
            {
                currentSpeed = 0;
                horizontalAxis = 0;
                statesMovement = 0;
            }
        }
    }

    private void ResetJump()
    {
        airTime = 0;
        jumpForceCrt = jumpForce / 10;
        IsDoubleJumping = false;
    }

    private void Checkers()
    {
        IsGrounded();

        IsWalled();

       /* if(isGrounded )
        {
            rbPlayer.useGravity = false;

            rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, 0, rbPlayer.velocity.z);
        }
        else
        {
            rbPlayer.useGravity = true;
        }*/

    }

    private void IsGrounded()
    {
        //CHECKER GROUND
        Collider[] collider;

        collider = Physics.OverlapBox(groundChecker.position, groundSize/2, groundChecker.rotation, groundMask);

        if (collider.Length != 0)
        {
            isGrounded = true;
            ResetJump();

        }
        else
        {
            isGrounded = false;
        }

    }

    private void IsWalled()
    {
        //CHECKER GROUND
        Collider[] collider;

        collider = Physics.OverlapBox(wallChecker.position, wallSize / 2, wallChecker.rotation, wallMask);
        if (collider.Length != 0)
        {
            isTouchingWall = true;
            ResetJump();
        }
        else
        {
            isTouchingWall = false;
        }

    }


    private void Flip()
    {
        if (horizontalAxis > 0)
        {
            graphics.localScale = new Vector3(1, graphics.localScale.y, graphics.localScale.z);
            isRight = true;
        }
        else if (horizontalAxis < 0)
        {
            graphics.localScale = new Vector3(-1, graphics.localScale.y, graphics.localScale.z);
            isRight = false;
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("ATTACK");
            /*int crntLayerIndex = animAtck.GetLayerIndex
            if (animAtck.GetCurrentAnimatorStateInfo())*/

            //if(animAtck.)
            //TODO ATTACK TRIGGER

            animAtck.SetTrigger("Attack");
        }
    }


    public void ReciveDamage(int damageRecived)
    {
        lifePoints -= damageRecived;
        if (lifePoints <= 0) Dead();
    }

    public void ReciveLifes(int lifesRecieved)
    {
        lifePoints += lifesRecieved;
        if(lifePoints >= maxLifes) lifePoints = maxLifes;

    }

    public void LifePontsHealed(int lifePointshealed)
    {
        lifePoints += lifePointshealed;
        if(lifePoints >= maxLifes) lifePoints = maxLifes;
    }

    private void Dead()
    {
        IsDead = true;
        lifes -= 1;
        if (lifes <= 0) GameOver();
    }

    private void GameOver()
    {
        gameOver = true;

    }

    private void Locomotion()
    {
        animMov.SetFloat("Speed", currentSpeed);

    }


    void OnDrawGizmos()//todo lo k esta aki dentro se estara llamando cada frame
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundChecker.position, groundSize/2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallChecker.position, wallSize/2);
    }
}
