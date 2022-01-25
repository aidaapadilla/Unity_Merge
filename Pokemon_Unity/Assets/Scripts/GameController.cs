using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState { FreeRoam, Battle, Dialog, PartyScreen, Bag, Cutscene, Menu }

public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;
    GameState state;

    //Enviat desde Android
    public string iduser;
    public string charactername;
    public string pokemon1;
    public string pokemon2;
    public string pokemon3;
    public string object1;
    public string object2;
    public string object3;
    public int skin;

    //Enviat a Unity
    public string map;
    public string experience;

    public static GameController Instance { get; private set; }
    MenuController menuController;
    public int getAvatar
    {
        get { return skin; }
    }
    private void Awake()
    {
        Instance = this;
        menuController = GetComponent<MenuController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ConditionsDB.Init();
        PokemonDB.Init();
        MoveDB.Init();
    }
    private void Start()
    {
#if UNITY_ANDROID
            pokemon1=null;
            pokemon2=null;
            pokemon3=null;
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            
            Debug.Log("Searching username and skin");

            charactername = intent.Call<string>("getStringExtra", "charactername");
            skin = intent.Call<int>("getIntExtra", "avatarname", 0); 

            Debug.Log("Charactername is "+charactername);
            Debug.Log("Avatar number is " +skin.ToString());
            
            bool hasExtra = intent.Call<bool>("hasExtra", "pokemon1");

            if (hasExtra)
            {
                Debug.Log("Inside If Pokemon1");
                pokemon1 = intent.Call<string>("getStringExtra", "pokemon1");
                Debug.Log("First Pokemon is "+pokemon1);
            }

            hasExtra = intent.Call<bool>("hasExtra", "pokemon2");

            if (hasExtra)
            {
                Debug.Log("Inside If Pokemon2");
                pokemon2 = intent.Call<string>("getStringExtra", "pokemon2");
                Debug.Log("Second Pokemon is "+pokemon2);
            }

            hasExtra = intent.Call<bool>("hasExtra", "pokemon3");

            if (hasExtra)
            {
                Debug.Log("Inside If Pokemon3");
                pokemon3 = intent.Call<string>("getStringExtra", "pokemon3");
                Debug.Log("Third Pokemon is "+pokemon3);
            }

            hasExtra = intent.Call<bool>("hasExtra", "object1");

            if (hasExtra)
            {
                Debug.Log("Inside If Object1");
                object1 = intent.Call<string>("getStringExtra", "object1");
                Debug.Log("Object1 is "+object1);
            }

            hasExtra = intent.Call<bool>("hasExtra", "object2");

            if (hasExtra)
            {
                Debug.Log("Inside If Object2");
                object2 = intent.Call<string>("getStringExtra", "object2");
                Debug.Log("Object2 is "+object2);
            }

            hasExtra = intent.Call<bool>("hasExtra", "object3");

            if (hasExtra)
            {
                Debug.Log("Inside If Object3");
                object3 = intent.Call<string>("getStringExtra", "object3");
                Debug.Log("Object3 is "+object3);
            }

#endif
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        playerController.OnEnteredTrainersView += (Collider2D trainerCollider) => 
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
         DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
        menuController.onBack += () =>
         {
             state = GameState.FreeRoam;
         };
        menuController.onMenuSelected += OnMenuSelected;
        battleSystem.getAndroidInfo(pokemon1, pokemon2, pokemon3, object1, object2, object3);
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetWildPokemon();
        
        var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);
        
        battleSystem.StartBattle(playerParty, wildPokemonCopy);
    }

    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        this.trainer = trainer;
        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();
        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }
    TrainerController trainer;
    void EndBattle(bool won)
    {
        if(trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action onSelected = () =>
            {
                // TODO: Go to Summary Screen
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };
            Action onItemUsed = () =>
            {
                //state = GameState.Bag;
                inventoryUI.gameObject.SetActive(false);
                //StartCoroutine(RunTurns(BattleAction.UseItem));
            };
            inventoryUI.HandleUpdate(onBack, onItemUsed);
        }
    }
    void OnMenuSelected(int selectedItem)
    {
        if(selectedItem == 0)
        {
            //Pokemon selected
            partyScreen.gameObject.SetActive(true);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 1)
        {
            //Bag selected
            inventoryUI.gameObject.SetActive(true);
            partyScreen.Init();
            state = GameState.Bag;
        }
        else if (selectedItem == 2)
        {
#if UNITY_ANDROID
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("dsa.ejercicios_practica.pokemon_android.IntegrationUnity");

            experience = "27";
            Debug.Log("The experience sent is "+experience);
            map="SampleScene";
            Debug.Log("The map sent is "+map);
            UnityPlayer.CallStatic("getInformationUnity", experience, map, charactername);
            Debug.Log("Information sent");

            Debug.Log("Before closing game");
            Application.Quit();

#endif
        }
        else if (selectedItem == 3)
        {
            //Load selected 
            SavingSystem.i.Load("saveSlot1");
            state = GameState.FreeRoam;
        }
    }

}
