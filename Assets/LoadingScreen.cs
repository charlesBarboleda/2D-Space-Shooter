using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public List<Sprite> bgSprites = new List<Sprite>();
    public Image _image;

    void OnEnable()
    {
        _image.sprite = bgSprites[Random.Range(0, bgSprites.Count)];
    }
}
