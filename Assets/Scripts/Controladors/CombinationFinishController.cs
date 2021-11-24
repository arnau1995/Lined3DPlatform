using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationFinishController : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") 
        {
            Transform targetObject = transform;
            while (targetObject != null && targetObject.tag != "Combination") targetObject = targetObject.transform.parent;
            if (targetObject != null) targetObject.GetComponent<CombinationController>().playerExit(other.gameObject);
        }
    }
}
