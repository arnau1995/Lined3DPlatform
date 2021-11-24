using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerController : MonoBehaviour
{
    [SerializeField] private float maxTimeBall = 2f;
    private float actualTimeBall = 0f;
    [SerializeField] private GameObject ball;

    // Update is called once per frame
    void Update()
    {
        actualTimeBall += Time.deltaTime;
        if (actualTimeBall >= maxTimeBall) 
        {
            GameObject newCombi = Instantiate(ball, transform.position, Quaternion.identity, transform);
            actualTimeBall = 0;
        }
    }
}
