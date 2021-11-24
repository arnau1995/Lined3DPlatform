using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager is NULL");

            return _instance;
        }
    }

    // Spawner
    [SerializeField] private GameObject map;
    [System.Serializable] public class mapCombination {
        public int percent;
        public GameObject combi;
    }
    [SerializeField] private mapCombination[] mapCombinations;
    [SerializeField] private int startSpawnCombi = 3;
    private int maxPercent;
    private GameObject lastCombi;
    private int idLastCombi;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // Guardem el nombre màxim per calcular els % de cada combinacio
        maxPercent = 0;
        //int index = 0;
        foreach(mapCombination mapCombi in mapCombinations)
        {
            maxPercent += mapCombi.percent;
            //index++;
        }

        lastCombi = map.transform.Find("Inici").gameObject;
        for (int i = 0; i < startSpawnCombi; i++)
        {
            createNewCombi();
        }
    }

    public float roundNumber(float valor, int decimals)
    {
        decimals = 10 ^ decimals;
        return Mathf.Round(valor * decimals) / decimals;
    }

    public void createNewCombi()
    {
        Vector3 position = lastCombi.GetComponent<CombinationController>().getFinishPos();
        int resultPercent = Random.Range(0, maxPercent+1);
        idLastCombi = findCombiByPercent(resultPercent);
        
        GameObject newCombi = Instantiate(mapCombinations[idLastCombi].combi, position, Quaternion.identity, map.transform);
        newCombi.GetComponent<CombinationController>().centerStartPos();

        lastCombi = newCombi;
    }

    private int findCombiByPercent(int resultPercent)
    {
        int i = 0;
        int combiPercent = 0;
        bool found = false;
        while (!found && i < mapCombinations.Length)
        {
            combiPercent += mapCombinations[i].percent;
            if (resultPercent <= combiPercent)
            {
                found = true;
            }
            else i++;
        }

        return i;
    }
}
