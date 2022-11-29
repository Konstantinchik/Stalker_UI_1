using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    Transform canvasTransform;

    private void Awake()
    {
        canvasTransform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        canvasTransform.SetAsFirstSibling();
    }

}
