using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour 
{

	public Renderer rend;
	public Material dissShader;
	public float dissAmount;
	public float disTime;
	public bool disOnOff;
	public GameObject GO;
	public Collider goCollider;
	void Start () 
	{
		rend = GetComponent<Renderer>();
		dissShader = rend.material;
		GO = this.gameObject;
		goCollider = GO.GetComponent<Collider>();

		disTime = Random.Range(1.0f, 3.0f);
	}

	void Update () 
	{
		goCollider.enabled = true;

		dissShader.SetFloat ("_DissolveAmount", dissAmount);

		disTime = disTime + 0.6f * Time.deltaTime;

		if (disTime >= 2.5f && disTime <= 5.0f)
		{
			disOnOff = true;
		}
		if (disTime >= 5.0f)
		{
			disOnOff = false;
			disTime = 0f;
		}

		if (disOnOff == true)
		{
			dissAmount =  Mathf.Clamp((dissAmount + 0.2f * Time.deltaTime), 0 ,0.6f);
		}
		if (disOnOff == false)
		{
			dissAmount =  Mathf.Clamp((dissAmount - 0.2f * Time.deltaTime), 0 ,0.6f);
		}
		if (dissAmount >= 0.59f)
		{
			goCollider.enabled = false;
		}
		if (dissAmount <= 0.05f)
		{
			goCollider.enabled = true;
		}
	}
}
