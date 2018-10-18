using UnityEngine;

public class FollowObjPos : MonoBehaviour 
{
	public GameObject objToFollow;



	void Update () 
	{
		this.transform.position = objToFollow.transform.position;
	}
}
