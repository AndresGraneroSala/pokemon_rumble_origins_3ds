using System;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager instance;

    public LetterFont[] letters = new LetterFont[78];
    public GameObject letterPrefab;
    public float letterSpacing = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Test Text")]
    public void TestText()
    {
        //InstanceText("AA BBB", Vector3.zero,Color.white);
    }

    public void InstanceText(string text, Vector3 position,Color color)
    {
        return;
        List<Sprite> sprites = SelectSprites(text);

        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject letterGO = Instantiate(letterPrefab, position + new Vector3(i * letterSpacing, 0, 0), letterPrefab.transform.rotation);
            SpriteRenderer sr = letterGO.GetComponent<SpriteRenderer>();
            sr.sprite = sprites[i];
            sr.color = color;
        }
    }

    private List<Sprite> SelectSprites(string text)
    {
        List<Sprite> sprites = new List<Sprite>();

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i]; // Asegura minúsculas
            for (int j = 0; j < letters.Length; j++)
            {
                if (letters[j].letter == currentChar)
                {
                    sprites.Add(letters[j].sprite);
                    break;
                }
            }
        }

        return sprites;
    }
}

[Serializable]
public class LetterFont
{
    public char letter;
    public Sprite sprite;
}