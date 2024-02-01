using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private CarregaFase carregaFase;

    private void Update()
    {
        // verifica clique na tela
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(carregaFase.IniciaFase("SelecaoFases"));
        }
    }
        public void IniciarJogo()
    {
        StartCoroutine(carregaFase.IniciaFase("SelecaoFases"));
    }

    public void SairJogo()
    {
        Application.Quit();

    }


}
