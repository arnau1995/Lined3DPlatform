using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private EnumType type;
    [SerializeField] private float collisionForce = 8f;

    private Vector3 antPosition;
    private Vector3 velocity;
    private enum EnumType{R,Q,L};

    void Start()
    {
        velocity = new Vector3();
        antPosition = transform.position;
    }

    void Update()
    {
        velocity = (transform.position - antPosition) / Time.deltaTime;
        antPosition = transform.position;
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerCollided(collision.gameObject);
        }
        else if (collision.transform.tag == "PlayerMesh")
        {
            GameObject player = findPlayerParent(collision.transform);
            if (player != null) playerCollided(collision.gameObject);
        }
    }*/

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            playerCollided(collider.gameObject);
        }
        else if (collider.transform.tag == "PlayerMesh")
        {
            GameObject player = findPlayerParent(collider.transform);
            if (player != null) playerCollided(player);
        }
    }

    private GameObject findPlayerParent(Transform player)
    {
        while (player != null && player.tag != "Player") 
        {
            player = player.transform.parent;
        }
        
        return player.gameObject;
    }

    public void playerCollided(GameObject player)
    {
        if (type == EnumType.L)
        {
            Application.Quit();
        }
        else
        {
            Vector3 pushDirect = new Vector3();
            Vector3 pushVel = new Vector3();
            ThirdPersonMovement playerMovement = player.GetComponent<ThirdPersonMovement>();
            
            if (type == EnumType.R)
            {
                // Calcular la velocitat de rebot
                pushDirect = (player.transform.position - transform.position).normalized;
            }
            else if (type == EnumType.Q)
            {
                // Calcul distancia amb parent, agafar la distancia més gran (x, y o z)
                pushDirect = (transform.position - transform.parent.position).normalized;
            }

            pushVel = pushDirect * collisionForce;

            // Suma la velocitat que porta l'obstace
            pushVel += velocity;

            // Suma la velocitat que porta el Player (Ja que serà contraria)
            Vector3 playerPushDirect = new Vector3(1 - pushDirect.x, 1 - pushDirect.y, 1 - pushDirect.z);
            pushVel += Vector3.Scale(playerMovement.getVelocity(), playerPushDirect);
            
            // Emputxa al Player
            playerMovement.push(pushVel);
        }
    }
}
