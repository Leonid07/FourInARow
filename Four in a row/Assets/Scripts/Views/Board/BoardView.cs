using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardView : BaseView
{
    // Массив ссылок на колонки
    [SerializeField]
    private ColumnView[] columns;

    // Событие, которое вызывается при клике на колонку
    public Action<int> onColumnClicked;
    // Событие, которое вызывается при наведении на колонку
    public Action<int> onColumnHovered;

    // Метод, вызываемый при старте объекта
    public void Start()
    {
        // Подписываемся на события клика и наведения для каждой колонки
        foreach (ColumnView column in columns)
        {
            column.onColumnClicked += HandleColumnClicked;
            column.onColumnHovered += HandleColumnHovered;
        }
    }

    // Переопределенный метод сброса состояния
    public void Reset()
    {
        // Сбрасываем каждую колонку
        foreach (ColumnView column in columns)
        {
            column.Reset();
        }
    }

    // Метод, вызываемый при уничтожении объекта
    private void OnDestroy()
    {
        // Отписываемся от событий клика и наведения для каждой колонки
        foreach (ColumnView column in columns)
        {
            column.onColumnClicked -= HandleColumnClicked;
            column.onColumnHovered -= HandleColumnHovered;
        }
    }

    // Устанавливаем состояние ячейки в колонке
    public void SetSlotState(int col, int row, SlotState state)
    {
        columns[col].SetSlotState(row, state);
    }

    // Устанавливаем состояние ячейки при наведении в колонке
    public void SetHoverSlotState(int col, SlotState state)
    {
        int hoverIndex = columns.Length - 1;
        SetSlotState(col, hoverIndex, state);
    }

    // Обработчик события клика на колонку
    private void HandleColumnClicked(ColumnView column)
    {
        // Находим индекс колонки и вызываем соответствующее событие
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i] == column)
            {
                onColumnClicked?.Invoke(i);
                break;
            }
        }
    }

    // Обработчик события наведения на колонку
    private void HandleColumnHovered(ColumnView column)
    {
        // Находим индекс колонки и вызываем соответствующее событие
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i] == column)
            {
                onColumnHovered?.Invoke(i);
                break;
            }
        }
    }
}
