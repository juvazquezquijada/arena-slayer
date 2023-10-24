using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostFX : MonoBehaviour
{
    private static PostFX instance;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed on scene change

        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
}
