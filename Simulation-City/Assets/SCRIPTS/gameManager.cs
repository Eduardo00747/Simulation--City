using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enchente; // Referência ao objeto "Enchente"

    [SerializeField]
    private GameObject alagamento; // Referência ao objeto "Enchente"

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DesativarEnchenteDepoisDeTempo(20f)); // Iniciar a coroutine para desativar depois de 14 segundos
        StartCoroutine(AtivarAlagamentoDepoisDeTempo(13f)); // Iniciar a coroutine para ativar depois de 14 segundos

    }

    IEnumerator DesativarEnchenteDepoisDeTempo(float tempo)
    {
        yield return new WaitForSeconds(tempo); // Esperar o tempo especificado
        enchente.SetActive(false); // Desativar o objeto "Enchente"
    }

    IEnumerator AtivarAlagamentoDepoisDeTempo(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        alagamento.SetActive(true);
    }
}
