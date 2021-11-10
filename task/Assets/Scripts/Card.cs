using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public GameObject thisCard;
    public Image cardBack;
    public Image cardFront;
    public Image cardPicture;
    public Image carMusk;
    public bool isOpend = false;
    Sequence FlipOnFrontAnim;
    Sequence FlipOnBackAnim;
    float timeOfAnimation = 0.3f;
    public UnityEvent FinishAnimation;
	// Start is called before the first frame update
	private void Awake()
	{
        DOTween.Init();
	}
	void Start()
    {
        TurnOffFront();
    }

    public void FlipToFront()
    {
		if (!isOpend)
		{
            FlipOnFrontAnim = DOTween.Sequence();
            FlipOnFrontAnim.Append(transform.DORotate(new Vector3(0, transform.rotation.y + 90, 0), timeOfAnimation, RotateMode.Fast))
                .AppendCallback(() => {
                    cardBack.enabled = false;
                    cardFront.transform.SetAsLastSibling();
                    TurnOnFront();
                });
            FlipOnFrontAnim.Join(transform.DORotate(new Vector3(0, transform.rotation.y + 180, 0), timeOfAnimation, RotateMode.Fast)).
                AppendCallback(() => 
                {
                    isOpend = true;
                    FinishAnimation.Invoke();
                });
        }
        
    }

    public void FlipToBack()
	{
		if (isOpend)
		{
            isOpend = false;
            FlipOnBackAnim = DOTween.Sequence();
            FlipOnBackAnim.Append(transform.DORotate(new Vector3(0, transform.rotation.y - 90, 0), timeOfAnimation, RotateMode.Fast))
                .AppendCallback(() => {
                    cardBack.enabled = true;
                    cardFront.transform.SetAsLastSibling();
                    TurnOffFront();
                });
            FlipOnBackAnim.Join(transform.DORotate(new Vector3(0, transform.rotation.y - 180, 0), timeOfAnimation, RotateMode.Fast)).
                AppendCallback(() =>
                {
                    FinishAnimation.Invoke();
                });
		}
		else
		{
            FinishAnimation.Invoke();
        }
    }

    void TurnOffFront()
	{
        cardFront.enabled = false;
        cardPicture.enabled = false;
        carMusk.enabled = false;
    }
    void TurnOnFront()
	{
        cardFront.enabled = true;
        cardPicture.enabled = true;
        carMusk.enabled = true;
    }
}
