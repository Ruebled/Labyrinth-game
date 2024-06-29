using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfQuestions {  get; private set; }

    public List<Question> questions = new List<Question>();
    
    public void CollectQuestion(Question question)
    {
        questions.Add(question);
        NumberOfQuestions++;
    }
}
