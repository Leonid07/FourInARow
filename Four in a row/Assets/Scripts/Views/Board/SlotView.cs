using DG.Tweening;
using UnityEngine;

public class SlotView : BaseView
{
    public Canvas canvas;
    [SerializeField] private GameObject elementToMoveBlue;
    [SerializeField] private GameObject elementToMoveRed;
    public SlotState state;

    public RectTransform startPosition;

    private RectTransform targetElement;

    private void Start()
    {
        targetElement = GetComponent<RectTransform>();
    }

    GameObject blue, red;

    public void SetState(SlotState state)
    {
        switch (state)
        {
            case SlotState.WHITE:
                if (blue != null) Destroy(blue);
                blue = Instantiate(elementToMoveBlue.gameObject, transform, true);
                blue.GetComponent<RectTransform>().position = startPosition.position;
                blue.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                RectTransform targetForBlue = blue.GetComponent<RectTransform>();
                MoveToTarget(targetElement, targetForBlue);
                blue.SetActive(true);
                break;
            case SlotState.RED:
                if (red != null) Destroy(red);
                red = Instantiate(elementToMoveRed.gameObject, transform, true);
                red.GetComponent<RectTransform>().position = startPosition.position;
                red.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                RectTransform targetForRed = red.GetComponent<RectTransform>();
                MoveToTarget(targetElement, targetForRed);
                red.SetActive(true);
                break;
            case SlotState.EMPTY:
            default:
                if (blue != null) Destroy(blue);
                if (red != null) Destroy(red);
                break;
        }
        this.state = state;
    }

    public void MoveToTarget(RectTransform targetElement, RectTransform elementToMove)
    {
        // Получаем мировую позицию целевого элемента
        Vector3 worldPosition = targetElement.position;

        // Преобразуем мировую позицию в экранную позицию
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPosition);

        // Преобразуем экранную позицию в локальные координаты элемента
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(elementToMove.parent.GetComponent<RectTransform>(), screenPosition, canvas.worldCamera, out localPosition);

        // Используем DOTween для перемещения элемента
        elementToMove.DOLocalMove(localPosition, 1.0f).SetEase(Ease.InOutQuad).OnKill(() => elementToMove = null);
    }
}
