using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
	public float mLifetime;
	public float counter;
    // Start is called before the first frame update
    void Start()
    {
		this.transform.position = new Vector2(Random.Range(0, 1200), this.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
		if (counter >= mLifetime)
			Destroy(this.gameObject);

		this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - (150 * Time.deltaTime));

		counter += Time.deltaTime;
    }
}
