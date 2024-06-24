using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtivarButton : MonoBehaviour
{
    [SerializeField]
    private GameObject zibraLiquid1;

    [SerializeField]
    private Button buttonChuva;

    [SerializeField]
    private Button buttonSol;

    // Start is called before the first frame update
    void Start()
    {
        // Adicionar listeners aos botões
        buttonChuva.onClick.AddListener(AtivarZibraLiquid);
        buttonSol.onClick.AddListener(DesativarZibraLiquid);
    }

    // Método para ativar o objeto
    void AtivarZibraLiquid()
    {
        zibraLiquid1.SetActive(true);
    }

    // Método para desativar o objeto
    void DesativarZibraLiquid()
    {
        zibraLiquid1.SetActive(false);
    }
}

