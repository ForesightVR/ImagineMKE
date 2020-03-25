using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance;
    [System.Serializable]
    public struct Character
    {
        public GameObject character;
        public Sprite characterImage;
    }

    public List<Character> characters;
    public Transform characterOptions;
    public CharacterTemplate characterImageTemplate;
    
    public Character CurrentCharacter { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentCharacter = characters[0];
        CurrentCharacter.character.SetActive(true);

        foreach(Character character in characters)
        {
            CharacterTemplate characterTemplate = Instantiate(characterImageTemplate, characterOptions);
            characterTemplate.SetCharacter(character);
        }
    }

    public void ChangeCharacter(Character newCharacter)
    {
        CurrentCharacter.character.SetActive(false);
        CurrentCharacter = newCharacter;
        CurrentCharacter.character.SetActive(true);
    }

    public void SelectCharacter()
    {
        NetworkConnectionManager.Instance.SetCharacter((byte)characters.IndexOf(CurrentCharacter));
    }
}
