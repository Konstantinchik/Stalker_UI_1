using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    public Texture2D[] Cursors;
    private Texture2D cur;
    float TimerChange;
    float TimerDown;
    int i;



    void Start()
    {
        TimerChange = 1f;
        UnityEngine.Cursor.visible = false;
        TimerDown = TimerChange;
        cur = Cursors[0];
    }

    void Update()
    {
        UnityEngine.Cursor.visible = false;
       
        TimerDown -= 0.2f; // Глюк с Time.deltaTime. Выдает 0  вместо дробного числа
        
        //TimerDown -= Time.deltaTime ;
        if (TimerDown < 0)
        {
            // Debug.Log(i);
            if (i >= Cursors.Length - 1) i = 0;
            cur = Cursors[i];
            i++;
            TimerDown = TimerChange;
        }
        
    }

    void OnGUI()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        mousePos.x -= 5;
        mousePos.y -= 5;

        GUI.DrawTexture(new Rect(mousePos.x, mousePos.y, 60, 60), cur);
    }
}
