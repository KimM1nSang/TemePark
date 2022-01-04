using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Text turnTxt;


    public Button StartButton
    {
        get { return startButton; }
        private set { startButton = value; }
    }

    [SerializeField]
    private Button watchButton;

    public Button WatchButton
    {
        get { return watchButton; }
        private set { watchButton = value; }
    }

    private void Awake()
    {
        Instance = this;
        turnTxt.gameObject.SetActive(false);
    }
    public void RemoveButton()
    {
        WatchButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
    }
    public void TurnText(string text,float delay)
    {
        StartCoroutine(TurnTextDelay(text, delay));
    }
    private IEnumerator TurnTextDelay(string text,float delay)
    {
        SetTurnText(text);
        turnTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        turnTxt.gameObject.SetActive(false);
    }
    public void SetTurnText(string text)
    {
        turnTxt.text = text;
    }
}
