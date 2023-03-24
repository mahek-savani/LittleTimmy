using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    
    public Slider slider;
    // Start is called before the first frame update

            public float ShakeMagnitude = 5f;
        public float ShakeTime = 0.3f;
 
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value= health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public IEnumerator ShakeHealth()
    {
        Vector3 originalposition = transform.position;

        float t = 0.0f;

        
        while (t < ShakeTime)
        {
            float x = originalposition.x + Random.Range(-1,1) * ShakeMagnitude;
            float y = originalposition.y + Random.Range(-1,1) * ShakeMagnitude;


            transform.position = new Vector3(x, y, originalposition.z);
            t += Time.deltaTime;

            yield return null;

                
            
        }

         transform.position = originalposition;
    }
}
