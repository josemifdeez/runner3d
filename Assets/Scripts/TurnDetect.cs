using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnDetect : MonoBehaviour
{
    public GameManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Turn") || other.tag.Equals("TurnT"))
        {
            TurnControl turn = other.GetComponent<TurnControl>();
            if (other.tag.Equals("Turn")) manager.ActiveTurn(true, turn.newPath);
            else if (other.tag.Equals("TurnT")) manager.ActiveTurn(true, turn.newPath, turn.newEnd);
        }
        if (other.tag.Equals("Trap")) manager.GetDamage();
        if (other.tag.Equals("PowerUp")) manager.ActivateInvincibility();
        if (other.tag.Equals("Coin_5"))
        {
            manager.GetScore(5);
            Destroy(other.gameObject);
        }
        if (other.tag.Equals("Coin_10"))
        {
            manager.GetScore(10);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Turn") || other.tag.Equals("TurnT"))
        {
            manager.ActiveTurn(false, null);
        }
    }
}
