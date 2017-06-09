using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public AudioClip HighlightAudio;
    public AudioClip AppearingAudio;
    public AudioClip RotationAudio;
    public AudioClip HoloAudio;
    public AudioClip ExplosionAudio;
    public AudioClip LabelingAudio;

    private Coroutine coroutine;
    private Coroutine overAllCoroutine;
    private Coroutine rotationCoroutine;
    private bool isOpen = false;
    private bool isHoloView = false;

    private GameObject holoTrigger;
    private Transform engineBody;
    private AudioSource audioSource;

    private void Awake()
    {
        this.holoTrigger = this.transform.Find("HoloTrigger").gameObject;
        Debug.Assert(this.holoTrigger != null);
        this.engineBody = this.transform.FindChild("EngineBody");
        this.engineBody.gameObject.SetActive(false);
        this.audioSource = GetComponent<AudioSource>();

        StartCoroutine(EngineAppear());
    }

    private IEnumerator EngineAppear()
    {
        var allParts = CheckThroughAllParts();
        this.engineBody.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        this.audioSource.PlayOneShot(this.AppearingAudio);
        for (int i = 4; i >= 0; i--)
        {
            foreach (var part in allParts[i])
            {
                part.SetActive(true);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(1.0f);
        this.transform.FindChild("PanelHolder").gameObject.SetActive(true);
    }

    private IList<IList<GameObject>> CheckThroughAllParts()
    {
        var allParts = new List<IList<GameObject>>();
        allParts.Add(new List<GameObject>());
        allParts.Add(new List<GameObject>());
        allParts.Add(new List<GameObject>());
        allParts.Add(new List<GameObject>());
        allParts.Add(new List<GameObject>());

        foreach (Transform partTrans in this.engineBody)
        {
            var partLabel = partTrans.GetComponent<PartLabel>();
            Debug.Assert(partLabel != null);
            Debug.Assert(partLabel.GetLayer() >= 0 && partLabel.GetLayer() < 5);
            allParts[partLabel.GetLayer()].Add(partTrans.gameObject);
            partTrans.gameObject.SetActive(false);
        }
        return allParts;
    }

    public void SetExplosionViewStage(int i)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(SetExplosionStage(i));
    }

    public void FlipExplosionStage()
    {
        if (isOpen)
        {
            if (this.overAllCoroutine != null)
            {
                StopCoroutine(this.overAllCoroutine);
            }
            SetExplosionViewStage(0);
            this.isOpen = false;
        }
        else
        {
            this.overAllCoroutine = StartCoroutine(OpenAllStages());
            this.isOpen = true;
        }
    }

    private IEnumerator OpenAllStages()
    {
        this.SetExplosionViewStage(1);
        yield return new WaitForSeconds(3);

        this.SetExplosionViewStage(2);
        yield return new WaitForSeconds(3);

        this.SetExplosionViewStage(3);
    }

    private IEnumerator SetExplosionStage(int index)
    {
        this.audioSource.PlayOneShot(this.ExplosionAudio);
        var parts = GameObject.FindObjectsOfType<PartLabel>();
        int[] steps = new int[] { 0, Mathf.Max(0, index), Mathf.Max(0, index - 1), Mathf.Max(0, index - 2), Mathf.Max(0, index - 3) };
        for (int i = 0; i < 300; i++)
        {
            foreach (var part in parts)
            {
                part.GetComponent<PartExplosion>().MoveOffCenter(steps[part.GetLayer()]);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void FlipHoloView()
    {
        if (!isHoloView)
        {
            this.holoTrigger.SetActive(true);
            StartCoroutine(MoveHoloTrigger(Vector3.zero, true));
            isHoloView = true;
        }
        else
        {
            StartCoroutine(MoveHoloTrigger(new Vector3(0, 0, -0.5f), false));
            isHoloView = false;
        }
    }

    private IEnumerator MoveHoloTrigger(Vector3 target, bool disable)
    {
        this.audioSource.PlayOneShot(this.HoloAudio);
        while (Vector3.Distance(this.holoTrigger.transform.localPosition, target) > 0.01f)
        {
            this.holoTrigger.transform.localPosition = Vector3.MoveTowards(this.holoTrigger.transform.localPosition, target, 0.004f);
            yield return new WaitForEndOfFrame();
        }
        this.holoTrigger.SetActive(enabled);
    }

    public void RotateTheEngine(bool right)
    {
        if (this.rotationCoroutine != null)
        {
            StopCoroutine(this.rotationCoroutine);
        }
        this.rotationCoroutine = StartCoroutine(RotateEngineBody(right ? 90 : -90));
    }

    private IEnumerator RotateEngineBody(int angle)
    {
        this.audioSource.PlayOneShot(this.RotationAudio);
        Quaternion target = this.engineBody.rotation * Quaternion.Euler(0, angle, 0);
        while (Quaternion.Angle(this.engineBody.rotation, target) > 3)
        {
            //this.engineBody.rotation = Quaternion.RotateTowards(this.engineBody.rotation, target, 10);
            this.engineBody.rotation = Quaternion.Slerp(this.engineBody.rotation, target, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlayHighlightAudio()
    {
        this.audioSource.PlayOneShot(this.HighlightAudio);
    }

    public void PlayLabelingAudio()
    {
        this.audioSource.PlayOneShot(this.LabelingAudio);
    }
}
