using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class CardOpener : MonoBehaviour
{
    public GameObject[] cards;
    public Image[] images;
    float imageWidth;
    float imageHeight;
    const int countOfCards = 5;
    public bool[] ReadyCard = new bool[countOfCards];

    void Start()
    {
        imageHeight = images[0].sprite.rect.height;
        imageWidth = images[0].sprite.rect.width;

        foreach(var card in cards)
		{
            card.GetComponent<Card>().FinishAnimation.AddListener(onCompliteAnim);

        }
    }


    public IEnumerator OpenCard(int numberOfCard)
	{
        while (ReadyCard[numberOfCard] == false) yield return new WaitForFixedUpdate();
        AssetDatabase.Refresh();
        images[numberOfCard].sprite = Resources.Load<Sprite>("Images/" + numberOfCard.ToString());
        cards[numberOfCard].GetComponent<Card>().FlipToFront();
        yield return StartCoroutine(WaitUntilCardAnimEnds(numberOfCard));
	}
    
    public IEnumerator OpenAllCards()
	{
        bool ready = false;
        int counter = 0;
        while (!ready)
        {
            counter = 0;
            for (int i = 0; i< ReadyCard.Length; i++)
		    {
			    if (ReadyCard[i])
			    {
                counter++;
			    }
		    }
            if (counter == countOfCards) ready = true;
            yield return new WaitForFixedUpdate();
		}
        AssetDatabase.Refresh();
        for(int i = 0; i < images.Length; i++)
		{
            images[i].sprite = Resources.Load<Sprite>("Images/" + i.ToString());
        }
        foreach(var card in cards)
		{
            card.GetComponent<Card>().FlipToFront();
		}
        yield return StartCoroutine(WaitUntilAllWillBeComplited());
    }

    public IEnumerator AllCardsFlipBack()
    {
        StopAllCoroutines();
        complitedAnimation = false;
        StartCoroutine(WaitUntilAllWillBeComplited());
        foreach (var card in cards)
        {
            card.GetComponent<Card>().FlipToBack();
        }
        while (countOfComplitedAnimation < cards.Length - 1 && !complitedAnimation)
        {
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < ReadyCard.Length; i++)
        {
            ReadyCard[i] = false;
        }
    }

    int countOfComplitedAnimation = 0;
    bool complitedAnimation = false;
    public IEnumerator WaitUntilAllWillBeComplited()
	{
        countOfComplitedAnimation = 0;
        while (countOfComplitedAnimation < cards.Length-1)
		{
            yield return new WaitForFixedUpdate();
		}
        complitedAnimation = true;

    }


    public bool allAreOpend = false;
    public bool[] opendCards = new bool[countOfCards];
    public IEnumerator WaitUntilAllWillBeReady()
    {
        allAreOpend = false;
        for (int i = 0; i < countOfCards; i++)
		{
            opendCards[i] = false;
            StartCoroutine(WaitUntilCardAnimEnds(i));
        }
		while (!allAreOpend)
		{
            int openedCards = 0;
            for (int i = 0; i < countOfCards; i++)
                if (cards[i].GetComponent<Card>().isOpend == true)
                    openedCards++;
            if (openedCards == countOfCards)
                allAreOpend = true;
            yield return new WaitForFixedUpdate();
        }    


    }

    public IEnumerator WaitUntilCardAnimEnds(int numberOfCard)
	{
        while (cards[numberOfCard].GetComponent<Card>().isOpend == false)
        {
            yield return new WaitForFixedUpdate();
        }
        opendCards[numberOfCard] = true;
    }

    void onCompliteAnim()
	{
        countOfComplitedAnimation++;
    }
}
