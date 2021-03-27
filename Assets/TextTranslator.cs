using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTranslator : MonoBehaviour
{
    public TMP_Text TMPText;
    public Text Text;
    [TextArea]
    public string engText, rusText;
    public GameManager manager;
    public WebManager webManager;
    

    private void Start()
    {
        webManager = FindObjectOfType<WebManager>();
           manager = FindObjectOfType<GameManager>();
        TMPText = GetComponent<TMP_Text>();
        Text = GetComponent<Text>();
    }
    private void Update()
    {
        if (manager != null)
        {
            if (TMPText != null)
            {
                if (manager.rus == false)
                    TMPText.text = engText;
                else
                    TMPText.text = rusText;
            }
            if (Text != null)
            {
                if (manager.rus == false)
                    TMPText.text = engText;
                else
                    TMPText.text = rusText;
            }
        }
        else
        {
            if (webManager != null)
            {
                if (TMPText != null)
                {
                    if (webManager.rus == false)
                        TMPText.text = engText;
                    else
                        TMPText.text = rusText;
                }
                if (Text != null)
                {
                    if (webManager.rus == false)
                        TMPText.text = engText;
                    else
                        TMPText.text = rusText;
                }
            }
        }
    }
}
