using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;
    public event Action OnUpdated;
    public List<Pokemon> Pokemons
    {
        get { return this.pokemons; }
        set 
        { 
            pokemons = value;
            OnUpdated?.Invoke();
        }
    }

    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }
    public Pokemon GetHealthyPokemon()
    {
        //Look through the list of pokemons that we have, and returns the first pokemon that satisfies the condition
        //If the condition isn't satisfied returns null
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
 
    }
    public bool AddPokemon(Pokemon newPokemon)
    {
        if (pokemons.Count < 3)
        {
            pokemons.Add(newPokemon);
            OnUpdated?.Invoke();
            return true;
        }
        else
        {
            //TO DO: Add to the PC once that's implemented
            //Choose a pokemon
            return false;
        }
    }
    public void getPokemons(String name1, String name2, String name3)
    {
        PokemonBase poke1 = PokemonDB.GetPokemonByName(name1);
        Pokemon pokemon1 = new Pokemon(poke1, 2);
        pokemons.Add(pokemon1);

        PokemonBase poke2 = PokemonDB.GetPokemonByName(name1);
        Pokemon pokemon2 = new Pokemon(poke2, 2);
        pokemons.Add(pokemon2);

        PokemonBase poke3 = PokemonDB.GetPokemonByName(name1);
        Pokemon pokemon3 = new Pokemon(poke3, 2);
        pokemons.Add(pokemon3);


    }
    public static PokemonParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PokemonParty>();
    }
}
