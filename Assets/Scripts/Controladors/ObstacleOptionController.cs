using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleOptionController : MonoBehaviour
{
    [SerializeField] private string optionSingle = "Obstacle Option Single";
    [SerializeField] private string optionFull = "Obstacle Option Full";

    // Start is called before the first frame update
    void Start()
    {
        showObstacleOptions(transform);
    }

    void showObstacleOptions(Transform targetObject)
    {
        if (targetObject.tag == optionSingle || targetObject.tag == optionFull)
        {
            int childShow = -1; // Es mostren tots els fills
            if (targetObject.tag == optionSingle)
            {
                childShow = Random.Range(0, targetObject.childCount); // Es mostra només un dels fills
            }
            
            for (int i = 0; i < targetObject.childCount; i++)
            {
                Transform targetChild = targetObject.GetChild(i);
                if (childShow == -1 || childShow == i) 
                {
                    targetChild.gameObject.SetActive(true);
                    showObstacleOptions(targetChild);
                }
                else targetChild.gameObject.SetActive(false);
            }
        }
    }
}
