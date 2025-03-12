using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : MonoBehaviour
{
    public List <GameObject> coins;
    public bool anyCoin;
    public float percent;
    void Start()
    {
        float canCreate = Random.Range(0f, 100f);
        if (canCreate < percent)
        {
            if (anyCoin)
            {
                GameObject[] GetTotalCoins = Resources.LoadAll<GameObject>("Coins");
                Instantiate(GetTotalCoins[Random.Range(0, GetTotalCoins.Length)], transform);
            }
            else
            {
                Instantiate(coins[Random.Range(0, coins.Count)], transform);
            }
            
        }
    }

}
