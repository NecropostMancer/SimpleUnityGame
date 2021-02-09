using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBundle : MonoBehaviour
{
    public abstract void SendCommand(UICommand command);

    private void OnDestroy()
    {
        GameUIManager.instance.RemoveUI();
    }
}
