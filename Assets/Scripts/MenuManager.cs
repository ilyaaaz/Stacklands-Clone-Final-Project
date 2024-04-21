using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI foodMenuText, coinMenuText, cardMenuText, titleText, detailedText;

    //food
    public void FoodChangeFont()
    {  
        InvokeRepeating("FoodMenuFont", 0, Time.deltaTime); 
    }

    void FoodMenuFont()
    {
        foodMenuText.fontStyle = FontStyles.Bold | FontStyles.Underline;
        titleText.text = "FOOD";
        detailedText.text = "Every Moon you need " + "\n 0" + "  <sprite index=1> to feed your Villagers. You currently have " + "0 " + " <sprite index=1>";

    }

    public void FoodChangeBack()
    {
        foodMenuText.fontStyle = FontStyles.Bold;
        titleText.text = "";
        detailedText.text = ""; 
        CancelInvoke();
    } 


    //coin
    public void CoinChangeFont()
    {  
        InvokeRepeating("CoinMenuFont", 0, Time.deltaTime); 
    }

    void CoinMenuFont()
    {
        coinMenuText.fontStyle = FontStyles.Bold | FontStyles.Underline;
        titleText.text = "COINS";
        detailedText.text = "The total amount of Coins you have"; 
    }

    public void CoinChangeBack()
    {
        coinMenuText.fontStyle = FontStyles.Bold;
        titleText.text = "";
        detailedText.text = ""; 
        CancelInvoke();
    }


    //card
    public void CardChangeFont()
    {  
        InvokeRepeating("CardMenuFont", 0, Time.deltaTime); 
    }

    void CardMenuFont()
    {
        cardMenuText.fontStyle = FontStyles.Bold | FontStyles.Underline;
        titleText.text = "CARD CAP";
        detailedText.text = "Your current maximum amount of Cards is 20. You have " + " 0" + "  <sprite index=2> (does not include Coins)"; 
    }

    public void CardChangeBack()
    {
        cardMenuText.fontStyle = FontStyles.Bold;
        titleText.text = "";
        detailedText.text = ""; 
        CancelInvoke();
    }
}
