using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameCard : MonoBehaviour
{
    [SerializeField]
    private int value = 1; //default value

    public int Value => value;
}

