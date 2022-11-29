using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    [SerializeField] Sprite Checked;
    [SerializeField] Sprite Unchecked;
    Button checkbox;

    private bool isChecked;

    private void Start()
    {
        checkbox = GetComponent<Button>();
    }

    public bool IsChecked
    {
        get
        {
            return isChecked;
        }
        set
        {
            isChecked = value;
        }
    }

    public void Toggle()
    {
        isChecked = !isChecked;
        checkbox.image.overrideSprite = IsChecked ? Checked : Unchecked;
    }

    public void SetChecked()
    {
        checkbox.image.overrideSprite = Checked;
    }

    public void SetUnchecked()
    {
        checkbox.image.overrideSprite = Unchecked;
    }
}
