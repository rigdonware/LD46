using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Swatter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	//GameObject mTargetedEnemy;
	Dictionary<int, GameObject> mTargetedEnemies;
	GameObject mShopButton;
	GameObject mPlayButton;
	GameObject mQuitButton;
	GameObject mNextButton; //tutorial

	public GameObject mTutorial;
	public GameObject mStartScreen;

	public Sprite mSwatterSprite;
	public Sprite mUmbrellaSprite;

	const int WATER_BUTTON = 8;
	const int FOOD_BUTTON = 9;
	const int SWATTER_BUTTON = 10;
	const int SHELTER_BUTTON = 11;
	const int NEXT_DAY = 12;

	int mWaterCost = 20;
	int mFoodCost = 20;
	int mSwatterUpgradeCost = 100;
	int mUmbrellaCost = 200;

	const int MAX_SWATTER_UPGRADES = 3;
	int mCurrentSwatterUpgrade = 0;

	float mAttackSpeed = 1;
	float mAttackCounter = 2;

	float mChangingCounter = 1;
	float mChangingSpeed = 1;
	bool mUmbrellaUnlocked = false;

	void Start()
    {
		Cursor.visible = false;
		mTargetedEnemies = new Dictionary<int, GameObject>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		mAttackCounter += Time.deltaTime;
		mChangingCounter += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Alpha1) && !GameManager.instance.mUsingSwatter)
		{
			if (mChangingCounter >= mChangingSpeed)
			{
				ChangeToSwatter();
				mChangingCounter = 0;
			}
		}
		else if (mUmbrellaUnlocked && Input.GetKeyDown(KeyCode.Alpha2) && GameManager.instance.mUsingSwatter)
		{
			if (mChangingCounter >= mChangingSpeed)
			{
				GameManager.instance.mUsingSwatter = false;
				GetComponent<Image>().sprite = mUmbrellaSprite;
				transform.localScale = new Vector3(3, 3, 3);
				mChangingCounter = 0;
			}
		}
	}

	public void ChangeToSwatter()
	{
		GameManager.instance.mUsingSwatter = true;
		GetComponent<Image>().sprite = mSwatterSprite;
		transform.localScale = new Vector3(1, 1, 1);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!GameManager.instance.mUsingSwatter)
			return;

		if (!GameManager.instance.mInShop && GameManager.instance.mStartedGame)
		{
			if (mAttackCounter >= mAttackSpeed)
			{
				transform.eulerAngles = new Vector3(20, 20, 0);

				if (mTargetedEnemies.Count > 0)
				{
					foreach (KeyValuePair<int, GameObject> pair in mTargetedEnemies)
					{
						Debug.Log("Killing enemy");
						Destroy(pair.Value);
						mTargetedEnemies.Remove(pair.Key);
						break;
						//mTargetedEnemy = null;
					}
				}
				mAttackCounter = 0;
			}

			return;
		}

		transform.eulerAngles = new Vector3(20, 20, 0);

		if (mShopButton)
		{
			if (mShopButton.layer == WATER_BUTTON && GameManager.instance.mTotalMoney >= mWaterCost)
			{
				GameManager.instance.mTotalMoney -= mWaterCost;
				GameManager.instance.mFlowerScript.OnWaterButtonClicked();
				GameManager.instance.mShop.GetComponent<ShopManager>().UpdateText();
			}
			else if (mShopButton.layer == FOOD_BUTTON && GameManager.instance.mTotalMoney >= mFoodCost)
			{
				GameManager.instance.mTotalMoney -= mFoodCost;
				GameManager.instance.mFlowerScript.OnFoodButtonClicked();
				GameManager.instance.mShop.GetComponent<ShopManager>().UpdateText();
			}
			else if (mCurrentSwatterUpgrade < MAX_SWATTER_UPGRADES && mShopButton.layer == SWATTER_BUTTON && GameManager.instance.mTotalMoney >= mSwatterUpgradeCost)
			{
				mAttackSpeed /= 2;
				GameManager.instance.mTotalMoney -= mSwatterUpgradeCost;
				mSwatterUpgradeCost *= 2;
				GameManager.instance.mShop.GetComponent<ShopManager>().mSwatterPrice = mSwatterUpgradeCost;
				GameManager.instance.mShop.GetComponent<ShopManager>().UpdateText();
				//GameManager.instance.mFlowerScript.OnSwatterButtonClicked();
			}
			else if (!mUmbrellaUnlocked && mShopButton.layer == SHELTER_BUTTON && GameManager.instance.mTotalMoney >= mUmbrellaCost)
			{
				GameManager.instance.mTotalMoney -= mUmbrellaCost;
				GameManager.instance.mShop.GetComponent<ShopManager>().UpdateText();
				mUmbrellaUnlocked = true;
			}
			else if (mShopButton.layer == NEXT_DAY)
				GameManager.instance.NextLevel();

			Debug.Log("Pressed shop button");
		}

		if (mPlayButton)
		{
			mStartScreen.SetActive(false);
			if (GameManager.instance.mStartedGame) //replay
				GameManager.instance.Restart();
			else
			{
				mTutorial.SetActive(true);
				GameManager.instance.AdvanceTutorial();
			}
		}

		if (mNextButton)
			GameManager.instance.AdvanceTutorial();

		if (mQuitButton)
			Application.Quit();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		transform.eulerAngles = new Vector3(0, 0, 0);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Debug.Log("Found enemy");
			int id = other.gameObject.GetComponent<Enemy>().mId;
			mTargetedEnemies.Add(id, other.gameObject);
		}

		if (other.gameObject.tag == "ShopButton")
		{
			Debug.Log("Found shop button");
			mShopButton = other.gameObject;
		}

		if (other.gameObject.tag == "PlayButton")
			mPlayButton = other.gameObject;

		if (other.gameObject.tag == "QuitButton")
			mQuitButton = other.gameObject;

		if (other.gameObject.tag == "NextButton")
			mNextButton = other.gameObject;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			int id = collision.gameObject.GetComponent<Enemy>().mId;
			if(mTargetedEnemies.ContainsKey(id))
				mTargetedEnemies.Remove(id);
		}

		if(collision.gameObject.tag == "ShopButton")
			mShopButton = null;

		if (collision.gameObject.tag == "PlayButton")
			mPlayButton = null;

		if (collision.gameObject.tag == "QuitButton")
			mQuitButton = null;

		if (collision.gameObject.tag == "NextButton")
			mNextButton = null;
	}
}
