using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInitials : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI initialText;
    [SerializeField] private Transform select;

    private bool isEnabled = false;
    private int charIndex;
    private int letterIndex;

    private CanvasGroup group;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    public void Enable()
    {
        initialText.text = "AAA";
        isEnabled = true;
        charIndex = 0;
        letterIndex = 0;
        SetSelect();
        group.alpha = 1;
    }
    private void Submit()
    {

    }
    void Update()
    {
        if (isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                letterIndex++;
                UpdateLetter();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                letterIndex--;
                UpdateLetter();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                charIndex = Mathf.Clamp(charIndex - 1, 0, 2);
                letterIndex = CharToIndex(initialText.text[charIndex]);
                SetSelect();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                charIndex = Mathf.Clamp(charIndex + 1, 0, 2);
                letterIndex = CharToIndex(initialText.text[charIndex]);
                SetSelect();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (charIndex < 2)
                {
                    charIndex++;
                    letterIndex = CharToIndex(initialText.text[charIndex]);
                    SetSelect();
                }else
                {

                }
                Submit();
            }
        }
    }
    private void SetSelect()
    {
        switch (charIndex)
        {
            case 0:
                select.position = Vector3.right * -83f;
                break;
            case 1:
                select.position = Vector3.zero;
                break;
            case 2:
                select.position = Vector3.right * 83f;
                break;
        }
    }
    private void UpdateLetter()
    {
        string txt = initialText.text;
        
        char newChar = IndexToChar(letterIndex);
        switch (charIndex)
        {
            case 0:
                txt = newChar.ToString() + txt[1] + txt[2];
                break;
            case 1:
                txt = txt[0] + newChar.ToString() + + txt[2];
                break;
            case 2:
                txt = txt[0] + txt[1] + newChar.ToString();
                break;
        }
        initialText.text = txt;
    }
    private int CharToIndex(char ch)
    {     
        //Return char
        switch (ch)
        {
            case 'A':
                return 0;
            case 'B':
                return 1;
            case 'C':
                return 2;
            case 'D':
                return 3;
            case 'E':
                return 4;
            case 'F':
                return 5;
            case 'G':
                return 6;
            case 'H':
                return 7;
            case 'I':
                return 8;
            case 'J':
                return 9;
            case 'K':
                return 10;
            case 'L':
                return 11;
            case 'M':
                return 12;
            case 'N':
                return 13;
            case 'O':
                return 14;
            case 'P':
                return 15;
            case 'Q':
                return 16;
            case 'R':
                return 17;
            case 'S':
                return 18;
            case 'T':
                return 19;
            case 'U':
                return 20;
            case 'V':
                return 21;
            case 'W':
                return 22;
            case 'X':
                return 23;
            case 'Y':
                return 24;
            case 'Z':
                return 25;
        }
        return 'A';
    }
    private char IndexToChar(int index)
    {
        //Wraparound
        if (index > 26) index = 0;
        if (index < 0) index = 26;

        letterIndex = index;
        
        //Return char
        switch (index)
        {
            case 0:
                return 'A';
            case 1:
                return 'B';
            case 2:
                return 'C';
            case 3:
                return 'D';
            case 4:
                return 'E';
            case 5:
                return 'F';
            case 6:
                return 'G';
            case 7:
                return 'H';
            case 8:
                return 'I';
            case 9:
                return 'J';
            case 10:
                return 'K';
            case 11:
                return 'L';
            case 12:
                return 'M';
            case 13:
                return 'N';
            case 14:
                return 'O';
            case 15:
                return 'P';
            case 16:
                return 'Q';
            case 17:
                return 'R';
            case 18:
                return 'S';
            case 19:
                return 'T';
            case 20:
                return 'U';
            case 21:
                return 'V';
            case 22:
                return 'W';
            case 23:
                return 'X';
            case 24:
                return 'Y';
            case 25:
                return 'Z';
        }
        return 'A';
    }
}
