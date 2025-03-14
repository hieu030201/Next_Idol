using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilAmount = 0.5f;  
    public float recoilDownAmount = 0.3f; 
    public float recoilDuration = 0.1f; 
    public float returnSpeed = 2.0f; 
    public float recoilSideAmount = 0.1f; 

    private Vector3 originalPosition; 
    private float recoilTimer; 

    void Start()  
    {  
        originalPosition = transform.localPosition;
    }  

    void Update()  
    {  
        if (recoilTimer > 0)  
        {  
            transform.localPosition += new Vector3(0, recoilAmount, -recoilDownAmount);   
            
            float sideRecoil = Random.Range(-recoilSideAmount, recoilSideAmount);  
            transform.localPosition += new Vector3(sideRecoil, 0, 0);   

            recoilTimer -= Time.deltaTime; 
            
            if (recoilTimer <= 0)  
            {  
                recoilTimer = 0; 
            }  
        }  
        else  
        {  
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, returnSpeed * Time.deltaTime);  
        }  
    }  
    
    public void Fire()  
    {  
        recoilTimer = recoilDuration; 
    }  
   
        
}
