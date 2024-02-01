using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ControlaJogadorMouse : MonoBehaviour
{
    [SerializeField] private int botaoMouse;
    private NavMeshAgent agente;
    private Camera cameraMain;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cameraMain = Camera.main;
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("andando", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(botaoMouse))
        {
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {   
                agente.SetDestination(hit.point);
            }
        }
    }
}
