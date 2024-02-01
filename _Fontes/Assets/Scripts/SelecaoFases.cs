using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SelecaoFases : MonoBehaviour
{
    [SerializeField] private Button[] botoes;
    [SerializeField] private Slider[] sliders;
    [SerializeField] private TextMeshProUGUI mensagemTela;
    [SerializeField] private CarregaFase carregaFase;
    [SerializeField] private GameObject[] setas;

    private int botaoSelecionadoIndice = 0;
    private bool botaoSendoPressionado = false;
    private float tempoPressionado = 0f;
    private float tempoPressionadoNecessario = 5f;
    private float tempoSemAcao = 0;
    private bool bloqueiaBotao;
    private bool carregandoFase = false;

    private void Start()
    {
        for (int i = 0; i < botoes.Length; i++)
        {
            bool faseDesbloqueada = GerenciadorFases.FaseDesbloqueada(i);
            botoes[i].interactable = faseDesbloqueada;
        }
    }

    private void Update()
    {
        // verifica clique na tela
        if (Input.GetMouseButtonDown(0))
        {
            botaoSendoPressionado = true;
            tempoSemAcao = 0; // reseta quando clicar na tela
        }

        if (botaoSendoPressionado && botoes[botaoSelecionadoIndice].interactable)
        {
            mensagemTela.gameObject.SetActive(false);
            // tempo botao pressionado
            tempoPressionado += Time.deltaTime;

            float progresso = tempoPressionado / tempoPressionadoNecessario;
            sliders[botaoSelecionadoIndice].value = progresso;

            // carrega cena
            if (tempoPressionado >= tempoPressionadoNecessario)
            {
                switch (botaoSelecionadoIndice)
                {
                    case 0:
                        StartCoroutine(carregaFase.IniciaFase("FaixaPedestre"));
                        carregandoFase = true;
                        break;
                    case 1:
                        StartCoroutine(carregaFase.IniciaFase("Patinete"));
                        carregandoFase = true;
                        break;
                    case 2:
                        StartCoroutine(carregaFase.IniciaFase("Acidente"));
                        carregandoFase = true;
                        break;
                }
            }
            else
            {
                bloqueiaBotao = false;
            }
        }
        else
        {

            tempoSemAcao += Time.deltaTime;

            // mostra mensagem na tela se o tempo for >= 10 segundos
            if (tempoSemAcao >= 10.0f)
            {
                setas[botaoSelecionadoIndice].SetActive(false);
                mensagemTela.gameObject.SetActive(true);
                mensagemTela.text = "• Toque no botão para mudar a fase selecionada \r\n• Mantenha pressionado para carregar a fase";
            }
        }


        if (!bloqueiaBotao && Input.GetMouseButtonUp(0) && !carregandoFase)
        {
            botaoSendoPressionado = false;
            tempoPressionado = 0f;
            sliders[botaoSelecionadoIndice].value = 0;
            botaoSelecionadoIndice = (botaoSelecionadoIndice + 1) % botoes.Length; // avanca para prox botao
            AlternarSelecaoBotoes();
        }
    }

    private void AlternarSelecaoBotoes()
    {
        for (int i = 0; i < botoes.Length; i++)
        {
            bool isBotaoSelecionado = i == botaoSelecionadoIndice;
            // botoes[i].interactable = isBotaoSelecionado;

            if (setas[i] != null)
            {
                setas[i].SetActive(isBotaoSelecionado);
            }
        }
    }
}