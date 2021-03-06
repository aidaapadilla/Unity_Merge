using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text countText;

    RectTransform rectTransform;

    public Text NameText => nameText;
    public Text CountText => countText;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public float Height => rectTransform.rect.height;
    public void SetData(ItemSlot itemSlot)
    {
        nameText.text = itemSlot.Item.Name;
        countText.text = $"X {itemSlot.Count}";
    }
}
