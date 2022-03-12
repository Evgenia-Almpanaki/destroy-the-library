using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    private const float CHARACTER_WRITING_SPEED = .01f;
    private Text textField;
    private int characterIndex;
    private float timer;
    private bool targetTextSet;
    private string textToWrite;

    private void Awake()
    {
        textField = GetComponent<Text>();
    }

    /// <summary>
    /// Add a new dialogue to be displayed.
    /// </summary>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="timePerCharacter"></param>
    public void AddWriter(string text)
    {
        textToWrite = text;
        targetTextSet = true;
        characterIndex = 0;
    }

    private void Update()
    {
        if (targetTextSet)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                this.characterIndex++;
                if(characterIndex < textToWrite.Length)
                {
                    textField.text = textToWrite.Substring(0, this.characterIndex + 1) +
                                    "<color=#00000000>" +
                                    textToWrite.Substring(this.characterIndex + 1) +
                                    "</color>";
                }
                else
                    targetTextSet = false;

                timer += CHARACTER_WRITING_SPEED;
            }
        }
    }
}
