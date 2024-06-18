using UnityEngine;

public class movimentController : MonoBehaviour
{
    public float velocidadeMovimento = 5f; // Velocidade de movimento do personagem

    // Update is called once per frame
    void Update()
    {
        // Captura os inputs de movimento nas teclas W, S, A, D
        float movimentoVertical = Input.GetAxis("Vertical");
        float movimentoHorizontal = Input.GetAxis("Horizontal");

        // Calcula a direção de movimento baseada nos inputs
        Vector3 movimento = new Vector3(movimentoHorizontal, movimentoVertical, 0f) * velocidadeMovimento * Time.deltaTime;

        // Aplica o movimento ao transform do personagem
        transform.Translate(movimento);
    }
}
