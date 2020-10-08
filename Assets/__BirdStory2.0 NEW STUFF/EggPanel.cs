using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPanel : MonoBehaviour {
	public ClickOnEggs clickOnEggs;
	public inputDetector myInputDetector;
	public SceneTapEnabler sceneTapEnabScript;

	[Header("Egg Panel")]
	public GameObject eggPanel;
	public GameObject eggPanelHidden;
	public GameObject eggPanelShown;
	public float panelMoveSpeed;
	public float basePanelOpenTime;
	public GameObject dropDrowArrow;
	private float openTimer;
	public bool lockDropDownPanel;
	public bool openEggPanel;

	void Update() {
		if (myInputDetector.Tapped) {
			mousePos = Camera.main.ScreenToWorldPoint(myInputDetector.TapPosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit) {
				if (sceneTapEnabScript.canTapEggRidPanPuz) { // On regular eggs, puzzle, eggPanel
					if (hit.collider.CompareTag("EggPanel")) {
						if (lockDropDownPanel) {
							openEggPanel = false;
							lockDropDownPanel = false;
							//SFX Play close panel sound
							audioSceneGenScript.closePanel();
							return;
						}
						if (clickOnEggs.eggMoving <= 0)	{
							openEggPanel = true;
							lockDropDownPanel = true;
							//SFX Play close panel sound
							audioSceneGenScript.openPanel();
						}
						if (clickOnEggs.eggMoving > 0) {
							lockDropDownPanel = true;
						}
					}
				}
			}
		}

		// -- Egg Panel Movement -- //
		if (clickOnEggs.eggMoving <= 0 && !lockDropDownPanel) {
			// - Hide Egg Panel - //
			if (timer <= basePanelOpenTime && openEggPanel)
			{
				timer += Time.deltaTime;
			}
			else { openEggPanel = false; timer = 0f; }

			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelHidden.transform.position, Time.deltaTime * panelMoveSpeed);
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y , 180);
		}
		if (clickOnEggs.eggMoving > 0 || openEggPanel) {
			// - Show Egg Panel - //
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelShown.transform.position, Time.deltaTime * panelMoveSpeed);
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y , 0);
		}
	}

	IEnumerator EggPanelMovement (Vector3 panelDestination) {
		float panelTimer = 0f;
		Vector3 currentPosition = eggPanel.transform.position;
		while (panelTimer < 1f) {
			// panelMoveSpeed += Time.deltaTime/basePanelOpenTime;
			// eggPanel.transform.postion = Vector3.Lerp(currentPosition, panelDestination, panelTimer);
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, panelDestination, Time.deltaTime*panelMoveSpeed);
		}
	}
}
