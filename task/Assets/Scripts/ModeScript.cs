using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeScript : MonoBehaviour
{
    public Dropdown dropdown;
    public Downloader downloader;
	private void Start()
	{
        downloader.mode = ModeOfWork.AllByOne;
    }
	public void ModeChanged()
	{
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
}
