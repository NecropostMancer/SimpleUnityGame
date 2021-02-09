using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvents :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void _onPointerEnter();
    public _onPointerEnter onPointerEnter;
    public delegate void _onPointerExit();
    public _onPointerExit onPointerExit;
    public delegate void _onPointerClick();
    public _onPointerClick onPointerClick;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onPointerEnter != null)
        {
            onPointerEnter();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onPointerExit != null)
        {
            onPointerExit();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onPointerClick != null)
        {
            onPointerClick();
        }
    }
}
