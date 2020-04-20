using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwatterHandler : MonoBehaviour, IPointerClickHandler
{
	GameObject mTargetedEnemy;
	GameObject mShopButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		if (mTargetedEnemy)
		{
			Debug.Log("Killing enemy");
			Destroy(mTargetedEnemy);
			mTargetedEnemy = null;
		}

		if (mShopButton)
		{
			Debug.Log("Pressed shop button");
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("Trigger");
		if (other.gameObject.tag == "Enemy")
		{
			Debug.Log("Found enemy");
			mTargetedEnemy = other.gameObject;
		}
		if (other.gameObject.tag == "ShopButton")
		{
			Debug.Log("Found shop button");
			mShopButton = other.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		mTargetedEnemy = null;
		mShopButton = null;
	}
}
