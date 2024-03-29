﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    public Animator ani;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ani.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ani.SetBool("Selected", false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        ani.SetBool("Selected", true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ani.SetBool("Selected", false);
    }
}