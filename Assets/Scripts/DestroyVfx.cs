using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVfx : MonoBehaviour
{

    void Update()
    {
        Invoke("DestroyObject", 1.5f);
        
    }
    private void DestroyObject()
    {
        Destroy(gameObject);    
    }

}
