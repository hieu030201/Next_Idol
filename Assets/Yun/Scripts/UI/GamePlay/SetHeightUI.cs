using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeightUI : MonoBehaviour
{
    public GameObject heightTarget;
    public void OnInit()
    {
        if (heightTarget != null)  
        {  
            RectTransform currentRectTransform = GetComponent<RectTransform>();  
            RectTransform targetRectTransform = heightTarget.GetComponent<RectTransform>();  
            if (targetRectTransform != null && currentRectTransform != null)  
            {  
                currentRectTransform.sizeDelta = new Vector2(currentRectTransform.sizeDelta.x, targetRectTransform.sizeDelta.y);  
            }  
        }  
    }
}
