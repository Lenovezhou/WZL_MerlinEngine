using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;

public class UnityMain : MonoBehaviour {

    private EngineController controller;
    private GestureRecognizer gestureRecognizer;
    private KeywordRecognizer keywordRecongizer;

    private void Start()
    {
        this.controller = Resources.FindObjectsOfTypeAll<EngineController>()[0];
        this.controller.gameObject.SetActive(false);

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
        gestureRecognizer.StartCapturingGestures();

        var keywords = new Dictionary<string, Action>();
        keywords.Add("Switch View", () => this.controller.FlipHoloView());
        keywords.Add("Switch State", () => this.controller.FlipExplosionStage());
        this.keywordRecongizer = new KeywordRecognizer(keywords.Keys.ToArray());
        this.keywordRecongizer.OnPhraseRecognized += (PhraseRecognizedEventArgs args) => {
            Action action;
            if (keywords.TryGetValue(args.text, out action))
            {
                action();
            }
        };
        this.keywordRecongizer.Start();
    }

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, 
            new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 80, 30), "Layer 0"))
        {
            this.controller.SetExplosionViewStage(0);
        }
        if (GUI.Button(new Rect(100, 10, 80, 30), "Layer 1"))
        {
            this.controller.SetExplosionViewStage(1);
        }
        if (GUI.Button(new Rect(190, 10, 80, 30), "Layer 2"))
        {
            this.controller.SetExplosionViewStage(2);
        }
        if (GUI.Button(new Rect(280, 10, 80, 30), "Layer 3"))
        {
            this.controller.SetExplosionViewStage(3);
        }
    }
}
