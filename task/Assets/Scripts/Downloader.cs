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
    // Start is called before the first frame update
    CardOpener cardOpener;
    public ModeOfWork mode;
    string imagePath = "C:/Users/User/Documents/GitHub/ExpertSystem/RandomImages.TestTask/task/Assets/Resources/Images/";
    void Start()
    {
        cardOpener = GetComponent<CardOpener>();
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DownloadImageAsync(string imageName)
	{
        WebClient client = new WebClient();
        client.DownloadFileAsync(new System.Uri(url), imagePath + imageName.ToString()+ ".png");
		client.DownloadFileCompleted += Client_DownloadFileCompleted;
	}

    void DownloadImage(string imageName)
    {
        WebClient client = new WebClient();
        client.DownloadFile(new System.Uri(url), imageName + imageName.ToString() + ".png");
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
                if(counter< cardOpener.ReadyCard.Length) cardOpener.ReadyCard[counter] = true;
                break;
            case ModeOfWork.ByReady:
                TurnOnByCompleted(counter);
                counter++;
                break;
            default:
                break;
        }
    }


    void TurnOnByCompleted(int numberOfImage)
	{
        cardOpener.OpenCard(numberOfImage);
	}

    public void LoadImagesAsync(int CountOfImages)
	{
        counter = 0;
        for(int i = 0; i < CountOfImages; i++)
		{
            DownloadImageAsync(i.ToString());
        }
	}

    public void StartDownload(int countOfImages)
	{
        ResetAll();
        switch (mode)
        {
            case ModeOfWork.AllByOne:
                StartCoroutine(cardOpener.OpenAllCards());
                LoadImagesAsync(countOfImages);
                break;
            case ModeOfWork.Series:
                StartCoroutine(LoadOneByOne());
                break;
            case ModeOfWork.ByReady:
                LoadImagesAsync(countOfImages);
                break;
            default:
                break;
        }
    }

    IEnumerator LoadOneByOne()
	{
        bool allNotOpen = true;
        int numberOfLoadedImage = 0;
        DownloadImageAsync(numberOfLoadedImage.ToString());
        cardOpener.ReadyCard[0] = true;
		while (allNotOpen)
		{
            
            for (int i = numberOfLoadedImage; i< cardOpener.ReadyCard.Length; i++)
			{
                if (cardOpener.ReadyCard[i] == true)
                {
                    cardOpener.ReadyCard[numberOfLoadedImage] = true;
                    //Debug.Log(i.ToString() + " READY");
                    cardOpener.OpenCard(numberOfLoadedImage);
                    numberOfLoadedImage++;
                    counter++;
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

    void ResetAll()
	{
        StopAllCoroutines();
        counter = 0;
        for (int i = 0; i < cardOpener.ReadyCard.Length; i++)
        {
            cardOpener.ReadyCard[i] = false;
        }
    }
}
