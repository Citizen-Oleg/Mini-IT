using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.SimpleEventBus;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventStreams.UserInterface.Publish(new EventAutoMerge());
    }
}
