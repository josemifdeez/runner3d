using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvControl : MonoBehaviour
{
    public List<Renderer> mesh;
    public List<Color> invColor;
    int currentColor;
    public float stopInv;
    public void ActiveInv()
    {
        CancelInvoke(nameof(StopInv));
        InvokeRepeating(nameof(SetColorInv), 0, 0.25f);
        Invoke(nameof(StopInv), stopInv);
    }
    public void SetColorInv()
    {
        currentColor++;
        if(currentColor >= invColor.Count)
        {
            currentColor = 0;
        }
        for (int i = 0; i < mesh.Count; i++)
        {
            mesh[i].material.color = invColor[currentColor];
        }
    }

    public void StopInv()
    {
        CancelInvoke(nameof(SetColorInv));
        for (int i = 0; i < mesh.Count; i++)
        {
            mesh[i].material.color = invColor[0];
        }
    }
}
