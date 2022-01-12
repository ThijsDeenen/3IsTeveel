using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private int chosenLevel;
    private List<int> chosenLevelList = new List<int>();
    private int playerCount;
    private bool gameIsPLaying;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };
    
    void Update()
    {
        if (!gameIsPLaying)
        {
            //This is where every individual player picks a number. Should be connected to an NFC scanner.
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    int numberPressed = i + 1;
                    playerCount++;
                    chosenLevel = numberPressed;
                    chosenLevelList.Add(chosenLevel);
                }
            }
            //This is where the host starts the game by pressing the button on their controller (should check if all connected devices have selected a number.)
            if (Input.GetMouseButtonDown(0))
            {
                if (playerCount > 0 && chosenLevelList.Count == playerCount) gameIsPLaying = true;
            }
        }
        else
        {
            //This is where someone starts walking to a different spot (should check if device is not close to other devices anymore.)
            if (Input.GetKeyDown(KeyCode.Space)) GiveFunction();
        }
    }

    public void ChooseTableLevel(int level)
    {
        chosenLevel = level;
    }

    public void GiveFunction()
    {
        int multiplier = Random.Range(1, 11);
        StartCoroutine(CallTextToSpeech(chosenLevel + " x " + multiplier));
        Debug.Log(chosenLevel + " x " + multiplier + " = " + (chosenLevel * multiplier));
    }

    private IEnumerator CallTextToSpeech(string spokenText)
    {
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + spokenText + "&tl=Nl";
        WWW www = new WWW(url);
        yield return www;
        audioSource.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        audioSource.Play();
    }
}



