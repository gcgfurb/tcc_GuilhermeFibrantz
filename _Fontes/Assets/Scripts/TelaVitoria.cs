using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TelaVitoria : MonoBehaviour
{
    [SerializeField] private Animation animacaoTelaVitoria;

    private void Update()
    {
        if (!animacaoTelaVitoria.isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                string cenaAtual = SceneManager.GetActiveScene().name;
                int faseDesbloquear = 0;

                switch (cenaAtual)
                {
                    case "FaixaPedestre":
                        faseDesbloquear = 1;
                        break;
                    case "Patinete":
                        faseDesbloquear = 2;
                        break;
                }

                if (faseDesbloquear > 0)
                {
                    GerenciadorFases.DesbloquearFase(faseDesbloquear);
                }

                SceneManager.LoadScene("SelecaoFases");
            }
        }
    }
}
