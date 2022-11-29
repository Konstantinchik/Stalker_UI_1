using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsKeyboardMenu : MonoBehaviour
{
    [SerializeField] Slider MouseSensitivitySlider;
    [SerializeField] GameObject InverseMouse;
    [SerializeField] GameObject KeyboardBind;

    [Header("Direction")]
    [SerializeField] InputField k_left;
    [SerializeField] InputField k_right;
    [SerializeField] InputField k_up;
    [SerializeField] InputField k_down;

    [Header("Movement")]
    [SerializeField] InputField k_forward;
    [SerializeField] InputField k_backward;
    [SerializeField] InputField k_stepLeft;
    [SerializeField] InputField k_stepRight;

    [SerializeField] InputField k_jump;
    [SerializeField] InputField k_sit;
    [SerializeField] InputField k_allwaysSit;
    [SerializeField] InputField k_step;

    [SerializeField] InputField k_run;
    [SerializeField] InputField k_lookLeft;
    [SerializeField] InputField k_lookRight;

    [Header("Weapon")]
    [SerializeField] InputField k_weapon1;
    [SerializeField] InputField k_weapon2;
    [SerializeField] InputField k_weapon3;
    [SerializeField] InputField k_weapon4;

    [SerializeField] InputField k_weapon5;
    [SerializeField] InputField k_weapon6;
    [SerializeField] InputField k_bulletType;
    [SerializeField] InputField k_nextSlot;

    [SerializeField] InputField k_previousSlot;
    [SerializeField] InputField k_fire;
    [SerializeField] InputField k_optic;
    [SerializeField] InputField k_reload;

    [SerializeField] InputField k_underBarell;
    [SerializeField] InputField k_nextMode;
    [SerializeField] InputField k_previousMode;

    void Start()
    {
        
    }

   

    void Update()
    {
        
    }
}
