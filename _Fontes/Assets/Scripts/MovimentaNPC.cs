using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovimentaNPC : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool podeAndar = false;
    [SerializeField] private float proximidadePonto = 1.0f;
    private NavMeshAgent agente;
    private int indiceWaypoint;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanciaParaWaypoint = Vector3.Distance(transform.position, waypoints[indiceWaypoint].position);

        if (distanciaParaWaypoint < proximidadePonto)
        {
            if (indiceWaypoint >= waypoints.Length - 1)
            {
                indiceWaypoint = 0;
                if (gameObject.name.Equals("Van") || gameObject.name.Equals("Carro"))
                {
                    agente.enabled = false;
                    gameObject.SetActive(false);
                }
                else if (gameObject.name.Equals("Ambulancia"))
                {
                    // agente.enabled = false;
                }
            }
            else
            {
                indiceWaypoint++;
            }
        }

        if (gameObject.tag.Equals("Carro"))
        {
            if (agente.enabled)
            {
                VerificarColisao();
            }
        }

        if (animator != null)
        {
            // seta valor para animacao de andar
            animator.SetFloat("Speed_f", agente.velocity.magnitude);
        }
        if (agente.enabled && podeAndar)
        {
            agente.SetDestination(waypoints[indiceWaypoint].position);
        }
    }

    public void LiberaMovimento()
    {
        podeAndar = true;
    }

    private void VerificarColisao()
    {
        RaycastHit colisao;

        // a frente do jogador
        Vector3 origemRaycast = transform.position + transform.forward * 6f;
        Vector3 direcaoRayCast = (transform.forward - 2f * transform.up).normalized;

        Debug.DrawRay(origemRaycast, direcaoRayCast, Color.blue);

        if (Physics.Raycast(origemRaycast, direcaoRayCast, out colisao, agente.stoppingDistance))
        {
            NavMeshModifier[] navModifier = colisao.collider.gameObject.GetComponents<NavMeshModifier>();
            // fazer o carro parar
            if (navModifier.Length > 1)
            {
                if (navModifier[1].enabled)
                {
                    agente.isStopped = true;
                }
                else
                {
                    agente.isStopped = false;
                }
            }
        }
    }

}
