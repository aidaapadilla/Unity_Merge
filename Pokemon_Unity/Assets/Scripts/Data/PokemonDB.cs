using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDB : MonoBehaviour
{
    static Dictionary<string, PokemonBase> pokemons;

    public static void Init()
    {
        pokemons = new Dictionary<string, PokemonBase>(); //WATCH OUT THE MAYUS ON THE NAMES Pickachu yes, pickachu no

        var pokemonArray = Resources.LoadAll<PokemonBase>(""); //Name where you want to load the information if it is null will load everything of unity
        foreach (var pokemon in pokemonArray)
        {
            if (pokemons.ContainsKey(pokemon.Name))
            {
                Debug.LogError($"There are two pokemons with the name {pokemon.Name}");
                continue;
            }

            pokemons[pokemon.Name] = pokemon;
        }
    }

    public static PokemonBase GetPokemonByName(string name)
    {
        if (!pokemons.ContainsKey(name))
        {
            Debug.LogError($"Pokmeon with name {name} not found in the database");
            return null;
        }

        return pokemons[name];
    }
    public static List<PokemonBase> GetPokemonsAndroid(string name1, string name2, string name3)
    {
        List<PokemonBase> pokemons = new List<PokemonBase>();
        if (name1 != null)
        {
            pokemons.Add(GetPokemonByName(name1));
        }
        if (name2 != null)
        {
            pokemons.Add(GetPokemonByName(name2));
        }
        if (name3 != null)
        {
            pokemons.Add(GetPokemonByName(name3));
        }
        return pokemons;
    }
}
