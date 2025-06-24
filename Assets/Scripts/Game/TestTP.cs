using UnityEngine;

public class TestTP : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Teleporting player +1 X");
            Player.transform.position = new Vector3(3.5f, 13.75f, 0f);
        }
    }
}