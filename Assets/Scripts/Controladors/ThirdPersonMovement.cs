using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Main
    [Header("Main")]
    [SerializeField] private float maxVelocityToCollide = 2f;
    private Animator animator;
    private Vector3 velocity;

    // Moviment frontal
    [Header("Moviment frontal")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float timeToStandUp = 1f;
    [SerializeField] private float velocityToStandUp = .1f;
    private float speed = 0f;
    private CharacterController controller;
    private bool applyMovement = true;
    private float timeSinceDrop = 0f;

    // Moviment lateral
    [Header("Moviment lateral")]
    [SerializeField] private float speedLateralPercent = .5f;
    private float speedLateral = 0f;
    private float direccioLateral = 0f;

    // Moviment vertical
    [Header("Moviment vertical")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = .4f;
    [SerializeField] private float jumpHeight = 1.25f;
    private Vector3 gravityVelocity;
    private bool isGrounded = false;

    // Colisions
    [Header("Colisions")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private Vector3 pushVelocity;
    [SerializeField] private float airDrag = 1f;
    [SerializeField] private float floorDrag = 6f;

    /*
    // GLOBAL
    */
    void Start()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        velocity = new Vector3();

        animator = transform.Find("Mesh").GetComponent<Animator>();
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 prevPos = transform.position;

        // Moviment Push
        if (!applyMovement)
        {
            // La velocitat ha de tendir a 0
            pushVelocity.x += getDrag(pushVelocity.x) * Time.deltaTime;
            if (Mathf.Abs(pushVelocity.x) < .01f) pushVelocity.x = 0;
            
            pushVelocity.y += getAirDrag(pushVelocity.y) * Time.deltaTime;
            if (Mathf.Abs(pushVelocity.y) < .01f) pushVelocity.y = 0;
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && pushVelocity.y < 0)
            {
                pushVelocity.y = -1f;
            }
            pushVelocity.y += gravity * Time.deltaTime;
            
            pushVelocity.z += getDrag(pushVelocity.z) * Time.deltaTime;
            if (Mathf.Abs(pushVelocity.z) < .01f) pushVelocity.z = 0;

            controller.Move(pushVelocity * Time.deltaTime);

            // Calculem la velocitat lateral
            speedLateral = Mathf.Abs(pushVelocity.z) * speedLateralPercent;
        }
        // Fi Moviment Push

        // Gravity
        else
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && gravityVelocity.y < 0)
            {
                gravityVelocity.y = -2f;
            }

            gravityVelocity.y += gravity * Time.deltaTime;
            controller.Move(gravityVelocity * Time.deltaTime);

            // Calculem la velocitat frontal
            if (speed < maxSpeed) speed += acceleration * Time.deltaTime;

            // Calculem la velocitat lateral
            speedLateral = speed * speedLateralPercent;
        }
        // Fi Gravity

        // Moviment Jugador
        moveForward();
        moveSide();

        Vector3 newPos = transform.position;
        velocity = (newPos - prevPos) / Time.deltaTime;
        // Fi Moviment Jugador
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }
    /*
    // FI GLOBAL
    */

    /*
    // MOVE FORWARD 
    */
    private void moveForward()
    {
        if (applyMovement)
        {
            Vector3 direction = new Vector3(0f, 0f, 1f).normalized;
            controller.Move(direction * speed * Time.deltaTime);
        }
        else
        {
            // El Player s'aixeca si té una velocitat molt baixa i toca al terra
            if (playerCanStandUp())
            {
                animator.enabled = true;
                animator.SetBool("StandUp", true);
            }
        }
    }

    public void increaseSpeed(float percent) // % 0 a 1
    {
        maxSpeed += (maxSpeed * percent);
        maxSpeed = GameManager.Instance.roundNumber(percent, 4);
    }

    public bool playerCanStandUp()
    {
        timeSinceDrop += Time.deltaTime;
        if (timeSinceDrop >= timeToStandUp && isGrounded) // Minimum time to stand up
        {
            if (velocity.x < velocityToStandUp && velocity.y < velocityToStandUp && velocity.z < velocityToStandUp) return true;
        }
        
        return false;
    }
    /*
    // FI MOVE FORWARD 
    */

    /*
    // MOVE SIDED 
    */
    private void moveSide()
    {
        //if (speedLateral < maxSpeedLateral) speedLateral += accelerationLateral * Time.deltaTime;
        //if (applyMovement && direccioLateral != 0)
        if (direccioLateral != 0)
        {
            Vector3 direction = new Vector3(1f, 0f, 0f).normalized;
            controller.Move((direction * direccioLateral) * speedLateral * Time.deltaTime);
        }
    }

    public void setSide(float side) // 0 a 1
    {
        direccioLateral = side;
    }
    /*
    // FI MOVE SIDED 
    */

    /*
    // MOVE VERTICAL 
    */
    public void jump()
    {
        if (isGrounded)
        {
            gravityVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    /*
    // FI MOVE VERTICAL 
    */

    /*
    // COLLISIONS 
    */
    /*void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if( ((1<<hit.gameObject.layer) & collisionMask) != 0 && (velocity.x >= maxVelocityToCollide || velocity.z >= maxVelocityToCollide) )
        {
            hit.transform.GetComponent<ObstacleController>().playerCollided(gameObject);
        }
    }*/

    public void push(Vector3 vel)
    {
        speed = 0f;
        speedLateral = 0f;
        animator.enabled = false;
        applyMovement = false;
        timeSinceDrop = 0f;
        
        pushVelocity = vel;
    }

    public void recover()
    {
        //animator.enabled = true;
        animator.SetBool("StandUp", false);
        applyMovement = true;
    }

    public void setFloorDrag(float drag)
    {
        floorDrag = drag;
    }
    private float getDrag(float val)
    {
        float drag = airDrag;
        if (isGrounded) drag += floorDrag;
        if (val > .01f) return -drag;
        else if (val < .01f) return drag;
        else return 0;
    }

    private float getAirDrag(float val)
    {
        if (val > .01f) return -airDrag;
        else if (val < .01f) return airDrag;
        else return 0;
    }
    /*
    // FI COLLISIONS 
    */
}
