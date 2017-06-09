using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{

    private void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        var point = ray.GetPoint(1.5f);

        this.transform.position = Vector3.MoveTowards(this.transform.position, point, .8f * Time.deltaTime);

        var lookAtVector = this.transform.position * 2 - Camera.main.transform.position;
        lookAtVector.y = 0;
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(lookAtVector), 45.0f * Time.deltaTime);
    }

    public void StartButtonClick()
    {
        StartCoroutine(StartScene());
    }

    private IEnumerator StartScene()
    {
        this.GetComponent<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(1f);
        var engine = Resources.FindObjectsOfTypeAll<EngineController>()[0];
        engine.transform.position = this.transform.position;
        var horizontalLine = Camera.main.transform.position;
        horizontalLine.y = this.transform.position.y;
        engine.transform.LookAt(horizontalLine);
        engine.transform.Rotate(Vector3.up, -90);
        engine.gameObject.SetActive(true);
        Destroy(this.gameObject, 0.5f);
    }
}
