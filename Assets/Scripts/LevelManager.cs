using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public GameObject mSunPivot;
	public GameObject mSun;
	private Image mBackgroundImage;
	public GameObject mSafeArea;
	public Gradient mBackgroundColor;
	public GameObject mWeatherCreator;
	public GameObject mRainPrefab;
	public GameObject mSnowPrefab;

	float mGameTime = 0;
	float mEvaluationTime = 0;
	float mTimePerLevel = 30;

	float rainCounter = 0;
	float rainSpawn = 0;
	float snowSpawn = 0;

	public class Day
	{
		public float mFoodMultiplier;
		public float mWaterMultiplier;
		public float mTemperatureMultiplier;
		public float mAirSpawnRate;
		public float mGroundSpawnRate;
		public string mWeatherReport;

		public Day(float foodMult, float waterMult, float tempMult, float airRate, float groundRate)
		{
			mFoodMultiplier = foodMult;
			mWaterMultiplier = waterMult;
			mTemperatureMultiplier = tempMult;
			mAirSpawnRate = airRate;
			mGroundSpawnRate = groundRate;
		}
	};

	public class Season
	{
		public Day[] mDays;
		public string mSeasonName;
		public Season(string seasonName)
		{
			mSeasonName = seasonName;
			mDays = new Day[5];
		}
	};

	public Season[] mSeasons = new Season[4];

	private void Awake()
	{
		mBackgroundImage = mSafeArea.GetComponent<Image>();
		CreateSpring();
		CreateSummer();
		CreateFall();
		CreateWinter();
	}

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		if (GameManager.instance.mBetweenLevels)
			return;

		mSunPivot.transform.eulerAngles = new Vector3(mSunPivot.transform.eulerAngles.x, mSunPivot.transform.eulerAngles.y, (mSunPivot.transform.eulerAngles.z - (8 * Time.deltaTime)));
		mSun.transform.eulerAngles = new Vector3(mSun.transform.eulerAngles.x, mSun.transform.eulerAngles.y, (mSun.transform.eulerAngles.z + (8 * Time.deltaTime)));
		mBackgroundImage.color = mBackgroundColor.Evaluate(mEvaluationTime);
		mEvaluationTime += (Time.deltaTime / mTimePerLevel);
		mGameTime += Time.deltaTime;

		if (rainSpawn > 0)
		{
			if (rainCounter >= rainSpawn)
			{
				Instantiate(mRainPrefab, mWeatherCreator.transform);
			}
			rainCounter += Time.deltaTime;
		}

		if (snowSpawn > 0)
		{
			if (rainCounter >= snowSpawn)
			{
				Instantiate(mSnowPrefab, mWeatherCreator.transform);
			}
			rainCounter += Time.deltaTime;
		}
		
		if (mGameTime >= mTimePerLevel)
			GameManager.instance.mBetweenLevels = true;
	}

	public void OnNewLevel()
	{
		if(mSunPivot)
			mSunPivot.transform.eulerAngles = new Vector3(0, 0, 20);

		if(mBackgroundImage)
			mBackgroundImage.color = mBackgroundColor.Evaluate(0);

		mEvaluationTime = 0;
		mGameTime = 0;

		if (mSeasons[GameManager.instance.mCurrentSeason].mDays[GameManager.instance.mCurrentDay].mWaterMultiplier < 0)
		{
			if (mSeasons[GameManager.instance.mCurrentSeason].mDays[GameManager.instance.mCurrentDay].mWaterMultiplier <= 2)
				rainSpawn = 0.25f;
			else
				rainSpawn = 0.5f;
		}
		else if (mSeasons[GameManager.instance.mCurrentSeason].mDays[GameManager.instance.mCurrentDay].mTemperatureMultiplier == 3)
		{
			snowSpawn = 0.5f;
		}
		else
		{
			rainSpawn = 0;
			snowSpawn = 0;
		}
	}

	void CreateSpring()
	{
		Season spring = new Season("Spring");
		spring.mDays[0] = new Day(1, 1, 1, 1, 1);
		spring.mDays[1] = new Day(1, -1.5f, 1, 1, 1);
		spring.mDays[1].mWeatherReport = "There will be heavy rain tomorrow! Be sure to cover up those flowers!";
		spring.mDays[2] = new Day(1, 1, 1, 3, 3);
		spring.mDays[2].mWeatherReport = "It looks like all sun tomorrow. Animals will be out adventuring!";
		spring.mDays[3] = new Day(1, -1, 1, 1, 1);
		spring.mDays[3].mWeatherReport = "Light rain tomorrow. The rain will give those flowers a drink!";
		spring.mDays[4] = new Day(1, -2, 1, 1, 1);
		spring.mDays[4].mWeatherReport = "Heavy rain on the last day of spring! Stay indoors and cover up those plants!";

		mSeasons[0] = (spring);
	}

	void CreateSummer()
	{
		Season summer = new Season("Summer");
		summer.mDays[0] = new Day(1, 1.25f, 1, 1, 1);
		summer.mDays[0].mWeatherReport = "First day of summer! Nothing urgent to report for the weather tomorrow!";
		summer.mDays[1] = new Day(-1, 1.5f, 1, 1.5f, 1.5f);
		summer.mDays[1].mWeatherReport = "Why is it so hot! Cover up those flowers!";
		summer.mDays[2] = new Day(1, 1, 1, 1.5f, 1.5f);
		summer.mDays[2].mWeatherReport = "It looks like we are heading in to a heat wave. Get prepared for the coming days.";
		summer.mDays[3] = new Day(-2f, 2f, 1, 2, 2);
		summer.mDays[3].mWeatherReport = "Heat wave! Cover up your flowers! Make sure they have plenty of water!";
		summer.mDays[4] = new Day(1.25f, 1, 1, 3, 3);
		summer.mDays[4].mWeatherReport = "Last day of summer, what a bummer. Expect nothing but blue skies!";

		mSeasons[1] = (summer);
	}

	void CreateFall()
	{
		Season fall = new Season("Fall");
		fall.mDays[0] = new Day(1, 1, 1, 2, 2);
		fall.mDays[0].mWeatherReport = "First day of Fall.. Leaves are falling, bugs are out!";
		fall.mDays[1] = new Day(1, 1, 1, 2, 2);
		fall.mDays[1].mWeatherReport = "It looks like we are going to be in a long month for animals peeking around the corner.";
		fall.mDays[2] = new Day(1, 1, 2, 2, 2);
		fall.mDays[2].mWeatherReport = "Brrr... It's starting to get chilly. Don't get sick! Watch your temperature.";
		fall.mDays[3] = new Day(1, 1, 2, 3, 3);
		fall.mDays[3].mWeatherReport = "I hate this... too cold and too many bugs. When is spring?";
		fall.mDays[4] = new Day(1, 1, 2, 5, 5);
		fall.mDays[4].mWeatherReport = "Infestation! Be prepared for hitting (I really hope you've upgraded your swatter)!";

		mSeasons[2] = (fall);
	}

	void CreateWinter()
	{
		Season winter = new Season("Winter");
		winter.mDays[0] = new Day(1.25f, 1, 3, 1.5f, 1.5f);
		winter.mDays[0].mWeatherReport = "First day of Winter! Baby it's cold outside!";
		winter.mDays[1] = new Day(1.5f, 1, 3, 1.5f, 1.5f);
		winter.mDays[1].mWeatherReport = "At least the investation has calmed down a little bit?";
		winter.mDays[2] = new Day(1.5f, 1, 4, 1.5f, 1.5f);
		winter.mDays[2].mWeatherReport = "Prepare for more chilly weather. Check your flowers temperature";
		winter.mDays[3] = new Day(1.5f, 1, 4, 1.5f, 1.5f);
		winter.mDays[3].mWeatherReport = "Hitting freezing temperatures! *Watch your flowers temperature*";
		winter.mDays[4] = new Day(1.5f, 1, 5, 1.5f, 1.5f);
		winter.mDays[4].mWeatherReport = "Last day of winter! Hooray! But... We are in sub-zero temperatures";

		mSeasons[3] = (winter);
	}
}
