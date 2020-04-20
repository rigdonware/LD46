using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public GameObject mMoneyTextObject;
	public GameObject mNextDayWeatherReport;
	public GameObject mSwatterUpgradePrice;

	public int mSwatterPrice = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void UpdateText()
	{
		string tempString = "$" + GameManager.instance.mTotalMoney.ToString();
		mMoneyTextObject.GetComponent<Text>().text = (tempString);
		mSwatterUpgradePrice.GetComponent<Text>().text = ("$" + mSwatterPrice.ToString());

		int currentDay = GameManager.instance.mCurrentDay;
		int currentSeason = GameManager.instance.mCurrentSeason;
		//int currentDay = previousDay + 1;
		//if (previousDay >= 5)
		//{
		//	currentSeason++;
		//	currentDay = 0;
		//}

		LevelManager.Day day = GameManager.instance.mLevelManager.mSeasons[currentSeason].mDays[currentDay];

		string weatherReport = day.mWeatherReport;
		mNextDayWeatherReport.GetComponent<Text>().text = "Tomorrows Forecast:\n" + weatherReport;
	}
}
