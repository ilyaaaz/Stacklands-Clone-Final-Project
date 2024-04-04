using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.cardNum++;
        GameManager.instance.StorageUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
