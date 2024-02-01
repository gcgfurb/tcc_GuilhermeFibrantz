using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Alerta : MonoBehaviour
{
    [SerializeField] private GameObject mensagemAlerta;
    [SerializeField] private TMP_Text texto;
    [SerializeField] private GameObject esquerda;
    [SerializeField] private GameObject direita;
    [SerializeField] private GameObject personagem;
    [SerializeField] private GameObject textoBotaoSemaforo;
    [SerializeField] private GameObject controle;
    [SerializeField] private bool alertaVisivel = false;
    [SerializeField] private bool estaFrenteBotao = false;
    [SerializeField] private bool estaFrenteFaixa = false;
    [SerializeField] public bool podeAcionarBotao = false;
    [SerializeField] private Patinete pt;
    [SerializeField] private bool mostrouAlerta = false;
    

    [SerializeField] private Sprite controleA;
    [SerializeField] private Sprite controleB;
    [SerializeField] private Sprite controleC;
    [SerializeField] private Sprite controleD;
    [SerializeField] private Sprite controleAnalogico;
    [SerializeField] private Sprite controleGSuperior;
    [SerializeField] private Sprite controleGInferior;

    public bool mostrouAlertaOlharDoisLados;
    private SpriteRenderer controleSpriteRenderer;
    private string alertaParametro;

    void Start()
    {
        pt = GameObject.FindAnyObjectByType<Patinete>();
        if (controle != null)
        {
            controleSpriteRenderer = controle.GetComponent<SpriteRenderer>();
        }
    }

    public void VerificaPosicaoBotaoSemaforo(bool condicao)
    {
        estaFrenteBotao = condicao;
    }

    public void VerificaPosicaoFaixa(bool condicao)
    {
        estaFrenteFaixa = condicao;
    }

    public void MostrarAlerta(string alerta)
    {
        texto.fontSize = 40;
        switch (alerta)
        {
            case "CaixaBotaoSemaforo" when !estaFrenteBotao:
                texto.text = "Se aproxime e aperte botão para solicitar a travessia";
                mensagemAlerta.SetActive(true);
                break;
            case "Botao" when estaFrenteBotao:
                textoBotaoSemaforo.SetActive(true);
                podeAcionarBotao = true;
                break;
            case "Faixa":
                alertaVisivel = true;
                texto.text = "Nunca atravesse a faixa sem olhar para os dois lados!";
                mostrouAlertaOlharDoisLados = true;
                mensagemAlerta.SetActive(true);
                break;
            case "Capacete":
                texto.text = "Sempre coloque o capacete ao andar de patinete!";
                mensagemAlerta.SetActive(true);
                break;
            case "MovimentarPersonagem":
                texto.text = "Utilize o analógico para mover o personagem!";
                AlterarSpriteDoControle(controleAnalogico);
                mensagemAlerta.SetActive(true);
                break;
            case "ColocarCapacete" when pt.subiuPatinete:
                texto.text = "Aperte B para colocar o capacete!";
                AlterarSpriteDoControle(controleB);
                mensagemAlerta.SetActive(true);
                break;
            case "FaixaSemaforo":
                alertaVisivel = true;
                texto.text = "Nunca atravesse a faixa se o sinal não estiver verde!";
                mensagemAlerta.SetActive(true);
                break;
            case "Patinete":
                texto.text = "Aperte B para subir no patinete!";
                AlterarSpriteDoControle(controleB);
                mensagemAlerta.SetActive(true);
                break;
            case "AcelerarPatinete":
                texto.text = "Aperte o gatilho para acelerar e freiar o patinete!";
                AlterarSpriteDoControle(controleGSuperior);
                mensagemAlerta.SetActive(true);
                break;
            case "CarroGaragem" when !mostrouAlerta:
                texto.text = "Cuidado com veículos que podem cruzar o caminho!";
                mensagemAlerta.SetActive(true);
                break;
            case "VelocidadeCalcada":
                texto.text = "Na calçada não passe de 6KM/H!";
                mensagemAlerta.SetActive(true);
                break;
            case "VelocidadeCiclovia":
                texto.text = "Na ciclovia não passe de 20KM/H!";
                mensagemAlerta.SetActive(true);
                break;
            case "Volume":
                AlterarSpriteDoControle(controleD);
                texto.fontSize = 34;
                texto.text = "Não é seguro andar de patinete com fones de ouvido! Aperte D para desligar a música";
                mensagemAlerta.SetActive(true);
                break;
            case "PedestreAcidente":
                texto.fontSize = 33;
                texto.text = "Vá até o local e Peça para que as pessoas se posicionem em um local seguro!";
                mensagemAlerta.SetActive(true);
                break;
            case "SinalizaAcidente":
                texto.fontSize = 34;
                texto.text = "Sinalize antes do local, por exemplo com um galho da árvore!";
                personagem.GetComponent<Personagem>().DesativarControle();
                mensagemAlerta.SetActive(true);
                Invoke("OcultarAlertaSinalizacao", 3.0f);              
                break;
            case "SinalizouAcidente":
                texto.fontSize = 33;
                texto.text = "Isso faz com que outros motoristas diminuam a velocidade e evita outros acidentes!";
                personagem.GetComponent<Personagem>().DesativarControle();
                mensagemAlerta.SetActive(true);
                alertaParametro = "RetornarCalcada";
                Invoke("MostrarOutroAlerta", 3.0f);
                break;
            case "RetornarCalcada":
                texto.text = "Retorne a um local seguro da via!";
                mensagemAlerta.SetActive(true);
                break;
            case "LigaEmergencia":
                texto.fontSize = 32;
                texto.text = "Ligue para a emergência e informe o acidente! Aperte 'D' para selecionar o telefone";
                controle.SetActive(true);
                AlterarSpriteDoControle(controleD);
                mensagemAlerta.SetActive(true);
                break;
            case "Esquerda":
            case "Direita":
                ExibirAlertaDirecao(alerta);
                break;
            default:
                break;
        }
    }

    private void MostrarOutroAlerta()
    {
        MostrarAlerta(alertaParametro);
        personagem.GetComponent<Personagem>().AtivarControle();
    }

    private void OcultarAlertaSinalizacao()
    {
        personagem.GetComponent<Personagem>().AtivarControle();
        OcultarAlerta("SinalizaAcidente");
    }

    public void AlterarSpriteDoControle(Sprite novaSprite)
    {
        if (controleSpriteRenderer != null)
        {
            controleSpriteRenderer.sprite = novaSprite;
            controle.SetActive(true);
        }
    }

    private void ExibirAlertaDirecao(string direcao)
    {
        GameObject alertaObjeto = direcao == "Esquerda" ? esquerda : direita;

        if (estaFrenteFaixa)
        {
            alertaObjeto.SetActive(true);
        }
        else
        {
            alertaObjeto.SetActive(false);
        }
    }

    private void EsconderAlertaDirecao(string direcao)
    {
        GameObject alertaObjeto = direcao == "LadoEsquerdo" ? esquerda : direita;
        alertaObjeto.SetActive(false);
    }

    public void OcultarAlerta(string alerta)
    {
        switch (alerta)
        {
            case "LadoEsquerdo":
            case "LadoDireito":
                EsconderAlertaDirecao(alerta);
                break;
            case "FaixaSemaforo":
            case "Faixa":
                if (alertaVisivel)
                {
                    alertaVisivel = false;
                    mensagemAlerta.SetActive(false);
                }
                break;
            case "Botao":
                textoBotaoSemaforo.SetActive(false);
                podeAcionarBotao = false;
                break;
            case "ColocarCapacete":
                mensagemAlerta.SetActive(false);
                MostrarAlerta("Volume");
                if (controle != null)
                {
                    controle.SetActive(false);
                }
                break;
            case "MovimentarPersonagem":
                mensagemAlerta.SetActive(false);
                personagem.GetComponent<Personagem>().AtivarControle();
                if (controle != null)
                {
                    controle.SetActive(false);
                }
                break;
            case "CaixaBotaoSemaforo":
            case "Capacete":
            case "Patinete":
            case "AcelerarPatinete":
            case "CarroGaragem":
            case "PedestreAcidente":
            case "VelocidadeCalcada":
            case "SinalizaAcidente":
            case "VelocidadeCiclovia":
            case "LigaEmergencia":
            case "Volume":
            case "SinalizouAcidente":
            case "RetornarCalcada":
                mensagemAlerta.SetActive(false);
                if (controle != null)
                {
                    controle.SetActive(false);
                }
                break;
            default:
                break;
        }
        

    }

    public void MostrouAlerta()
    {
        mostrouAlerta = true;
    }

  

}

