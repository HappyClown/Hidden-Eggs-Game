using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteSaveButton : MonoBehaviour 
{
	private Button button;


	void Start () 
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(DeleteSaveFile);
	}


	public void DeleteSaveFile ()
	{
		GlobalVariables.globVarScript.DeleteAllData();
	}

}
