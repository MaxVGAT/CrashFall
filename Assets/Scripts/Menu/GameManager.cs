using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadPanel()
    {

    }
}
