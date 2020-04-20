using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
	public Slider mHealthBar;
	public Slider mFoodBar;
	public Slider mWaterBar;
	public Slider mTemperatureBar;

	Text mHealthBarText;
	Text mFoodBarText;
	Text mWaterBarText;
	Text mTemperatureBarText;

	public float mFoodMultiplier;
	public float mWaterMultiplier;
	public float mTemperatureMultiplier;

	float mWaterValue = 100;
	float mFoodValue = 100;

	float mStartX;
	float mEndX;

    // Start is called before the first frame update
    void Start()
    {
		mHealthBarText = mHealthBar.transform.Find("Text").GetComponent<Text>();
		mFoodBarText = mFoodBar.transform.Find("Text").GetComponent<Text>();
		mWaterBarText = mWaterBar.transform.Find("Text").GetComponent<Text>();
		mTemperatureBarText = mTemperatureBar.transform.Find("Text").GetComponent<Text>();

		mHealthBarText.text = "100/100";
		mFoodBarText.text = "100/100";
		mWaterBarText.text = "100/100";
		mTemperatureBarText.text = "100%";

		mStartX = this.GetComponent<RectTransform>().rect.x;
		mEndX = (mStartX + this.GetComponent<RectTransform>().rect.width);

		Debug.Log("Start x: " + mStartX);
		Debug.Log("End x: " + mEndX);
	}

    // Update is called once per frame
    void Update()
    {
		if (GameManager.instance.mInShop)
			return;

		float foodMultiplier = mFoodMultiplier;
		float waterMultiplier = mWaterMultiplier;
		float tempMultiplier = mTemperatureMultiplier;

		if (!GameManager.instance.mUsingSwatter)
		{
			Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			Debug.Log("Mouse pos x: " + mousePos.x + " pos y: " + mousePos.y);
			if (mousePos.x > 0.45f && mousePos.x < 0.55f && Input.mousePosition.y >= transform.position.y)
			{
				foodMultiplier = 1;
				waterMultiplier = 1;
				tempMultiplier = 1;
			}
		}

		//mHealthBar.value -= Time.deltaTime;
		if (mFoodBar.value > 0)
			UpdateFoodValue(-(Time.deltaTime * foodMultiplier));
		if(mFoodBar.value <= 0 || mFoodValue > 100)
			UpdateHealthValue(-(Time.deltaTime));

		if (mWaterBar.value > 0)
			UpdateWaterValue(-(Time.deltaTime * waterMultiplier));
		if (mWaterBar.value <= 0 || mWaterValue > 100)
			UpdateHealthValue(-(Time.deltaTime));

		if (tempMultiplier > 1)
		{
			UpdateTemperatureValue(-(Time.deltaTime * tempMultiplier));
			if (mTemperatureBar.value <= 50)
				UpdateHealthValue(-(Time.deltaTime));
		}
		else if (mTemperatureBar.value < 100)
			UpdateTemperatureValue(Time.deltaTime);


		if (mHealthBar.value <= 0)
			GameManager.instance.GameOver(false);
	}

	public void Restart()
	{
		mHealthBar.value = 100;
		mFoodBar.value = 100;
		mFoodValue = 100;
		mTemperatureBar.value = 100;
		mWaterBar.value = 100;
		mWaterValue = 100;

		mHealthBarText.text = "100/100";
		mWaterBarText.text = "100/100";
		mFoodBarText.text = "100/100";
		mTemperatureBarText.text = "100%";
	}

	public void LoadLevelSettings(float foodMult, float waterMult, float tempMult)
	{
		mFoodMultiplier = foodMult;
		mWaterMultiplier = waterMult;
		mTemperatureMultiplier = tempMult;
		mTemperatureBar.value = 100;
	}

	public void OnFoodButtonClicked()
	{
		UpdateFoodValue(10);
	}

	public void OnWaterButtonClicked()
	{
		UpdateWaterValue(10);
	}

	public void TakeDamage(int damageValue)
	{
		UpdateHealthValue(-damageValue);
	}

	void UpdateFoodValue(float value)
	{
		if(mFoodValue <= 100)
			mFoodBar.value += value;
		mFoodValue += value;
		int newValue = (int)mFoodValue;
		mFoodBarText.text = (newValue.ToString() + "/100");
	}

	void UpdateWaterValue(float value)
	{
		if(mWaterValue <= 100)
			mWaterBar.value += value;
		mWaterValue += value;
		int newValue = (int)mWaterValue;
		mWaterBarText.text = (newValue.ToString() + "/100");
	}

	void UpdateHealthValue(float value)
	{
		mHealthBar.value += value;
		int newValue = (int)mHealthBar.value;
		mHealthBarText.text = (newValue.ToString() + "/100");
	}

	void UpdateTemperatureValue(float value)
	{
		mTemperatureBar.value += value;
		int newValue = (int)mTemperatureBar.value;
		mTemperatureBarText.text = (newValue.ToString() + "/100");
	}
}
