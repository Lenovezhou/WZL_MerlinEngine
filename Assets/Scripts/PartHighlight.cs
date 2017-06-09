using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    private Material highlightMat;

    [SerializeField]
    private Material holoMat;

    [SerializeField]
    private AutoLabel labeler;

    private MeshRenderer meshRenderer;
    private bool holoMode = false;
    private Material originalMat;
    private Coroutine materialTransformCoroutine;

    private EngineController controller;

    private IList<IList<GameObject>> allParts;

    private void Start()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.originalMat = new Material(this.meshRenderer.material);
        this.controller = this.transform.parent.parent.GetComponent<EngineController>();
        Debug.Assert(controller != null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!holoMode)
        {
            controller.PlayHighlightAudio();
            TransformIntoMaterial(this.highlightMat);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!holoMode)
        {
            TransformIntoMaterial(this.originalMat);
        }
    }

    private void TransformIntoMaterial(Material targetMat)
    {
        if (this.materialTransformCoroutine != null)
        {
            StopCoroutine(this.materialTransformCoroutine);
        }
        this.materialTransformCoroutine = StartCoroutine(TransformMaterialCoroutine(targetMat));
    }

    private IEnumerator TransformMaterialCoroutine(Material targetMat)
    {
        var material = this.GetComponent<MeshRenderer>().material;
        var startTime = Time.time;
        float deltaTime = 0;
        while (deltaTime < 1.1f)
        {
            material.Lerp(material, targetMat, deltaTime);
            yield return new WaitForEndOfFrame();
            deltaTime += Time.deltaTime * 3.0f;     // Transition speed
        }
        this.GetComponent<MeshRenderer>().material = targetMat;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var labeler = Instantiate(this.labeler, this.transform.parent);
        var textInfo = GetComponent<PartLabel>();
        labeler.SetTextInfo(textInfo.Name, textInfo.Id, textInfo.Description, "Valid inventory item");
        var centerPoint = this.GetComponent<Renderer>().bounds.center;
        labeler.transform.position = centerPoint;

        var dir = centerPoint - (this.transform.position + 1f * Vector3.down);
        var middlePoint = RayCastToPlaceHolder(centerPoint, dir);
        //var middlePoint = centerPoint + dir.normalized * 0.1f;
        var end = middlePoint + Vector3.up * 0.1f;

        this.controller.PlayLabelingAudio();
        labeler.Label(centerPoint, middlePoint, end);
    }

    private Vector3 RayCastToPlaceHolder(Vector3 centerPoint, Vector3 dir)
    {
        RaycastHit raycastHit;
        var ray = new Ray(centerPoint, dir);
        if (Physics.Raycast(ray, out raycastHit, 10.0f, 0x100))
        {
            return raycastHit.point;
        }
        Debug.Log("Missed raycast");
        return centerPoint + dir.normalized * 0.2f;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("HoloDisplay"))
        {
            this.holoMode = true;
            this.meshRenderer.material = this.holoMat;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("HoloDisplay"))
        {
            this.holoMode = false;
            this.meshRenderer.material = this.originalMat;
        }
    }
}
