using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public CharacterData characterData;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
    }
    public static CharacterData GetData() 
    {
        if (instance && instance.characterData)
        {
            return instance.characterData;
        }
        else
        {
            CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            if(characters.Length > 0)
            {
                return characters[Random.Range(0, characters.Length)];
            }
        }
        return null;
    }

    public void SelectCharacter(CharacterData character)
    {
        characterData = character;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
