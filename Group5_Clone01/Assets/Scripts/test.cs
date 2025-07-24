using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [Header("Player 1 (WASD)")]
    public Rigidbody player1;
    public float player1Speed = 5f;

    [Header("Player 2 (Arrow Keys)")]
    public Rigidbody player2;
    public float player2Speed = 5f;

    void FixedUpdate()
    {
        float p1X = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        float p1Z = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
        Vector3 move1 = new Vector3(p1X, 0f, p1Z).normalized;
        player1.MovePosition(player1.position + move1 * player1Speed * Time.fixedDeltaTime);
     
        float p2X = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        float p2Z = Input.GetKey(KeyCode.DownArrow) ? -1 : Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        Vector3 move2 = new Vector3(p2X, 0f, p2Z).normalized;
        player2.MovePosition(player2.position + move2 * player2Speed * Time.fixedDeltaTime);
    }
}
