using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;
using Color = UnityEngine.Color;
using System.Linq;

public class EmblemEditor : MonoBehaviour
{
    #region Variables
    [Header("Lists")]
    [Space(5)]
    [SerializeField] private List<GameObject> mSpritesInScene = new List<GameObject>();
    public List<GameObject> SpritesInScene
    { get { return mSpritesInScene; } set { mSpritesInScene = value; }}
    [SerializeField] private List<string> mNames = new List<string>();
    public List<Sprite> AllSprites = new List<Sprite>(); 
    [SerializeField] private List<string> mAllSpriteNames = new List<string>();

    [Space(5)]
    [Header("Needs")]
    [SerializeField] private GameObject PoolObject;
    [SerializeField] private GameObject mSelectedObj; 
    public GameObject SelectedObject 
    {get { return mSelectedObj; }set { mSelectedObj = value; } }
    [HideInInspector] public GameObject Resizeb; 
    private GameObject LArrow;
    private GameObject RArrow;
    [SerializeField] private Sprite mSpritePicture; 
    public Sprite SpritePicture
    { get { return mSpritePicture; } set { mSpritePicture = value; } }
    [SerializeField] private Image mSpriteHolder; 
    public Image SpriteHolder { get { return mSpriteHolder; } }
    private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";
    [SerializeField] private Transform Parent;
    private bool bColorPicked;
    private bool bPicked = true;

    //private UndoManager undoManager;
    #endregion

    #region Unity Fields
    void Start()
    {
        mNames = mAllSpriteNames;
       // undoManager = new UndoManager();
        LArrow = GameObject.Find("Arrow_L");
        RArrow = GameObject.Find("Arrow_R");

        // Initialize the undo and redo buttons
        LArrow.GetComponent<Button>().interactable = false;
        RArrow.GetComponent<Button>().interactable = false;
        GameObject Canvas;
        Canvas = GameObject.Find("Edit Area");
    }

    void Update()
    {
        if (mSelectedObj != null && mSpritesInScene != null)
        {
            //opens interactability, needed w emblemsprite
            if (mSpritesInScene.Count > 1)
            {
                if (mSelectedObj.GetComponent<EmblemSprite>().IndexNum == 0)
                {
                    LArrow.GetComponent<Button>().interactable = false;
                    RArrow.GetComponent<Button>().interactable = true;
                }
                else if (mSelectedObj.GetComponent<EmblemSprite>().IndexNum == mSpritesInScene.Count - 1)
                {
                    LArrow.GetComponent<Button>().interactable = true;
                    RArrow.GetComponent<Button>().interactable = false;
                }
                else
                {
                    LArrow.GetComponent<Button>().interactable = true;
                    RArrow.GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                LArrow.GetComponent<Button>().interactable = false;
                RArrow.GetComponent<Button>().interactable = false;
            }

            if (bColorPicked)
            {
                mSpriteHolder.color = mSelectedObj.GetComponent<MouseDrag>().thesprite.color;
                bColorPicked = false;
            }
        }
        else
        {
            LArrow.GetComponent<Button>().interactable = false;
            RArrow.GetComponent<Button>().interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void LateUpdate()
    {
        if (mSelectedObj != null)
        {
            mSpriteHolder.sprite = mSpritePicture;
        }
        if (mSpritesInScene != null)
        {
            int spriteCount = mSpritesInScene.Count;
            // Debug.Log("Number of sprites in scene: " + spriteCount);
        }
    }
    #endregion

    #region Color
    public void ColorP()
    {
        bColorPicked = true;
        Debug.Log("I am color picked");
    }

    public void ChangeColor(Color color, string colorString)
    {
        if (mSelectedObj != null)
        {
            mSelectedObj.GetComponent<MouseDrag>().thesprite.color = color;
            mSelectedObj.GetComponent<EmblemSprite>().CurColor = color;
            mSelectedObj.GetComponent<EmblemSprite>().ColorIndex = colorString;
        }
        Debug.Log("I changed Color");
       // undoManager.RecordState("color", mSelectedObj, colorString);
    }
    #endregion

    #region UnRedo Att
    /*
    public void Undo()
    {
        undoManager.Undo();
    }

    public void Redo()
    {
        undoManager.Redo();
    }

    public void MoveObject(GameObject obj, MoveData moveData)
    {
        undoManager.RecordState("move", obj, moveData);
    }

    public void ResizeObject(GameObject obj, Vector2 size)
    {
    undoManager.RecordState("resize", obj, size);
    }
    */
    #endregion

    #region Layer Att
    public void MoveSelectedObjectUp()
    {
        Debug.Log("do nothing");
        return;
    }

    public void MoveSelectedObjectDown()
    {
        Debug.Log("do nothing");
        return;
    }

    private void UpdateIndexNum()
    {
        for (int i = 0; i < mSpritesInScene.Count; i++)
        {
            mSpritesInScene[i].GetComponent<EmblemSprite>().IndexNum = i;
        }
    }

    #endregion

    #region Object Checks
    public void SelectObject(GameObject gameObj)
    {
        if (gameObj != null)
        {
            mSelectedObj = gameObj;
            Resizeb = gameObj.GetComponent<MouseDrag>().Resizeb;
            gameObj.GetComponent<MouseDrag>().bSelected = true;
            for (int i = 0; i < mSpritesInScene.Count; i++)
            {
                if (mSpritesInScene[i] != gameObj)
                {
                    mSpritesInScene[i].GetComponent<MouseDrag>().bSelected = false;
                }
            }
        }
    }

    public void Deselect()
    {
        mSelectedObj = null;
        Resizeb = null;
        for (int i = 0; i < mSpritesInScene.Count; i++)
        {
            mSpritesInScene[i].GetComponent<MouseDrag>().bSelected = false;
            mSpritesInScene[i].gameObject.transform.SetSiblingIndex(i);
            mSpriteHolder.sprite = null;
        }
    }

    public void DeleteSelObj()
    {

        if (mSelectedObj != null)
        {
            Destroy(mSelectedObj);
            mSpritesInScene.Remove(mSelectedObj);
            mSpriteHolder.sprite = null;
        }
        else
        {
            Debug.Log("Do Nothing");
        }


        mSelectedObj = null;
    }

    public void InstantiateSelObj(GameObject gameObj)
    {
        string FullName;
        for (int i = 0; i < AllSprites.Count; i++)
        {
            if (gameObj.name == AllSprites[i].name)
            {
                // Instantiate a new selSprite
                int index = AllSprites.IndexOf(AllSprites[i]);
                int range = UnityEngine.Random.Range(10, 90);
                Vector3 rot = new Vector3(0, 0, 0);
                Vector3 pos = new Vector3(0, 0, 0);
                GameObject searchSprite = Instantiate(PoolObject, transform.position, transform.rotation) as GameObject;
                Resizeb = searchSprite.GetComponent<MouseDrag>().Resizeb;
                searchSprite.name = gameObj.name + range;
                FullName = searchSprite.name;
                searchSprite.GetComponent<MouseDrag>().thesprite.sprite = AllSprites[i];
                searchSprite.GetComponent<EmblemSprite>().SpriteIndex = i; // Set the SpriteIndex variable
                mSpritesInScene.Add(searchSprite);
                mNames.Add(searchSprite.name); // Update the mNames list
                SetupSprite(searchSprite.name, searchSprite, searchSprite.transform.parent != Parent, rot);
                searchSprite.GetComponent<RectTransform>().localPosition = pos;
                mSelectedObj = searchSprite;
                mSpritePicture = searchSprite.GetComponent<MouseDrag>().thesprite.sprite;
                mSpriteHolder.color = searchSprite.GetComponent<MouseDrag>().thesprite.color;

                //undoManager.RecordState("instantiate", searchSprite, new InstantiateData(searchSprite.name, searchSprite.GetComponent<MouseDrag>().thesprite.sprite, searchSprite.GetComponent<RectTransform>().localPosition, searchSprite.GetComponent<RectTransform>().sizeDelta));
            }
        }
    }

    public void SetupSprite(string searchSprite, GameObject sprite, bool isParentChanged, Vector3 rot)
    {
        if (sprite.transform.parent != Parent)
        {
            sprite.transform.SetParent(Parent);
        }

        if (!isParentChanged)
        {
            sprite.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.14f, 17, 0);
        }
        else
        {
            Transform k = sprite.GetComponent<MouseDrag>().thesprite.gameObject.transform;
            Vector3 g = sprite.GetComponent<MouseDrag>().thesprite.gameObject.transform.rotation.eulerAngles;
            g.z = rot.z;
            k.Rotate(g);
        }

        sprite.GetComponent<EmblemSprite>().IndexNum = mSpritesInScene.IndexOf(sprite);
    }

    public void UpdateProperty(int index, GameObject selSprite, Color curColor, bool isDupe, string spriteName)
    {
        for (int t = 0; t < mSpritesInScene.Count; t++)
        {
            if (index == mSpritesInScene[t].GetComponent<EmblemSprite>().IndexNum)
            {
                selSprite.GetComponent<MouseDrag>().thesprite.color = curColor;
                selSprite.name = spriteName;
                if (!isDupe)
                {
                    selSprite.GetComponent<MouseDrag>().thesprite.sprite = mSpritesInScene[t].GetComponent<MouseDrag>().thesprite.sprite;
                }
                else
                {
                    selSprite.GetComponent<MouseDrag>().thesprite.sprite = selSprite.GetComponent<MouseDrag>().thesprite.sprite;
                }
                if (mSpritesInScene[t].GetComponent<EmblemSprite>().IndexNum != selSprite.GetComponent<EmblemSprite>().IndexNum)
                {
                    mSpritesInScene[t].GetComponent<MouseDrag>().bSelected = false;
                }
            }
        }
    }
    #endregion
}