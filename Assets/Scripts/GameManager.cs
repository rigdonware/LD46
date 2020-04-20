using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public LevelManager mLevelManager;
	public GameObject mFlower;
	public GameObject mShop;
	public GameObject mTutorial;
	public GameObject mGameOverScreen;
	public Flower mFlowerScript;
	public int mTotalMoney = 200;
	public int mCurrentSeason = 0;
	public int mCurrentDay = 0;
	public int mCurrentTutorialScreen = 0;
	public bool mInShop = false;
	public bool mBetweenLevels = false;
	public bool mStartedGame = false;
	public bool mUsingSwatter = true; //else using umbrella
	public LevelManager.Day mCurrentDayStats;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
    {
		mLevelManager = GetComponent<LevelManager>();
		mBetweenLevels = true;
		mFlowerScript = mFlower.GetComponent<Flower>();
		mShop.SetActive(false);
		mTutorial.SetActive(false);
		mCurrentSeason = 0;
		mCurrentDay = 0;
		mTotalMoney = 200;
	}

    // Update is called once per frame
    void Update()
    {
		if (mStartedGame && !mInShop && mBetweenLevels && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
		{
			if (mCurrentSeason == 3 && mCurrentDay == 4)
				GameOver(true);

			mInShop = true;
			LoadShop();
		}
	}

	public void LoadShop()
	{
		mTotalMoney += 50;
		mShop.GetComponent<ShopManager>().UpdateText();
		mShop.SetActive(true);
		if (!mUsingSwatter)
			GameObject.FindGameObjectWithTag("Swatter").GetComponent<Swatter>().ChangeToSwatter();
	}

	public void AdvanceTutorial()
	{
		GameObject tutorialScreen = GameObject.FindGameObjectWithTag("Tutorial");
		tutorialScreen.GetComponent<TutorialAdvancement>().AdvanceTutorial();
	}

	public void NextLevel()
	{
		Debug.Log("Starting game");
		mStartedGame = true;
		mLevelManager.OnNewLevel();

		if (!mUsingSwatter)
			GameObject.FindGameObjectWithTag("Swatter").GetComponent<Swatter>().ChangeToSwatter();

		mShop.SetActive(false);
		mInShop = false;
		LevelManager.Day currentDay = mLevelManager.mSeasons[mCurrentSeason].mDays[mCurrentDay];
		mCurrentDayStats = currentDay;
		mFlowerScript.LoadLevelSettings(currentDay.mFoodMultiplier, currentDay.mWaterMultiplier, currentDay.mTemperatureMultiplier);
		mCurrentDay++;
		if (mCurrentDay >= 5)
		{
			mCurrentSeason++;
			mCurrentDay = 0;
		}
		mBetweenLevels = false;
	}

	public void Restart()
	{
		Debug.Log("restarting game");

		mFlowerScript.Restart();
		mCurrentDay = 0;
		mCurrentSeason = 0;
		mTotalMoney = 100;
		mGameOverScreen.SetActive(false);
		NextLevel();
	}

	public void GameOver(bool win)
	{
		mGameOverScreen.SetActive(true);
		mShop.SetActive(false);

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			Destroy(enemy);

		string message;
		if (win)
			message = "Congratulations! You survived all four seasons!";
		else
			message = "Game Over! Your flower did not survive.";

		mGameOverScreen.transform.Find("Text").GetComponent<Text>().text = message;
	}
}
