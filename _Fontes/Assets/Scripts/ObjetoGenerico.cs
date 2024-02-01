using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ObjetoGenerico : MonoBehaviour
{
    private Alerta alerta;
    private Personagem personagem;
    private Patinete patinete;
    // Start is called before the first frame update
    void Start()
    {
        GameObject container = GameObject.Find("Container");
        alerta = container.GetComponent<Alerta>();
        personagem = FindObjectOfType<Personagem>();
        patinete = FindObjectOfType<Patinete>();
    }

    public void OnPointerEnter()
    {
        string nomeObjeto = this.gameObject.name;

        switch (nomeObjeto)
        {
            case "LadoEsquerdo" when alerta.mostrouAlertaOlharDoisLados:
                alerta.MostrarAlerta("Esquerda");
                personagem.olhouEsquerda = true;
                break;
            case "LadoDireito" when alerta.mostrouAlertaOlharDoisLados:
                alerta.MostrarAlerta("Direita");
                personagem.olhouDireita = true;
                break;
            case "CaixaBotaoSemaforo":
                alerta.MostrarAlerta("CaixaBotaoSemaforo");
                break;
            case "Botao":
                //alerta.MostrarAlerta("CaixaBotaoSemaforo");
                alerta.MostrarAlerta("Botao");
                break;
            case "Capacete" when patinete.subiuPatinete:
                patinete.olhouCapacete = true;
                alerta.MostrarAlerta("ColocarCapacete");
                break;
            default:
                break;
        }

     //   Debug.Log("NOME DO OBJETO FIXADO O OLHAR " + this.gameObject.name);
    }


    public void OnPointerExit()
    {
        alerta.OcultarAlerta(this.gameObject.name);
    }

}