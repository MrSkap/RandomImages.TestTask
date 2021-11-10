using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;

public class Downloader : MonoBehaviour
{
    [SerializeField]
    int counter = 0;
    const string url = "https://picsum.photos/200";
    CardOpener cardOpener;
    public ModeOfWork mode;
    string imagePath = "Assets/Resources/Images/";
    void Start()
    {
        cardOpener = GetComponent<CardOpener>();
        counter = 0;
    }

    void DownloadImageAsync(string imageName)
	{
        WebClient client = new WebClient();
        client.DownloadFileAsync(new System.Uri(url), imagePath + imageName.ToString()+ ".png");
		client.DownloadFileCompleted += Client_DownloadFileCompleted;
	}


    private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
	{
        
		switch (mode)
		{
            case ModeOfWork.AllByOne:
                cardOpener.ReadyCard[counter] = true;
                counter++;
                break;
            case ModeOfWork.Series:
                if (counter < cardOpener.ReadyCard.Length)
                {
                    cardOpener.ReadyCard[counter] = true;
                    counter++;
                }
                break;
            case ModeOfWork.ByReady:
                cardOpener.ReadyCard[counter] = true;
                TurnOnByCompleted(counter);
                counter++;
                break;
            default:
                break;
        }
    }


    void TurnOnByCompleted(int numberOfImage)
	{
        StartCoroutine(cardOpener.OpenCard(numberOfImage));
	}

    public void LoadImagesAsync(int CountOfImages)
	{
        counter = 0;
        for(int i = 0; i < CountOfImages; i++)
		{
            DownloadImageAsync(i.ToString());
        }
	}

    public IEnumerator StartDownload(int countOfImages)
	{
        Debug.Log("in start");
        yield return StartCoroutine(ResetAll());
        switch (mode)
        {
            case ModeOfWork.AllByOne:
                StartCoroutine(LoadAll(countOfImages));
                break;
            case ModeOfWork.Series:
                StartCoroutine(LoadOneByOne());
                break;
            case ModeOfWork.ByReady:
                StartCoroutine(LoadByReady(countOfImages));
                break;
            default:
                break;
        }
    }

    IEnumerator LoadByReady(int countOfImages)
	{
        yield return StartCoroutine(ResetAll());
        LoadImagesAsync(countOfImages);

    }
    IEnumerator LoadAll(int countOfImages)
	{
        yield return StartCoroutine(ResetAll());
        StartCoroutine(cardOpener.OpenAllCards());
        LoadImagesAsync(countOfImages);
    }
    IEnumerator LoadOneByOne()
	{
        yield return StartCoroutine(ResetAll());
        bool allNotOpen = true;
        int numberOfLoadedImage = 0;
        DownloadImageAsync(numberOfLoadedImage.ToString());
		while (allNotOpen)
		{
            for (int i = numberOfLoadedImage; i< cardOpener.ReadyCard.Length; i++)
			{
                if (cardOpener.ReadyCard[i] == true)
                {
                    cardOpener.ReadyCard[numberOfLoadedImage] = true;
                    yield return StartCoroutine(cardOpener.OpenCard(numberOfLoadedImage));
                    numberOfLoadedImage++;
                    DownloadImageAsync(numberOfLoadedImage.ToString());
                }
			}
            if (numberOfLoadedImage == cardOpener.ReadyCard.Length)
            {
                allNotOpen = false;
            }
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Done!");
    }
    public IEnumerator ResetAll()
	{
        Debug.Log("in reset");
        counter = 0;
        for (int i = 0; i < cardOpener.ReadyCard.Length; i++)
        {
            cardOpener.ReadyCard[i] = false;
            cardOpener.opendCards[i] = false;
        }
        cardOpener.allAreOpend = false;
        yield return StartCoroutine(cardOpener.AllCardsFlipBack());
    }
}
