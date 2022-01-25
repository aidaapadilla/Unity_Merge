using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Color highlightedColor;
    Pokemon _pokemon;

    public void Init(Pokemon pokemon)
    {
        _pokemon = pokemon;
        UpdateData();
        _pokemon.OnHPChanged += UpdateData;
    }
    void UpdateData()
    {
        nameText.text = _pokemon.Base.Name;
        levelText.text = "Lvl" + _pokemon.Level;
        float calc = (float)_pokemon.HP / _pokemon.MaxHp;
        hpBar.SetHP(calc);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = highlightedColor;
        else
            nameText.color = Color.black;
    }
}
