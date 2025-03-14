using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHeightContentUI : MonoBehaviour
{
    [SerializeField] private GameObject targetHeightA;
    [SerializeField] private GameObject targetHeightB;

    [SerializeField] private GameObject targetWidthA;
    [SerializeField] private GameObject targetWidthB;
    
    [SerializeField] private GameObject target2WidthA;
    [SerializeField] private GameObject target2WidthB;
    public void OnInit()
    {
        if (targetHeightA != null && targetHeightB != null)
        {
            // Get the RectTransforms  
            RectTransform rectA = targetHeightA.GetComponent<RectTransform>();
            RectTransform rectB = targetHeightB.GetComponent<RectTransform>();

            if (rectA != null && rectB != null)
            {
                float totalHeight = rectA.sizeDelta.y + rectB.sizeDelta.y;
                RectTransform currentRectTransform = GetComponent<RectTransform>();
                if (currentRectTransform != null)
                {
                    currentRectTransform.sizeDelta = new Vector2(currentRectTransform.sizeDelta.x, totalHeight);
                }
            }
        }

        // if (targetWidthA != null && targetWidthB != null)
        // {
        //     RectTransform rectWidthA = targetWidthA.GetComponent<RectTransform>();
        //     RectTransform rectWidthB = targetWidthB.GetComponent<RectTransform>();
        //     
        //     RectTransform rectTypeA = target2WidthA.GetComponent<RectTransform>();
        //     RectTransform rectTypeB = target2WidthB.GetComponent<RectTransform>();
        //     float screenRatio = (float)Screen.height / (float)Screen.width;  
        //
        //     // Adjust the aspect ratio based on screen dimensions  
        //     if (screenRatio >= 2.0f) // For ultra-wide screens (e.g., 21:9)  
        //     {  
        //         //rectWidthA.sizeDelta = 330; // Set to 2.0  
        //     }  
        //     else if (screenRatio >= 1.78f) // ~16:9  
        //     {  
        //         //aspectRatioFitter.aspectRatio = 1.5f; // Set to 16:9  
        //     }  
        //     else if (screenRatio >= 1.3f) 
        //     {  
        //         rectWidthA.sizeDelta = new Vector2(320, rectWidthA.sizeDelta.y); 
        //         rectWidthB.sizeDelta = new Vector2(320, rectWidthA.sizeDelta.y); 
        //         
        //         rectTypeA.sizeDelta = new Vector2(340, rectWidthA.sizeDelta.y); 
        //         rectTypeB.sizeDelta = new Vector2(340, rectWidthA.sizeDelta.y); 
        //     }  
        //     else if (screenRatio >= 1.33f) // ~4:3  
        //     {  
        //         //aspectRatioFitter.aspectRatio = 6f; // Set to 4:3  
        //     }  
        //     else  
        //     {  
        //         //aspectRatioFitter.aspectRatio = 1.0f; // Set to 1:1 for very narrow screens  
        //     }

        // }
       
    }
}
