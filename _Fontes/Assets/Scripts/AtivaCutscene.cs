using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SpatialTracking;

public class AtivaCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutscene;
    [SerializeField] private GameObject canvas;
    [SerializeField] private UnityEvent OnPlay;
    [SerializeField] private UnityEvent OnStop;

    private bool jaRodou;

    // Update is called once per frame

    private void Start()
    {
       IniciaCutscene();
    }
    void Update()
    {

    }

    public void IniciaCutscene()
    {
        if (jaRodou)
        {
            return;
        }
        jaRodou = true;
        OnPlay.Invoke();
        cutscene.Play();
        Invoke("FinalizaCutscene", (float)cutscene.duration);

    }

    void FinalizaCutscene()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        // Ativando giro da camera no HMD
        TrackedPoseDriver trackedPoseDriver = mainCamera.GetComponentInChildren<TrackedPoseDriver>();
        trackedPoseDriver.enabled = true;
        OnStop.Invoke();
    }


}
