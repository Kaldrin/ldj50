using System;
using System.Collections;
using System.Collections.Generic;
using JGDT.Audio.FadeInOut;
using UnityEngine;

public class LighterTrigger : MonoBehaviour
{
    [SerializeField] private Meche ropeToLight = null;
    [SerializeField] private Transform lightingArea = null;
    [SerializeField] private GameObject firePrefab = null;
    [SerializeField] private AudioSource lightSFX = null;

    [SerializeField] private AudioFade ambianceAudioFade = null;
    [SerializeField] private AudioFade music = null;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            LightRopeAtArea();
    }

    private void Start() {
        firePrefab = GameObject.FindObjectOfType<Flame>().gameObject;
        music = GameObject.Find("Music").GetComponent<AudioFade>();
        ambianceAudioFade = GameObject.Find("Ambiance").GetComponent<AudioFade>();
    }

    void LightRopeAtArea()
    {
        Meche[] ropes = GameObject.FindObjectsOfType<Meche>();
        foreach(Meche rope in ropes)
        {
            CheckForAdjacentSection(rope);
            rope.burning = true;  
        }
        if (ambianceAudioFade)
            ambianceAudioFade.FadeOut();
        Invoke("TriggerMusic", 1.2f);
        if (lightSFX)
            lightSFX.Play();
        Character.instance.faceAnimator.SetTrigger("Chocked");
        Destroy(GetComponent<Collider2D>());
    }

    void TriggerMusic()
    {
        if (music)
            music.audioSource.Play();
    }
    
    
    void CheckForAdjacentSection(Meche rope)
    {
        for (int i = 0; i < rope.lineRenderer.positionCount - 1; i++)
        {
            Vector3 position = rope.lineRenderer.GetPosition(i);
            if (position.z <= 0.2f && Vector3.Distance(position, lightingArea.position) < 1f)
            {
                SpreadFire(i, rope);
                return;
            }
        }
    }
    void SpreadFire(int index, Meche rope)
    {
        Flame newFlame = Instantiate(firePrefab).GetComponent<Flame>();
        newFlame.standing = false;
        newFlame.lineRendererToFollow = rope.lineRenderer;
        newFlame.RestartMovingFromBeginning(index);
    }
}
