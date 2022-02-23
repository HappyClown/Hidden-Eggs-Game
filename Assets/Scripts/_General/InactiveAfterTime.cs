using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveAfterTime : MonoBehaviour
{
    //float timeBeforeInactive = 5f;
    public bool waitforFiveSeconds = true;
    public bool waitForEightSeconds;
    WaitForSeconds waitFive = new WaitForSeconds(5f);
    WaitForSeconds waitEight = new WaitForSeconds(8f);

    void OnEnable() {
        StartCoroutine(InactiveAfterXSeconds());
    }

    IEnumerator InactiveAfterXSeconds() {
        if (waitforFiveSeconds) {
            yield return waitFive;
        }
        else if (waitForEightSeconds) {
            yield return waitEight;
        }
        this.gameObject.SetActive(false);
    }
}
