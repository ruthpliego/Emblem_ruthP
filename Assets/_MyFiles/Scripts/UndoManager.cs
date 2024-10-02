using System.Collections.Generic;
using UnityEngine;
using static MoveData;

public class UndoManager : MonoBehaviour
{
    private List<UndoStateExtended> undoStack = new List<UndoStateExtended>();
    private List<UndoStateExtended> redoStack = new List<UndoStateExtended>();
    private GameObject PoolObject;

    public void RecordState(string type, GameObject obj, object data = null, object previousData = null)
    {
        UndoStateExtended state = new UndoStateExtended(type, obj, data, previousData);
        undoStack.Add(state);
        redoStack.Clear();
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            UndoStateExtended state = undoStack[undoStack.Count - 1];
            undoStack.RemoveAt(undoStack.Count - 1);
            redoStack.Add(state);

            
            switch (state.Type)
            {
                case "instantiate":
                    state.Obj.SetActive(false);
                    break;
                case "color":
                    state.Obj.GetComponent<MouseDrag>().thesprite.color = (Color)state.PreviousData;
                    break;
                case "resize":
                    SpriteSize spriteSize = (SpriteSize)state.PreviousData;
                    state.Obj.GetComponent<MouseDrag>().thesprite.rectTransform.sizeDelta = spriteSize.size;
                    break;
                case "move":
                    MoveData moveData = (MoveData)state.PreviousData;
                    state.Obj.GetComponent<RectTransform>().anchoredPosition = moveData.startPosition;
                    break;
            }
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            UndoStateExtended state = redoStack[redoStack.Count - 1];
            redoStack.RemoveAt(redoStack.Count - 1);
            undoStack.Add(state);

            switch (state.Type)
            {
                case "instantiate ":
                    state.Obj.SetActive(true);
                    break;
                case "color":
                    state.Obj.GetComponent<MouseDrag>().thesprite.color = (Color)state.Data;
                    break;
                case "resize":
                    SpriteSize spriteSize = (SpriteSize)state.Data;
                    state.Obj.GetComponent<MouseDrag>().thesprite.rectTransform.sizeDelta = spriteSize.size;
                    break;
                case "move":
                    MoveData moveData = (MoveData)state.Data;
                    state.Obj.GetComponent<RectTransform>().anchoredPosition = moveData.endPosition;
                    break;
            }
        }
    }

    public bool CanUndo()
    {
        return undoStack.Count > 0;
    }

    public bool CanRedo()
    {
        return redoStack.Count > 0;
    }

    public class SpriteSize
    {
        public Vector2 size;

        public SpriteSize(Vector2 size)
        {
            this.size = size;
        }
    }
}

public class InstantiateData
{
    public string ObjectName { get; set; }
    public Sprite Sprite { get; set; }
    public Vector3 Position { get; set; }
    public Vector2 Size { get; set; }

    public InstantiateData(string objectName, Sprite sprite, Vector3 position, Vector2 size)
    {
        ObjectName = objectName;
        Sprite = sprite;
        Position = position;
        Size = size;
    }
}

public class MoveData
{
    public Vector2 startPosition;
    public Vector2 endPosition;

    public MoveData(Vector2 startPosition, Vector2 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
}

public class UndoState
{
    public string Type { get; set; }
    public GameObject Obj { get; set; }
    public object Data { get; set; }

    public UndoState(string type, GameObject obj, object data = null)
    {
        Type = type;
        Obj = obj;
        Data = data;
    }
}

public class UndoStateExtended
{
    public string Type { get; set; }
    public GameObject Obj { get; set; }
    public object Data { get; set; }
    public object PreviousData { get; set; }

    public UndoStateExtended(string type, GameObject obj, object data = null, object previousData = null)
    {
        Type = type;
        Obj = obj;
        Data = data;
        PreviousData = previousData;
    }
}