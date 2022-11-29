
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameManager.Instance.GoAhead();
        }
    }
}
