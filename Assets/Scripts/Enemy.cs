using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int mSpeed = 200;
	public int mAttackDamage = 1;
	private float mAttackDistance;
	public GameObject mFlower;
	private bool mSpawnedLeft = false;
	public float mAttackSpeed = 2f;
	private float mAttackCounter = 2f;
	private bool mArrivedAtDestination = false;
	public bool mAirUnit = false;
	public int mId;

	public enum GroundEnemyType
	{
		NONE = -1,
		CATERPILLAR = 0,	
		SNAIL = 1,
		BUG_MAX = 2,
	};

	public enum AirEnemyType
	{
		NONE = -1,
		APHID = 0,
		WHITE_FLY = 1,
		BUG_MAX = 2
	};

	public GroundEnemyType mGroundType;
	public AirEnemyType mAirType;

	// Start is called before the first frame update
	void Start()
    {
		mAttackCounter = mAttackSpeed;
		//mFlower = GameObject.FindGameObjectWithTag("Flower");
		//mAttackDistance = (mFlower.transform.GetComponent<RectTransform>().rect.width / 2);
		//mSpawnedLeft = (transform.position.x < mFlower.transform.position.x);

		//mFlower = GameObject.FindGameObjectWithTag("Flower");
		mAttackDistance = (GameManager.instance.mFlower.transform.GetComponent<RectTransform>().rect.width / 2);
		mSpawnedLeft = (transform.position.x < GameManager.instance.mFlower.transform.position.x);

		if (!mSpawnedLeft)
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
	}

    // Update is called once per frame
    void Update()
    {
		//float distanceFromFlower = (transform.position.x - mFlower.transform.position.x);
		if (!mArrivedAtDestination)
		{
			if (mSpawnedLeft)
			{
				if (mAirUnit)
				{
					bool goRight = true;// ((Random.Range(0, 100) > 25) && transform.position.x < mFlower.transform.position.x);
					bool goDown = true;// ((Random.Range(0, 100) > 25) && transform.position.y > mFlower.transform.position.y);
					transform.position = new Vector2(transform.position.x + (goRight ? (mSpeed * Time.deltaTime) : (-mSpeed * Time.deltaTime)), transform.position.y + (goDown ? ((-mSpeed / 2) * Time.deltaTime) : ((mSpeed / 2) * Time.deltaTime)));
				}
				else
					transform.position = new Vector2(transform.position.x + (mSpeed * Time.deltaTime), transform.position.y);
			}
			else
			{
				if (mAirUnit)
				{
					bool goLeft = true;// ((Random.Range(0, 100) > 25) && transform.position.x > mFlower.transform.position.x);
					bool goDown = true;// ((Random.Range(0, 100) > 25) && transform.position.y > mFlower.transform.position.y);
					transform.position = new Vector2(transform.position.x + (goLeft ? (mSpeed * Time.deltaTime) : (-mSpeed * Time.deltaTime)), transform.position.y + (goDown ? ((-mSpeed / 2) * Time.deltaTime) : ((mSpeed / 2) * Time.deltaTime)));
				}
				else
					transform.position = new Vector2(transform.position.x - (mSpeed * Time.deltaTime), transform.position.y);
			}
		}
		else
		{
			mAttackCounter += Time.deltaTime;
			if (mAttackCounter >= mAttackSpeed)
			{
				//mFlower.GetComponent<Flower>().TakeDamage(mAttackDamage);
				GameManager.instance.mFlower.GetComponent<Flower>().TakeDamage(mAttackDamage);
				mAttackCounter = 0;
			}
		}

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Flower")
			mArrivedAtDestination = true;
	}
}
