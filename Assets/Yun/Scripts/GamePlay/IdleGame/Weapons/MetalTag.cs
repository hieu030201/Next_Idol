using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Managers;

public class MetalTag : GameUnit
{
    public float liftHeight = 3f; // Chiều cao bay lên  
    public float rotateSpeed = 100f; // Tốc độ xoay  
    public float fallDuration = 1f;
    
    private Vector3 startPosition;  
    private bool isFlying = false;  
    public void OnInit()
    {
        startPosition = transform.position; // Lưu vị trí ban đầu  
        StartCoroutine(FlyAndFall());  
    }
    
    private IEnumerator FlyAndFall()  
    {  
        float randomRadius = 1f; // Bán kính của vùng văng  
        Vector3 randomOffset = new Vector3(  
            Random.Range(-randomRadius, randomRadius),  
            0, // Giữ đối tượng trên mặt phẳng nếu không muốn lệch trục Y  
            Random.Range(-randomRadius, randomRadius)  
        );  

        Vector3 targetPosition = startPosition + new Vector3(0, liftHeight, 0);  
        float elapsedTime = 0f;  
        float totalDuration = 1f;   
        Vector3 arcPeak = startPosition + (randomOffset / 2) + new Vector3(0, liftHeight, 0);  
    
        // Tạo một giá trị quay ngẫu nhiên  
        float randomRotationY = Random.Range(0f, 360f);  
    
        while (elapsedTime < totalDuration)  
        {  
            float t = elapsedTime / totalDuration;  
            Vector3 flyCurvePosition = Vector3.Lerp(  
                Vector3.Lerp(startPosition, arcPeak, t),  
                Vector3.Lerp(arcPeak, startPosition + randomOffset, t),  
                t  
            );  

            transform.position = flyCurvePosition;  

            // Áp dụng quay ngẫu nhiên quanh trục Y  
            transform.rotation = Quaternion.Euler(0, randomRotationY + (rotateSpeed * elapsedTime), 0);  

            elapsedTime += Time.deltaTime;  
            yield return null;  
        }  
    
        transform.position = startPosition + randomOffset;  
    }  
    public void MovingToTarget(GameObject metalTag , Transform target)  
    {  
        Vector3 targetPosition = target.transform.position + new Vector3(0, 0.5f, 0);  
        float distance = Vector3.Distance(metalTag.gameObject.transform.position, targetPosition);  
            
        float timeToMove = distance / 20.0f;
            
        metalTag.gameObject.transform.DOMove(targetPosition, timeToMove)  
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                OnDespawn();
            });  
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}
