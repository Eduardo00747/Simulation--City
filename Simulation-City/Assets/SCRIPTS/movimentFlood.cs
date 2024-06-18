using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentFlood : MonoBehaviour
{
    public float targetY = 6.10f; // Posição alvo em Y
    public float speed = 1.0f; // Velocidade do movimento

    // Update is called once per frame
    void Update()
    {
        // Verifica se a posição Y atual é menor que a posição alvo
        if (transform.position.y < targetY)
        {
            // Calcula a nova posição Y incrementando gradativamente
            float newY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
