using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Personagem : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject faixaPedestre2;
    [SerializeField] private float velocidadeY = -9.81f;
    [SerializeField] private float velocidade;
    [SerializeField] private Transform destino;
    [SerializeField] private PlayableDirector introCutscene;
    [SerializeField] private Transform cameraPrincipal;
    [SerializeField] private NavMeshSurface navMeshPersonagem;
    [SerializeField] private GameObject setaDestino;

    private Vector3 movimentoPersonagem;
    private Vector2 movimentoControle;
    private bool podeMover = true;
    private Alerta alerta;
    private bool movimentouPersonagem = true;
    private Vector3 posicaoAnterior;
    private bool navMeshConstruido = false;
    private Animator animator;
    private GameObject container;
    public bool olhouEsquerda;
    public bool olhouDireita;
    private float horInput;
    private float verInput;
    private bool abaixouBraco;
    private bool canvasAtivo;
    private bool ativouGalho;
    private string cenaAtiva;
    private bool mostrouAlertaInicialMovimento;

    private void Start()
    {

        container = GameObject.Find("Container");
        alerta = container.GetComponent<Alerta>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        posicaoAnterior = navMeshAgent.transform.position;
        animator = GetComponent<Animator>();
        cenaAtiva = SceneManager.GetActiveScene().name;
        //Invoke("AtivarControle", (float)introCutscene.duration);
    }


    // Desativado pelo gameobject container
    public void DesativarControle()
    {
        Debug.Log("Estou desativando o controle");
        movimentoControle = Vector3.zero;
        podeMover = false;
    }

    // Ativado pelo gameobject container
    public void AtivarControle()
    {
        Debug.Log("Estou ativando o controle");
        podeMover = true;
    }

    public void SetMovimento(InputAction.CallbackContext value)
    {
        if (!podeMover)
        {
            if (cenaAtiva.Equals("FaixaPedestre") && !mostrouAlertaInicialMovimento)
            {
                alerta.OcultarAlerta("MovimentarPersonagem");
                mostrouAlertaInicialMovimento = true;
            }
            return;
        }

        if (value.started)
        {
            movimentouPersonagem = true;
        }

        else if (value.canceled)
        {
            movimentouPersonagem = false;
        }

        movimentoControle = value.ReadValue<Vector2>();

    }

    private void Update()
    {
        if (podeMover)
        {
            if (SceneManager.GetActiveScene().name.Equals("FaixaPedestre"))
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(navMeshAgent.transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    int indiceAreaBotaoSemaforo = NavMesh.GetAreaFromName("AtivaBotaoSemaforo");
                    int indiceAreaFaixa = NavMesh.GetAreaFromName("TriggerFaixa");

                    bool estaSobreBotaoSemaforo = (hit.mask & (1 << indiceAreaBotaoSemaforo)) != 0;
                    bool estaSobreFaixa = (hit.mask & (1 << indiceAreaFaixa)) != 0;

                    alerta.VerificaPosicaoBotaoSemaforo(estaSobreBotaoSemaforo);
                    alerta.VerificaPosicaoFaixa(estaSobreFaixa);
                }

                if (olhouEsquerda && olhouDireita && !navMeshConstruido)
                {
                    faixaPedestre2.GetComponent<NavMeshModifier>().enabled = false;
                    navMeshPersonagem.UpdateNavMesh(navMeshPersonagem.navMeshData);
                    navMeshConstruido = true;
                }
                VerificarCenarioRaycast();
            }
            else if (SceneManager.GetActiveScene().name.Equals("Acidente"))
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(navMeshAgent.transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    int indiceAreaGalhos = NavMesh.GetAreaFromName("TriggerGalhos");
                    int indiceAreaArvore = NavMesh.GetAreaFromName("TriggerArvore");

                    bool estaSobreGalhos = (hit.mask & (1 << indiceAreaGalhos)) != 0;
                    bool estaAreaArvore = (hit.mask & (1 << indiceAreaArvore)) != 0;

                    Acidente acidente = container.GetComponent<Acidente>();

                    if (estaSobreGalhos && !abaixouBraco)
                    {
                        AbaixarBraco();
                    }

                    if (estaAreaArvore && !canvasAtivo && !ativouGalho)
                    {
                        acidente.canvasArvore.SetActive(true);
                        canvasAtivo = true;
                    }
                    else if(!estaAreaArvore && canvasAtivo)
                    {
                        acidente.canvasArvore.SetActive(false);
                        canvasAtivo = false;
                    }

                }
            }

            horInput = movimentoControle.x;
            verInput = movimentoControle.y;

            Vector3 camFrente = cameraPrincipal.forward;
            Vector3 camDireita = cameraPrincipal.right;

            Quaternion cameraRotacao = cameraPrincipal.rotation;

            // bloqueia rotacao no eixo x
            Quaternion novaRotacao = Quaternion.Euler(0f, cameraRotacao.eulerAngles.y, cameraRotacao.eulerAngles.z);

            // define a rotacao do personagem para a rotacao da camera
            this.transform.rotation = novaRotacao;

            camFrente.y = 0;
            camDireita.y = 0;

            Vector3 camFrenteRelativa = verInput * camFrente.normalized;
            Vector3 camDireitaRelativa = horInput * camDireita.normalized;

            float multiplicaMovimentacaoLateral = 2.0f;
            camDireitaRelativa *= multiplicaMovimentacaoLateral;

            Vector3 moveDir = (camFrenteRelativa + camDireitaRelativa);

            movimentoPersonagem = new Vector3(moveDir.x, velocidadeY, moveDir.z);

            navMeshAgent.Move(movimentoPersonagem * Time.deltaTime * velocidade);

            if (navMeshAgent.transform.position != posicaoAnterior && !movimentouPersonagem)
            {
                // Debug.Log("Teletransportou");
                posicaoAnterior = navMeshAgent.transform.position;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Colidiu");
    }

    private void VerificarCenarioRaycast()
    {
        Vector3 direcaoRayCast = (transform.forward - 1.5f * transform.up).normalized;

        float distanciaRayCast = 3.0f;
        RaycastHit colisao;
        Vector3 distancia = transform.forward * 0.5f; // ajusta distancia a frente do personagem
        Vector3 posicaoInicial = transform.position + distancia;

        //  Debug.DrawRay(posicaoInicial, direcaoRayCast * distanciaRayCast, Color.red);

        if (Physics.Raycast(posicaoInicial, direcaoRayCast, out colisao, distanciaRayCast))
        {
            if (colisao.collider.gameObject.name.Equals("faixa_pedestre") && colisao.collider.gameObject.GetComponent<NavMeshModifier>().enabled && verInput > 0f)
            {
                alerta.MostrarAlerta("FaixaSemaforo");
            }
            else if (colisao.collider.gameObject.name.Equals("faixa_pedestre2") && colisao.collider.gameObject.GetComponent<NavMeshModifier>().enabled && verInput > 0f)
            {
                alerta.MostrarAlerta("Faixa");
            }
            else
            {
                alerta.OcultarAlerta("FaixaSemaforo");
                alerta.OcultarAlerta("Faixa");
            }
        }
    }


    public void LevantarBraco()
    {
        if (animator != null)
        {
            ativouGalho = true;
            animator.SetBool("PodeLevantarBraco", true);
        }
    }

    public void AbaixarBraco()
    {
        abaixouBraco = true;
        if (animator != null)
        {
            animator.SetBool("PodeAbaixarBraco", true);
        }
    }

    public void AtivarGalhos()
    {
        // setaDestino.SetActive(false);
        animator.SetBool("PodeLevantarBraco", false);
        container.GetComponent<Acidente>().galhos.SetActive(true);
        alerta.MostrarAlerta("SinalizouAcidente");
    }
}
