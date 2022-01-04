using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveAfterTime : MonoBehaviour
{
    //float timeBeforeInactive = 5f;
    WaitForSeconds waitFiveSeconds = new WaitForSeconds(5f);

    void OnEnable() {
        StartCoroutine(InactiveAfterXSeconds());
    }

    IEnumerator InactiveAfterXSeconds() {
        yield return waitFiveSeconds;
        this.gameObject.SetActive(false);
    }
}
