using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    
    [SerializeField] List<EnemyNPC> characterList = new List<EnemyNPC>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
         foreach (var character in characterList)
         {
             character.UpdateBehaviour();
         }
    }

    public void AddCharacter(EnemyNPC character)
    {
        characterList.Add(character);
    }

    public void RemoveCharacter(EnemyNPC character)
    {
        characterList.Remove(character);
    }

    public void AddCharacters(EnemyNPC[] characters)
    {
        characterList.AddRange(characters);
    }

    public void RemoveCharacters(EnemyNPC[] characters)
    {
        foreach (var character in characters)
        {
            characterList.Remove(character);
        }
    }

}
