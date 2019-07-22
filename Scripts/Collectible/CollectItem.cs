using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectItem : MonoBehaviour
{
	[SerializeField] InGameController inGameController;

	private bool isCollected = false;
	public LayerMask playerLayer;
	public TextMeshProUGUI cherriesCollectedScreenText;

	void Update()
	{
		Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer);
		if (hit)
		{
			Collect();
		}
	}
    public void Collect()
    {
		//GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("wasCollected", true);
        Destroy(gameObject, 0.375f);
		if(this.tag == "Cherry" && !isCollected)
		{
			++inGameController.cherriesCollected;
			cherriesCollectedScreenText.text = "x" + inGameController.cherriesCollected.ToString();
		}
		else if(!isCollected)
		{
			inGameController.LevelFinished();
		}
		isCollected = true;

    }

}
