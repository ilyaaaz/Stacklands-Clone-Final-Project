using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.coinNum++;
        GameManager.instance.CoinUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
