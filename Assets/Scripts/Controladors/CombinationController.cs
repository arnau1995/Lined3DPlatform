using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationController : MonoBehaviour
{
    [SerializeField] private int maxPlayerDistance = 80;
    private bool delete = false;
    private GameObject player;

    public void playerExit(GameObject playerCollide)
    {
        GameManager.Instance.createNewCombi();
        player = playerCollide;
        delete = true;
    }

    private void Update()
    {
        if (delete)
        {
            // Mirar la pos del player
            if (player.transform.position.z - transform.position.z > maxPlayerDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    public Vector3 getStartPos()
    {
        return transform.Find("Start").position;
    }

    public Vector3 getFinishPos()
    {
        return transform.Find("Finish").position;
    }

    public void centerStartPos()
    {
        transform.position = new Vector3(transform.position.x + (transform.position.x - getStartPos().x), transform.position.y + (transform.position.y - getStartPos().y), transform.position.z + (transform.position.z - getStartPos().z));
    }
}
