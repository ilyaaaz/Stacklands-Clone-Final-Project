using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip cardPickUp, cardDrop, cardVillager, cardCoin, openPack, cardSell, buyPack, produceCard, stackCard, moon, feedVillager, attack, attackmiss;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCardPickUp()
    {
        audioSource.PlayOneShot(cardPickUp);
    }
    public void PlayCardDrop()
    {
        audioSource.PlayOneShot(cardDrop);
    }
    public void PlayVillagerCard()
    {
        audioSource.PlayOneShot(cardVillager);
    }
    public void PlayCoinCard()
    {
        audioSource.PlayOneShot(cardCoin);
    }
    public void PlayOpenPack()
    {
        audioSource.PlayOneShot(openPack);
    }
    public void PlayCardSell()
    {
        audioSource.PlayOneShot(cardSell);
    }
    public void PlayBuyPack()
    {
        audioSource.PlayOneShot(buyPack);
    }
    public void PlayProduceCard()
    {
        audioSource.PlayOneShot(produceCard);
    }
    public void PlaystackCard()
    {
        audioSource.PlayOneShot(stackCard);
    }
    public void Playmoon()
    {
        audioSource.PlayOneShot(moon);
    }
    public void PlayfeedVillager()
    {
        audioSource.PlayOneShot(feedVillager);
    }
    public void Playattack()
    {
        audioSource.PlayOneShot(attack);
    }
    public void Playattackmiss()
    {
        audioSource.PlayOneShot(attackmiss);
    }
}
