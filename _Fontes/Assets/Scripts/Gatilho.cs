using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gatilho : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    [SerializeField] private UnityEvent OnStay;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("NPC") || other.gameObject.tag.Equals("Carro") || other.gameObject.tag.Equals("Patinete"))
        {
            OnEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("NPC") || other.gameObject.tag.Equals("Carro") || other.gameObject.tag.Equals("Patinete"))
        {
            OnExit.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("NPC") || other.gameObject.tag.Equals("Carro") || other.gameObject.tag.Equals("Patinete"))
        {
            OnStay.Invoke();
        }
    }




}
