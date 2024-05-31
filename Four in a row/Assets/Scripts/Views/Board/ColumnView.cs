using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class ColumnView : BaseView, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private SlotView[] slots;
    public GameObject backGroundRow;

    public Action<ColumnView> onColumnClicked;
    public Action<ColumnView> onColumnHovered;

    private void Start()
    {
        foreach (Transform child in slots[slots.Length - 1].transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onColumnClicked?.Invoke(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        onColumnHovered?.Invoke(this);
        backGroundRow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slots[slots.Length - 1].SetState(SlotState.EMPTY);
        backGroundRow.SetActive(false);
    }

    public void Reset()
    {
        foreach (SlotView slot in slots)
        {
            slot.SetState(SlotState.EMPTY);
        }
    }

    public void SetSlotState(int row, SlotState state)
    {
        slots[row].SetState(state);
    }
}