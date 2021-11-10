using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeScript : MonoBehaviour
{
    public Dropdown dropdown;
    public Downloader downloader;
    public Button startButton;
    public Button cancelButton;
    int imagesCount = 5;
    public CardOpener cardOpener;

    
	private void Start()
	{
        downloader.mode = ModeOfWork.AllByOne;
        cancelButton.interactable = false;
    }
	public void ModeChanged()
	{
        downloader.ResetAll();
        switch (dropdown.value){
            case 0:
                downloader.mode = ModeOfWork.AllByOne;
                break;
            case 1:
                downloader.mode = ModeOfWork.Series;
                break;
            case 2:
                downloader.mode = ModeOfWork.ByReady;
                break;
            default:
                break;
        }
	}

    public void StartDownload()
	{
        StartCoroutine(downloader.StartDownload(imagesCount));
        StartCoroutine(MuteButtons());
	}

    IEnumerator MuteButtons()
	{
        ChangeInteractable();
        yield return StartCoroutine(cardOpener.WaitUntilAllWillBeReady());
        ChangeInteractable();
    }
    public void StopWorking()
	{
        StartCoroutine(downloader.ResetAll());
        ChangeInteractable();
    }

    void ChangeInteractable()
	{
        if (dropdown.interactable == false) dropdown.interactable = true;
        else dropdown.interactable = false;

        if (startButton.interactable == false) startButton.interactable = true;
        else startButton.interactable = false;

        if (cancelButton.interactable == false) cancelButton.interactable = true;
        else cancelButton.interactable = false;
    }

    


}
