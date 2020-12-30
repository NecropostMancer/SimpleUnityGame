using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBundle : MonoBehaviour
{
    protected GameUIManager gameUIManager;
    public abstract void SendCommand(UICommand command);

    public void InjectManager(GameUIManager manager)
    {
        gameUIManager = manager;
    }
}
