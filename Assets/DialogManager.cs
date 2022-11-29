using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Система диалогов в Unity : by.Brackeys
/// </summary>
public class DialogManager : MonoBehaviour
{
    private Queue<string> sentences;


    void Start()
    {
        sentences = new Queue<string>();

    }

    
    void Update()
    {
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.name);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // если мы достигли конца нашего Enqueue и можем закончить диалог
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
    }
}
