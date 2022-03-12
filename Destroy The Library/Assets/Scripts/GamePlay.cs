using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    [SerializeField] private Text matchCounterText;
    [SerializeField] private Text bookCounterText;
    [SerializeField] private GameObject matchPrefab;
    [SerializeField] private GameObject bookPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<Transform> spawns;
    [SerializeField] private List<Transform> doorSpawns;
    [SerializeField] private AudioClip bookClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip successClip;

    private static GamePlay instance;
    private const int NO_BOOKS = 5;
    private const int NO_MATCHES = 5;
    private Player player;
    private int matchCounter = 0;
    private int bookCounter = 0;
    private int timeBetweenMatchInstatiation = 5;
    private int timeBetweenBookInstatiation = 5;

    /// <summary>
    /// Adds a new dialogue to be displayed.
    /// </summary>
    /// <param name="text">The text to be displayed</param>
    public static void WriteText(string text)
    {
        instance.dialogueSystem.AddWriter(text);
    }
  
    /// <summary>
    /// Plays the correct audio based on the object type.
    /// </summary>
    /// <param name="objectType">The type of the object</param>
    public static void PlayAudio(Element.Type objectType)
    {
        switch (objectType)
        {
            case Element.Type.Match:
                instance.GetComponent<AudioSource>().PlayOneShot(instance.matchClip);
                break;

            case Element.Type.Book:
                instance.GetComponent<AudioSource>().PlayOneShot(instance.bookClip);
                break;

            case Element.Type.Door:
                instance.GetComponent<AudioSource>().PlayOneShot(instance.successClip);
                break;
        }
    }

    /// <summary>
    /// Uses a match if possible.
    /// </summary>
    /// <returns>True if there was a match to be lit; False, otherwise</returns>
    public static bool UseMatch()
    {
        if (instance.matchCounter <= 0) 
            return false;

        IncreaseCounter(ref instance.matchCounter, -1, instance.matchCounterText);
        return true;
    }

    /// <summary>
    /// Adds a new object to the player's intentory.
    /// </summary>
    /// <param name="type">The type of the object to be added</param>
    public static void AddObject(Element.Type type)
    {
        switch (type)
        {
            case Element.Type.Match:
                IncreaseCounter(ref instance.matchCounter, 1, instance.matchCounterText);
                break;
            case Element.Type.Book:
                IncreaseCounter(ref instance.bookCounter, 1, instance.bookCounterText);
                instance.CreateDoorIfNeeded();
                break;
            case Element.Type.Door:
                break;
        }
    }

    /// <summary>
    /// Increases the given counter by the given amount and it updates the appropriate UI element.
    /// </summary>
    /// <param name="counter">The counter to be increased</param>
    /// <param name="amountToAdd">The amount to be added</param>
    /// <param name="stringToUpdate">The UI element to be updated</param>
    private static void IncreaseCounter(ref int counter, int amountToAdd, Text stringToUpdate)
    {
        counter += amountToAdd;
        stringToUpdate.text = counter.ToString();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        WriteText("Go through this library section and destroy all the books you find!\n You can use the matches you find to see better.\n Click <E> to light a match...");

        StartCoroutine(InitialisesObjectPool(bookPrefab, timeBetweenBookInstatiation, NO_BOOKS));
        StartCoroutine(InitialisesObjectPool(matchPrefab, timeBetweenMatchInstatiation, NO_MATCHES));
    }

    /// <summary>
    /// Initialises the objects to be used in the game.
    /// </summary>
    /// <param name="objectPrefab">The prefab of the object</param>
    /// <param name="timeoutBetweenInstatiations">The time between the creation of each object</param>
    /// <param name="maxNoObjects">The number of objects to be created</param>
    /// <returns></returns>
    private IEnumerator InitialisesObjectPool(GameObject objectPrefab, int timeoutBetweenInstatiations, int maxNoObjects)
    {
        for (int i = 0; i < maxNoObjects; i++)
        {
            StartCoroutine(this.InstantiatePrefab(objectPrefab));
            yield return new WaitForSeconds(timeoutBetweenInstatiations);
        }
    }
    
    /// <summary>
    /// Checks if a door needs to be created and does if so.
    /// </summary>
    private void CreateDoorIfNeeded()
    {
        if (this.bookCounter >= NO_BOOKS)
        {
            int index = Random.Range(0, doorSpawns.Count);
            Instantiate(doorPrefab, doorSpawns[index].position, Quaternion.identity);
            WriteText("Find the door to get to the next room!");
        }
    }

    /// <summary>
    /// Instantiates an object.
    /// </summary>
    /// <param name="prefab">The prefab of the object to be created</param>
    private IEnumerator InstantiatePrefab(GameObject prefab)
    {
        float distance;
        Transform spawn;
        int tries = 5;

        do
        {
            yield return new WaitForSecondsRealtime(1f);
            int index = Random.Range(0, spawns.Count);
            spawn = spawns[index];
            distance = Vector3.Distance(player.transform.position, spawn.position);
            tries--;
        }
        while (distance < 10 && tries > 0);

        Instantiate(prefab, spawn.position, Quaternion.identity);
        spawns.Remove(spawn);
    }
}
