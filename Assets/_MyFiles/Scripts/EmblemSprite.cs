using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmblemSprite : MonoBehaviour
{
    private int mIndexNum;
    private float mSpriteIndex;
    private Color mCurColor;
    private string mColorIndex = "#FFFFFF";
    private string mObjectName;
    private GameObject EmblemEditor; 

    void Start()
    {
        EmblemEditor = GameObject.Find("EmblemEditor");
    }

    void Update()
    {
        GetComponent<MouseDrag>().thesprite.sprite = EmblemEditor.GetComponent<EmblemEditor>().AllSprites[((int)mSpriteIndex)];
    }

    public int IndexNum { get { return mIndexNum; } set { mIndexNum = value; } }
    public float SpriteIndex { get { return mSpriteIndex; } set { mSpriteIndex = value; } }
    public Color CurColor { get { return mCurColor; } set { mCurColor = value; } }
    public string ColorIndex { get { return mColorIndex; } set { mColorIndex = value; } }
}