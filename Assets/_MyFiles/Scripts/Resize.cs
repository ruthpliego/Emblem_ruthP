using UnityEngine.EventSystems;
using UnityEngine;

public class Resize : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 minSize = new Vector2(40f, 40f);
    private Vector2 maxSize = new Vector2(460f, 400f);

    public RectTransform rectTransform;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;
    //private Vector2 previousSize;

    public void OnPointerDown(PointerEventData data)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
        //previousSize = rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData data)
    {
        if (rectTransform == null)
            return;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out currentPointerPosition);
        Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

        sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
            Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
            );

        rectTransform.sizeDelta = sizeDelta;

        previousPointerPosition = currentPointerPosition;
    }

    /*public void OnPointerUp(PointerEventData data)
    {
        Record the resize as a state change
        if (previousSize != rectTransform.sizeDelta)
        {
            GameObject EmblemEd = GameObject.Find("EmblemEditor");
            UndoManager undoManager = EmblemEd.GetComponent<UndoManager>();
            undoManager.RecordState("resize", this.gameObject, new UndoManager.SpriteSize(rectTransform.sizeDelta), new UndoManager.SpriteSize(previousSize));
        }
    }*/
}