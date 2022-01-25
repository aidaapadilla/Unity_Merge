using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text statusText;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    Pokemon _pokemon;
    Dictionary<ConditionID, Color> statusColors;
    public void SetData(Pokemon pokemon)
    {
        if (_pokemon != null)
        {
            _pokemon.OnStatusChanged -= SetStatusText;
            _pokemon.OnHPChanged -= UpdateHP;
        }
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl" + pokemon.Level;
        _pokemon = pokemon;
        float calc = (float)pokemon.HP / pokemon.MaxHp;
        hpBar.SetHP(calc);
        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColor },
            {ConditionID.brn, brnColor },
            {ConditionID.slp, slpColor },
            {ConditionID.par, parColor },
            {ConditionID.frz, frzColor },
        };
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
        _pokemon.OnHPChanged += UpdateHP;
    }
    void SetStatusText()
    {
        if (_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }
    public void UpdateHP()
    {
        StartCoroutine(HPUpdateAsync());
    }
    public IEnumerator HPUpdateAsync()
    {
        float calc = (float)_pokemon.HP / _pokemon.MaxHp;
        yield return hpBar.SetHPSmooth(calc);
    }
    public IEnumerator WaitForHPUpdate()
    {
        yield return new WaitUntil(() => hpBar.IsUpdating == false);
    }
}
