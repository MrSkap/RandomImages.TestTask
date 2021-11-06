using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CardOpener : MonoBehaviour
{
    public GameObject[] cards;
    public Image[] images;
    float imageWidth;
    float imageHeight;
    public bool[] ReadyCard = new bool[5];
    // Start is called before the first frame update
    void Start()
    {
        imageHeight = images[0].sprite.rect.height;
        imageWidth = images[0].sprite.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCard(int numberOfCard)
	{
        AssetDatabase.Refresh();
        images[numberOfCard].sprite = Resources.Load<Sprite>("Images/" + numberOfCard.ToString());
        Debug.Log("Card " + numberOfCard.ToString() + " is ready!");
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
            if (counter == 5) ready = true;
            yield return new WaitForFixedUpdate();
		}
        AssetDatabase.Refresh();
        for(int i = 0; i < images.Length; i++)
		{
            images[i].sprite = Resources.Load<Sprite>("Images/" + i.ToString());
        }
        Debug.Log("All are ready!");
    }
}
