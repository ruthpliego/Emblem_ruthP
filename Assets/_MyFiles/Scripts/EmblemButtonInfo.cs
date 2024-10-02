using UnityEngine;
using System.Collections.Generic;

public class EmblemButtonInfo : MonoBehaviour
{

    private List<Sprite> Sprites = new List<Sprite>();
    private GameObject CurObject;
    private Transform gameObj;
    private Color color;

    void Update()
    {
        GameObject gameObj;
        gameObj = GameObject.Find("EmblemEditor");
        Sprites = gameObj.GetComponent<EmblemEditor>().AllSprites;

        if (CurObject != null)
        {
            color = CurObject.GetComponent<MouseDrag>().thesprite.color;
        }
    }

    void Start()
    {
        gameObj = this.transform.Find("Spritepic");
        color = Color.white;
    }

    private void Check()
    {
        GameObject go;
        go = GameObject.Find("EmblemEditor");
        go.GetComponent<EmblemEditor>().InstantiateSelObj(this.gameObject);
    }
}