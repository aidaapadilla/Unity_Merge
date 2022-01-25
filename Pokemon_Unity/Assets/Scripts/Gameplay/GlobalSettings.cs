using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] Color higlightedColor;

    public Color HighlightedColor => higlightedColor;
    public static GlobalSettings i { get; private set; }
    private void Awake()
    {
        i = this;
    }
}
