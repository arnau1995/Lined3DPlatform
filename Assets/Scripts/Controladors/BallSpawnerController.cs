using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerController : MonoBehaviour
{
    [SerializeField] private float maxTimeBall = 2f;
    [SerializeField] private GameObject ball;
    private float actualTimeBall = 0f;
    float[] ranges;

    void Start()
    {
        BoxCollider box = transform.GetComponent<BoxCollider>();
        ranges = new float[6] {
            box.center.x - box.bounds.extents.x,
            box.center.x + box.bounds.extents.x,
            box.center.y - box.bounds.extents.y,
            box.center.y + box.bounds.extents.y,
            box.center.z - box.bounds.extents.z,
            box.center.z + box.bounds.extents.z,
        };
    }

    // Update is called once per frame
    void Update()
    {
        actualTimeBall += Time.deltaTime;
        if (actualTimeBall >= maxTimeBall) 
        {
            Vector3 position = new Vector3(
                Random.Range( ranges[0], ranges[1] ),
                Random.Range( ranges[2], ranges[3] ),
                Random.Range( ranges[4], ranges[5] )
            );
            
            GameObject newCombi = Instantiate(ball, transform.position+position, Quaternion.identity, transform);
            actualTimeBall = 0;
        }
    }
}
