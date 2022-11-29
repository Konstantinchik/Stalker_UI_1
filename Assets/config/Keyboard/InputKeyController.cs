using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Convert pressed buttons in string
/// </summary>
public class InputKeyController : MonoBehaviour
{
    private static int trig = 0;
    private Button m_button;
    [SerializeField] InputField m_inputField;

    void Start()
    {
        m_button = GetComponent<Button>();
    }

    public void Click()
    {
        trig++;
        GameManager.Instance.KeyBind = true;
        m_inputField.GetComponent<Image>().color = Color.yellow;
        if (trig > 0)
        {
            GameManager.Instance.CheckKey(m_inputField);
            trig = 1;
        }
    }


}
