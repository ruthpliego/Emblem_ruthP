using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using Unity.VisualScripting;
public class MouseDrag : MonoBehaviour
{
    public bool bSelected;
    private RectTransform mRectTransform;
    public Image thesprite;
    public string SpriteName;
    public GameObject Resizeb;
    GameObject EmblemEd;
    private int LastIndex;

    void Start()
    {
        EmblemEd = GameObject.Find("EmblemEditor");
    }

    void Update()
    {
        if (Resizeb != EmblemEd.GetComponent<EmblemEditor>().Resizeb)
        {
            Resizeb.GetComponent<Image>().enabled = false;
        }
        else
        {
            Resizeb.GetComponent<Image>().enabled = true;
        }

        SpriteName = this.gameObject.name;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if (bSelected)
        {
            var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
            if (pointerData == null) { return; }


            var currentPosition = mRectTransform.position;
            currentPosition.x += pointerData.delta.x;
            currentPosition.y += pointerData.delta.y;
            mRectTransform.position = currentPosition;
            //Debug.Log(currentPosition);
        }

    }
    public void OnPointerClick(UnityEngine.EventSystems.BaseEventData eventData)
    {
        bSelected = true;
        for (int i = 0; i < EmblemEd.GetComponent<EmblemEditor>().SpritesInScene.Count; i++)
        {
            if (EmblemEd.GetComponent<EmblemEditor>().SpritesInScene[i].GetComponent<EmblemSprite>().IndexNum != GetComponent<EmblemSprite>().IndexNum)
            {
                EmblemEd.GetComponent<EmblemEditor>().SpritesInScene[i].GetComponent<MouseDrag>().bSelected = false;
            }
        }
        if (bSelected)
        {
            var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
            if (pointerData == null) { return; }


            var currentPosition = mRectTransform.position;
            currentPosition.x += pointerData.delta.x;
            currentPosition.y += pointerData.delta.y;
            mRectTransform.position = currentPosition;

            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(SelectedObjectvoid);
            entry.callback.AddListener(call);
            trigger.triggers.Add(entry);
        }
    }
    public void SelectedObjectvoid(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (bSelected)
        {
            EmblemEd.GetComponent<EmblemEditor>().SelectedObject = this.gameObject;
            EmblemEd.GetComponent<EmblemEditor>().SpritePicture = thesprite.sprite;
            EmblemEd.GetComponent<EmblemEditor>().SpriteHolder.color = thesprite.color;
            EmblemEd.GetComponent<EmblemEditor>().Resizeb = Resizeb;

            LastIndex = GetComponent<EmblemSprite>().IndexNum;
            int j;
            j = EmblemEd.GetComponent<EmblemEditor>().SpritesInScene.Count;
            j -= 1;
            EmblemEd.GetComponent<EmblemEditor>().SelectedObject.transform.SetSiblingIndex(j);
        }
        else
        {
            this.gameObject.transform.SetSiblingIndex(LastIndex);
        }
    }

    /*public bool bSelected;
    private RectTransform mRectTransform;
    public Image thesprite;
    public string SpriteName;
    public GameObject Resizeb;
    GameObject EmblemEd;
    private int LastIndex; 
    
    private Vector2 startPosition;
    private Vector2 previousPosition;
    private Vector2 endPosition;

    void Start()
    {
        EmblemEd = GameObject.Find("EmblemEditor");
    }

    void Update()
    {
        if (Resizeb != EmblemEd.GetComponent<EmblemEditor>().Resizeb)
        {
            Resizeb.GetComponent<Image>().enabled = false;
        }
        else
        {
            Resizeb.GetComponent<Image>().enabled = true;
        }

        SpriteName = this.gameObject.name;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if (bSelected)
        {
            var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
            if (pointerData == null) { return; }

            // Move the sprite
            var currentPosition = mRectTransform.position;
            currentPosition.x += pointerData.delta.x;
            currentPosition.y += pointerData.delta.y;
            mRectTransform.position = currentPosition;

            // Store the end position of the move
            endPosition = mRectTransform.anchoredPosition;
        }
    }


    public void OnPointerDown(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if (bSelected)
        {
            var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
            if (pointerData == null) { return; }

            // Capture the start position
            startPosition = mRectTransform.anchoredPosition;

            // Record the initial state for undo (before the move occurs)
            MoveData moveData = new MoveData(startPosition, startPosition); // Since movement hasn't started yet
            EmblemEd.GetComponent<UndoManager>().RecordState("move", this.gameObject, moveData);

            Debug.Log("start pos: " + startPosition);
        }
    }


    public void OnPointerUp(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if (bSelected)
        {
            // Create a new MoveData object
            MoveData moveData = new MoveData(previousPosition, endPosition);
            Debug.Log("Move data: " + moveData);

            // Record the move as a state change in UndoManager
            EmblemEd.GetComponent<UndoManager>().RecordState("move", this.gameObject, moveData);

            // Reset the end position
            endPosition = Vector2.zero;
        }
    }


    public void OnPointerClick(UnityEngine.EventSystems.BaseEventData eventData)
    {
        bSelected = true;
        for (int i = 0; i < EmblemEd.GetComponent<EmblemEditor>().SpritesInScene.Count; i++)
        {
            if (EmblemEd.GetComponent<EmblemEditor>().SpritesInScene[i].GetComponent<EmblemSprite>().IndexNum != GetComponent<EmblemSprite>().IndexNum)
            {
                EmblemEd.GetComponent<EmblemEditor>().SpritesInScene[i].GetComponent<MouseDrag>().bSelected = false;
            }
        }
        if (bSelected)
        {
            var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
            if (pointerData == null) { return; }

            var currentPosition = mRectTransform.position;
            currentPosition.x += pointerData.delta.x;
            currentPosition.y += pointerData.delta.y;
            mRectTransform.position = currentPosition;

            EventTrigger trigger = GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback = new EventTrigger.TriggerEvent();
            UnityEngine.Events.UnityAction<BaseEventData> callDown = new UnityEngine.Events.UnityAction<BaseEventData>(OnPointerDown);
            entryDown.callback.AddListener(callDown);
            trigger.triggers.Add(entryDown);

            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback = new EventTrigger.TriggerEvent();
            UnityEngine.Events.UnityAction<BaseEventData> callUp = new UnityEngine.Events.UnityAction<BaseEventData>(OnPointerUp);
            entryUp.callback.AddListener(callUp);
            trigger.triggers.Add(entryUp);
        }
    }

    public void SelectedObjectEvent(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        if (bSelected)
        {
            EmblemEd.GetComponent<EmblemEditor>().SelectedObject = this.gameObject;
            EmblemEd.GetComponent<EmblemEditor>().SpritePicture = thesprite.sprite;
            EmblemEd.GetComponent<EmblemEditor>().SpriteHolder.color = thesprite.color;
            EmblemEd.GetComponent<EmblemEditor>().Resizeb = Resizeb;

            LastIndex = GetComponent<EmblemSprite>().IndexNum;
            int i;
            i = EmblemEd.GetComponent<EmblemEditor>().SpritesInScene.Count;
            i -= 1;
            EmblemEd.GetComponent<EmblemEditor>().SelectedObject.transform.SetSiblingIndex(j);
        }
        else
        {
            this.gameObject.transform.SetSiblingIndex(LastIndex);
        }
    }*/
}