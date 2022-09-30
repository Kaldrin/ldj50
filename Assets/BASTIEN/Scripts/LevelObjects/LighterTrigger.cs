using System;
using System.Collections;
using System.Collections.Generic;
using JGDT.Audio.FadeInOut;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;



/// <summary>
/// Object that can light on fire a wick when the player passes through it
/// </summary>
public class LighterTrigger : MonoBehaviour
{
    // It should check for all the ropes at the area
    //[SerializeField] private Meche ropeToLight = null;
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    [SerializeField] private Transform lightingArea = null;
    [SerializeField] private GameObject firePrefab = null;
    [FormerlySerializedAs("shouldSetAllWickOnFire")] [SerializeField] bool shouldSetAllWicksToBurning = false;



    [Header("AUDIO")]
    [SerializeField] private AudioSource lightSFX = null;
    [SerializeField] private AudioFade ambianceAudioFade = null;
    [SerializeField] private AudioFade music = null;
    



    private void Start() {
        ambianceAudioFade = GameObject.Find("Ambiance").GetComponent<AudioFade>();
        music = GameObject.Find("Music").GetComponent<AudioFade>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            LightRopeAtArea();
    }


    /// <summary>
    /// Triggers the igniting process
    /// </summary>
    void LightRopeAtArea()
    {
        CheckForAdjacentSection();
        //ropeToLight.burning = true;
        

        // MUSIC
        if (ambianceAudioFade)
            ambianceAudioFade.FadeOut();
        Invoke("TriggerMusic", 1.2f);


        // FX
        if (lightSFX)
            lightSFX.Play();

        
        Destroy(GetComponent<Collider2D>());
    }

    /// <summary>
    /// Checks for all the wicks if there is a section on each of them close enough to the igniting point
    /// </summary>
    void CheckForAdjacentSection()
    {
        foreach (LineRenderer wick in FindObjectsOfType<LineRenderer>())
        {
            bool foundACloseSection = false;
            for (int i = 0; i < wick.positionCount - 1; i++)
            {
                Vector3 position = wick.GetPosition(i);
                if (position.z <= 0.2f && Vector3.Distance(position, lightingArea.position) < 1f)
                {
                    SpreadFire(i, wick);
                    foundACloseSection = true;
                }
                    
                if (foundACloseSection)
                    break;
            }
            // If it couldn't find a close section for this wick but it's set to set them all to burning, it will change their state to burning
            // The burning state of a wick means that if the wick becomes long enough it will start burning on its own
            // We use it here so that if only on player goes through the ignition trigger it still starts the game for all of them
            if (!foundACloseSection && shouldSetAllWicksToBurning)
                wick.GetComponent<Meche>().burning = true;
        }
    }
    /// <summary>
    /// Starts a fire on the wick
    /// </summary>
    /// <param name="index"></param>
    /// <param name="lineToFollow"></param>
    void SpreadFire(int index, LineRenderer lineToFollow)
    {
        // Cache reference
        Meche wick = lineToFollow.GetComponent<Meche>();

        // CHANGE STATE
        wick.burning = true;

        // SPAWN FLAME
        Flame newFlame = Instantiate(firePrefab).GetComponent<Flame>();
        newFlame.standing = false;
        newFlame.lineRendererToFollow = lineToFollow;
        newFlame.transform.position = lightingArea.position;
        newFlame.RestartMovingFromBeginning(index);
        SceneManager.MoveGameObjectToScene(newFlame.gameObject, SceneManager.GetSceneByBuildIndex(0));

        // ANIMATION
        wick.followedPlayer.faceAnimator.SetTrigger("Chocked");
    }






    void TriggerMusic()
    {
        if (music)
            music.audioSource.Play();
    }
}
