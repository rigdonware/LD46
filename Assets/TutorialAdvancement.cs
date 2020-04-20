using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAdvancement : MonoBehaviour
{
	public List<GameObject> mOrderToShow;
	int current = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void AdvanceTutorial()
	{
		if (current == 0)
			mOrderToShow[0].SetActive(true);
		else if (current == 1)
			mOrderToShow[1].SetActive(true);
		else if (current == 2)
		{
			mOrderToShow[0].SetActive(false);
			mOrderToShow[1].SetActive(false);
			mOrderToShow[2].SetActive(true);
			mOrderToShow[3].SetActive(true);
		}
		else if (current == 3)
			mOrderToShow[4].SetActive(true);
		else if (current == 4)
			mOrderToShow[5].SetActive(true);
		else if (current == 5)
		{
			mOrderToShow[2].SetActive(false);
			mOrderToShow[3].SetActive(false);
			mOrderToShow[4].SetActive(false);
			mOrderToShow[5].SetActive(false);
			mOrderToShow[9].SetActive(false);
			mOrderToShow[6].SetActive(true);
			mOrderToShow[7].SetActive(true);
		}
		else if (current == 6)
		{
			mOrderToShow[7].SetActive(false);
			mOrderToShow[8].SetActive(true);

			transform.Find("TutorialNext").Find("Text").GetComponent<Text>().text = "Play";
		}
		else if (current == 7)
		{
			this.gameObject.SetActive(false);
			GameManager.instance.NextLevel(); //start the game
		}

		current++;
	}
}
