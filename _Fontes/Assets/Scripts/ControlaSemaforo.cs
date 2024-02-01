using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ControleSemaforo : MonoBehaviour
{
    [SerializeField] private Material materialVermelho;
    [SerializeField] private Material materialVermelhoBoneco;
    [SerializeField] private Material materialVerde;
    [SerializeField] private Material materialVerdeBoneco;
    [SerializeField] private Material materialPreto;
    [SerializeField] private GameObject sinalVermelho;
    [SerializeField] private GameObject sinalVerde;
    [SerializeField] private NavMeshSurface navMeshPersonagem;
    [SerializeField] private NavMeshSurface navMeshCarro;
    [SerializeField] private GameObject faixaPedestre;

    private NavMeshModifier[] modificadoresFaixa;
    private List<Material> materialsPreto;
    private List<Material> materialsVermelho;
    private List<Material> materialsVerde;
    private float duracaoVerde = 15f;
    private Alerta alerta;

    private void Start()
    {
        GameObject container = GameObject.Find("Container");
        alerta = container.GetComponent<Alerta>();
        modificadoresFaixa = faixaPedestre.GetComponents<NavMeshModifier>();
        InicializarListasDeMateriais();
        SinalVermelho();
    }

    public void AcionaBotaoSemaforo()
    {
        if (alerta.podeAcionarBotao)
        {
            alerta.OcultarAlerta("Botao");
            StartCoroutine(SinalVerde());
        }
    }

    private void SinalVermelho()
    {
        sinalVermelho.GetComponent<MeshRenderer>().SetMaterials(materialsVermelho);
        sinalVerde.GetComponent<MeshRenderer>().SetMaterials(materialsPreto);

        faixaPedestre.GetComponent<NavMeshModifier>().enabled = true;

        // modificador do carro, permite andar
        modificadoresFaixa[1].enabled = false;

        ReconstruirNavmesh();
    }

    private void InicializarListasDeMateriais()
    {
        materialsPreto = new List<Material>();
        materialsVermelho = new List<Material>();
        materialsVerde = new List<Material>();

        materialsPreto.Add(materialPreto);
        materialsVermelho.Add(materialVermelhoBoneco);
        materialsVermelho.Add(materialVermelho);
        materialsVerde.Add(materialVerde);
        materialsVerde.Add(materialVerde);
        materialsVerde.Add(materialVerdeBoneco);
    }

    IEnumerator SinalVerde()
    {
        //espera antes de ficar verde
        yield return new WaitForSeconds(8f);

        sinalVerde.GetComponent<MeshRenderer>().SetMaterials(materialsVerde);
        sinalVermelho.GetComponent<MeshRenderer>().SetMaterials(materialsPreto);

        // modificador do carro, nao pode andar
        modificadoresFaixa[1].enabled = true;
        faixaPedestre.GetComponent<NavMeshModifier>().enabled = false;
        ReconstruirNavmesh();
        //fica verde depois vermelho novamente
        yield return new WaitForSeconds(duracaoVerde);
        SinalVermelho();
    }

    private void ReconstruirNavmesh()
    {
        //navMeshCarro.BuildNavMesh();
        // navMeshPersonagem.BuildNavMesh();
        navMeshPersonagem.UpdateNavMesh(navMeshPersonagem.navMeshData);
        navMeshCarro.UpdateNavMesh(navMeshCarro.navMeshData);
    }

}