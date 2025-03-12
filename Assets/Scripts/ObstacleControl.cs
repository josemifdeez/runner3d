using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{
    public List <GameObject> obstacles;
    public bool anyTrap;
    public float percent;
    void Start()
    {
        float canCreate = Random.Range(0f, 100f);
        if (canCreate < percent)
        {
            if (anyTrap)
            {
                GameObject[] GetTotalTraps = Resources.LoadAll<GameObject>("Traps");
                Instantiate(GetTotalTraps[Random.Range(0, GetTotalTraps.Length)], transform);
            }
            else
            {
                Instantiate(obstacles[Random.Range(0, obstacles.Count)], transform);
            }
            
        }
    }

}
