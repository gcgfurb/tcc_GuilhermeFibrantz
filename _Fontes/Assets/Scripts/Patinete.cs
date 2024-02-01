using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Patinete : MonoBehaviour
{
    [SerializeField] private GameObject patinete;
    [SerializeField] private TextMeshProUGUI velocidadeText;
    [SerializeField] private Rigidbody patineteRigidbody;
    [SerializeField] private Transform cam;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private AudioSource audioMusica;
    [SerializeField] private GameObject capacete;
    [SerializeField] private GameObject cabeca;
    [SerializeField] private GameObject personagem;
    [SerializeField] private GameObject indicadorAudio;
    [SerializeField] private CinemachineVirtualCamera cvc;
    [SerializeField] private GameObject flechaApontar;
    [SerializeField] private GameObject personagemPatinete;
    [SerializeField] private GameObject posicaoCamera;
    [SerializeField] private GameObject capacetePersonagem;
    [SerializeField] private Sprite[] spritesIndicadorAudio;
    [SerializeField] private Image imagemIndicadorAudio;

    private Animator animator;
    private Alerta alerta;
    private bool podeMover = true;
    private Vector2 movimento;
    private float velocidadeAtual = 0f;
    private float distanciaRaio = 2.0f;
    private bool colocouCapacete = false;
    public bool olhouCapacete = false;
    private bool movendoLateralmente = false;
    public bool subiuPatinete = false;
    private bool estaFrentePatinete = false;
    private bool desligouAudio = false;

    private void Start()
    {
        olhouCapacete = false;
        AtualizarIndicadorAudio(0);
        animator = GetComponent<Animator>();
        GameObject container = GameObject.Find("Container");
        alerta = container.GetComponent<Alerta>();
        if (patineteRigidbody == null)
        {
            patineteRigidbody = GetComponent<Rigidbody>();
        }
    }

    public void DesativarControle()
    {
        Debug.Log("Estou desativando o controle");
        movimento = Vector3.zero;
        podeMover = false;
    }

    public void SetMovimento(InputAction.CallbackContext value)
    {
        if (!podeMover)
        {
            return;
        }

        if (subiuPatinete && !podeMover)
        {
            return;
        }

        movimento = value.ReadValue<Vector2>();
        movendoLateralmente = Mathf.Abs(movimento.x) > 0.1f;
    }

    public void DiminuirAudio(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            DesligarAudio();
        }
    }

    private void AtualizarIndicadorAudio(int indiceSprite)
    {
        imagemIndicadorAudio.sprite = spritesIndicadorAudio[indiceSprite];
    }

    private void DesligarAudio()
    {
        audioMusica.volume = 0f;
        audioMusica.Stop();
        indicadorAudio.GetComponent<Image>().enabled = false;
        indicadorAudio.SetActive(false);
        desligouAudio = true;
        alerta.OcultarAlerta("Volume");
        alerta.MostrarAlerta("AcelerarPatinete");
    }


    public void SubirPatinete(InputAction.CallbackContext value)
    {
        personagem.SetActive(false);
        if (value.started && !subiuPatinete && estaFrentePatinete)
        {
            personagem.SetActive(false);
            Debug.Log("Subiu patinete");
            subiuPatinete = true;
            // ativa o personagem em cima do patinete, desativa o da cena e altera a camera
            personagemPatinete.SetActive(true);
            cvc.Follow = posicaoCamera.transform;

            alerta.OcultarAlerta("Patinete");
            velocidadeText.enabled = true;
            alerta.MostrarAlerta("Capacete");
            // apontar para capacete
            flechaApontar.SetActive(true);
        }
    }

    public void ColocaCapacete(InputAction.CallbackContext value)
    {
        if (value.started && olhouCapacete)
        {
            flechaApontar.SetActive(false);
            capacetePersonagem.SetActive(true);
            capacete.SetActive(false);
            colocouCapacete = true;
            audioMusica.Play();
            audioMusica.volume = 1f;
            indicadorAudio.SetActive(true);

            alerta.OcultarAlerta("ColocarCapacete");
            animator.SetBool("podeLevantarBracoPatinete", true);
            alerta.MostrarAlerta("Volume");

        }

    }
    public void AceleraBotao(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (subiuPatinete)
            {
                if (!colocouCapacete)
                {
                    alerta.MostrarAlerta("Capacete");
                }
                else
                {
                    if (!desligouAudio)
                    {
                        alerta.MostrarAlerta("Volume");
                    }
                    else
                    {
                        patineteRigidbody.isKinematic = false;
                        alerta.OcultarAlerta("AcelerarPatinete");
                        velocidadeAtual += 0.5f;
                    }
                }
            }
        }
    }

    private void ApontarCameraParaCapacete()
    {
        Vector3 direcao = capacete.transform.position - cam.position;
        cam.rotation = Quaternion.LookRotation(direcao);
    }

    public void EstaFrentePatinete(bool condicao)
    {
        estaFrentePatinete = condicao;
    }

    public void DesaceleraBotao(InputAction.CallbackContext value)
    {

        if (value.started && velocidadeAtual > 0)
        {
            if (subiuPatinete)
            {
                velocidadeAtual -= 0.5f;
            }
        }
        else if (velocidadeAtual == 0)
        {
            // parar patinete
            patineteRigidbody.isKinematic = true;
        }
    }

    private void Update()
    {
        if (subiuPatinete)
        {
            Quaternion cameraRotacao = cam.rotation;
            Quaternion novaRotacao = Quaternion.Euler(0f, cameraRotacao.eulerAngles.y, cameraRotacao.eulerAngles.z);

            // aplica rotacao
            cabeca.transform.rotation = novaRotacao;
            posicaoCamera.transform.rotation = novaRotacao;

            if (velocidadeAtual > 0)
            {
                if (velocidadeAtual * 2 < 20)
                {
                    alerta.OcultarAlerta("VelocidadeCiclovia");
                }
                if (movendoLateralmente)
                {
                    float rotacaoY = movimento.x;
                    patineteRigidbody.angularVelocity = Vector3.up * rotacaoY;
                }
                else
                {
                    patineteRigidbody.angularVelocity = Vector3.zero;
                }

                Quaternion rotacaoAtual = patineteRigidbody.rotation;
                Vector3 direcaoDeMovimento = rotacaoAtual * Vector3.forward;

                patineteRigidbody.velocity = direcaoDeMovimento * velocidadeAtual;

                if (movimento.y >= 0)
                {
                    if (velocidadeAtual * 2 > 20)
                    {
                        alerta.MostrarAlerta("VelocidadeCiclovia");
                    }
                    else
                    {
                        alerta.OcultarAlerta("VelocidadeCiclovia");
                    }
                }
                velocidadeText.text = (velocidadeAtual * 2f).ToString("F1");
            }
        }
    }

    private void VerificarCenarioRaycast()
    {
        float horInput = movimento.x;
        float verInput = movimento.y;

        if (horInput == 0 && verInput == 0)
        {
            return;
        }

        Ray ray = new Ray(navMeshAgent.transform.position, Vector3.down);
        RaycastHit raio;
        Debug.DrawRay(ray.origin, ray.direction * distanciaRaio, Color.red);

        if (Physics.Raycast(ray, out raio, distanciaRaio, NavMesh.AllAreas))
        {
            GameObject gameObject = raio.collider.gameObject;

            if (gameObject.name.Equals("Ciclovia"))
            {
                if (velocidadeAtual * 2 > 20)
                {
                    alerta.MostrarAlerta("VelocidadeCiclovia");
                }
                else
                {
                    alerta.OcultarAlerta("VelocidadeCiclovia");
                }
            }
            else if (gameObject.name.Equals("Calcada"))
            {
                if (velocidadeAtual * 2 > 6)
                {
                    alerta.MostrarAlerta("VelocidadeCalcada");
                }
                else
                {
                    alerta.OcultarAlerta("VelocidadeCalcada");
                }
            }
        }
    }
}
