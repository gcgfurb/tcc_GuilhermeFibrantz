using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Acidente : MonoBehaviour
{
    [SerializeField] private Animation animacaoAcidente;
    [SerializeField] private GameObject personagem;
    [SerializeField] private GameObject celular;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject ambulancia;
    [SerializeField] private Animation animacaoTelaAmbulancia;
    [SerializeField] public GameObject galhos;
    [SerializeField] private GameObject imagemAnimacaoAmbulancia;
    [SerializeField] private AudioSource audioCelular;

    public GameObject canvasArvore;
    private float duracaoAudio = 9.0f;
    private bool animacaoTelaAmbulanciaIniciou = false;
    private Alerta alerta;
    private Animator animator;
    private bool fezLigacao = false;

    private void Start()
    {
        StartCoroutine(EsperaAnimacaoTerminar());
        GameObject container = GameObject.Find("Container");
        alerta = container.GetComponent<Alerta>();
        animator = personagem.GetComponent<Animator>();
    }

    private IEnumerator EsperaAnimacaoTerminar()
    {
        animacaoAcidente.Play();

        while (animacaoAcidente.isPlaying)
        {
            yield return null;
        }

        personagem.SetActive(true);
        alerta.MostrarAlerta("PedestreAcidente");

    }

    private void Update()
    {
        if (audioCelular.isPlaying)
        {
            if (audioCelular.time >= duracaoAudio)
            {
                // audio chegou ao fim
                imagemAnimacaoAmbulancia.SetActive(true);
                animacaoTelaAmbulancia.Play();
                animacaoTelaAmbulanciaIniciou = true;
                celular.SetActive(false);
            }
        }
        if (!animacaoTelaAmbulancia.isPlaying && animacaoTelaAmbulanciaIniciou)
        {
            ambulancia.SetActive(true);
        }
    }

    public void ApontarCameraParaCelular()
    {
        Vector3 direcao = celular.transform.position;
        // rotaciona camera para olhar na direcao do capacete.
        cam.rotation = Quaternion.LookRotation(direcao);
    }

    public void SelecionarTelefone()
    {
        if (animator != null)
        {
            animator.SetBool("selecionouTelefone", true);
        }
    }

    public void Telefonar()
    {
        if (animator != null && !fezLigacao)
        {
            fezLigacao = true;
            animator.SetBool("podeTelefonar", true);
            animator.SetBool("selecionouTelefone", false);
        }
    }
}