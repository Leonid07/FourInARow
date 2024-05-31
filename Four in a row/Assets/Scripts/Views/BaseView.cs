using UnityEngine;

public class BaseView : MonoBehaviour
{

    protected void ToggleVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public virtual void Show()
    {
        ToggleVisibility(true);
    }

    public virtual void Hide()
    {
        ToggleVisibility(false);
    }    
}
