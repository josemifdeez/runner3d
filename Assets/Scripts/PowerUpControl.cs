using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpControl : MonoBehaviour
{
    public List <GameObject> powerups;
    public bool anyPowerUp;
    public float percent;
    void Start()
    {
        float canCreate = Random.Range(0f, 100f);
        if (canCreate < percent)
        {
            if (anyPowerUp)
            {
                GameObject[] GetTotalPowers = Resources.LoadAll<GameObject>("PowerUps");
                Instantiate(GetTotalPowers[Random.Range(0, GetTotalPowers.Length)], transform);
            }
            else
            {
                Instantiate(powerups[Random.Range(0, powerups.Count)], transform);
            }
            
        }
    }

}
