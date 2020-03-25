using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTemplate : MonoBehaviour
{
    public Image Image { get; private set; }
    public CharacterSelection.Character Character { get; private set; }

    public void SetCharacter(CharacterSelection.Character character)
    {
        if(!Image)
            Image = GetComponent<Image>();

        Character = character;
        Image.sprite = Character.characterImage;
    }

    public void SelectCharacter()
    {
        CharacterSelection.Instance.ChangeCharacter(Character);
    }
}
