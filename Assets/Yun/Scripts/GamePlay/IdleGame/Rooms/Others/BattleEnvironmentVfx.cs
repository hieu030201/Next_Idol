using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms.Others
{
    public class EnvironmentBattleVfx : MonoBehaviour
    {
        public ParticleSystem[] vfxPrefabs;
        public float scaleFactor = 1.2f;    
        public float duration = 2.0f;       

        public void PlayVFX(int index, Transform position, Quaternion euler)  
        {  
            if (index < 0 || index >= vfxPrefabs.Length)  
            {  
                Debug.LogError("Chỉ số VFX không hợp lệ.");  
                return;  
            }  
            
            ParticleSystem vfxInstance = Instantiate(vfxPrefabs[index], position.position, euler);  
            StartCoroutine(ScaleVFX(vfxInstance.transform));  
        }  
        
        private IEnumerator ScaleVFX(Transform vfxTransform)  
        {  
            Vector3 initialScale = vfxTransform.localScale;   
            Vector3 targetScale = initialScale * scaleFactor;  

            for (float t = 0; t < duration; t += Time.deltaTime)  
            {  
                float normalizedScale = t / duration;  
                vfxTransform.localScale = Vector3.Lerp(initialScale, targetScale, normalizedScale);  
                yield return null;   
            }  
            vfxTransform.localScale = targetScale;  
        }  
    }
}

