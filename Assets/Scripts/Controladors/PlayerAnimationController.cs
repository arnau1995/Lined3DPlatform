using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private ThirdPersonMovement player;

    // Start is called before the first frame update
    void Start()
    {
        Transform target = transform;
        while(target != null && target.tag != "Player") target = target.parent;
        if (target != null) player = target.GetComponent<ThirdPersonMovement>();
    }

    private void onAnimationStandUpFinish()
    {
        if (player != null) player.recover();
    }
}
