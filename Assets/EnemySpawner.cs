using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject mLeftGroundSpawner;
	public GameObject mRightGroundSpawner;
	public GameObject mLeftAirSpawner;
	public GameObject mRightAirSpawner;

	private float mNextGroundSpawn;
	private float mNextAirSpawn;
	private float mGroundCounter;
	private float mAirCounter;

	public GameObject mCaterpillarPrefab;
	public GameObject mSnailPrefab;
	public GameObject mAphidPrefab;
	public GameObject mWhiteFlyPrefab;
	public GameObject mEnemyContainer;

	private int mEnemyCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
		mNextGroundSpawn = Random.Range(5, 10);
		mNextAirSpawn = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
		if (GameManager.instance.mBetweenLevels)
			return;

		mGroundCounter += Time.deltaTime;
		mAirCounter += Time.deltaTime;

		if (mGroundCounter >= mNextGroundSpawn)
		{
			int randomSpawn = Random.Range(0, 2);
			Vector2 position = (randomSpawn == 0 ? mLeftGroundSpawner.transform.position : mRightGroundSpawner.transform.position);
			GameObject enemy = Instantiate(GetRandomGroundEnemy(), position, Quaternion.identity, mEnemyContainer.transform);
			//enemy.GetComponent<Enemy>().SetRandomGroundEnemy();
			enemy.GetComponent<Enemy>().mId = mEnemyCounter;
			mNextGroundSpawn = Random.Range((3 / GameManager.instance.mCurrentDayStats.mGroundSpawnRate), (8 / GameManager.instance.mCurrentDayStats.mGroundSpawnRate));
			mGroundCounter = 0;
			mEnemyCounter++;
		}

		if (mAirCounter >= mNextAirSpawn)
		{
			int randomSpawn = Random.Range(0, 2);
			Vector2 position = (randomSpawn == 0 ? mLeftAirSpawner.transform.position : mRightAirSpawner.transform.position);
			GameObject enemy = Instantiate(GetRandomAirEnemy(), mLeftAirSpawner.transform.position, Quaternion.identity, mEnemyContainer.transform);
			//enemy.GetComponent<Enemy>().SetRandomAirEnemy();
			enemy.GetComponent<Enemy>().mId = mEnemyCounter;
			mNextAirSpawn = Random.Range((3 / GameManager.instance.mCurrentDayStats.mAirSpawnRate), (8 / GameManager.instance.mCurrentDayStats.mAirSpawnRate));
			mAirCounter = 0;
			mEnemyCounter++;
		}
    }

	GameObject GetRandomGroundEnemy()
	{
		int randomEnemy = Random.Range(0, (int)(Enemy.GroundEnemyType.BUG_MAX));

		if (randomEnemy == (int)Enemy.GroundEnemyType.CATERPILLAR)
		{
			return mCaterpillarPrefab;
		}
		else if (randomEnemy == (int)Enemy.GroundEnemyType.SNAIL)
		{
			return mSnailPrefab;
		}

		return null;
	}

	GameObject GetRandomAirEnemy()
	{
		int randomEnemy = Random.Range(0, (int)(Enemy.AirEnemyType.BUG_MAX));

		if (randomEnemy == (int)Enemy.AirEnemyType.APHID)
		{
			return mAphidPrefab;
		}
		else if (randomEnemy == (int)Enemy.AirEnemyType.WHITE_FLY)
		{
			return mWhiteFlyPrefab;
		}

		return null;
	}
}
