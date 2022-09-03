using System;
using System.Collections;
using System.Collections.Generic;
using JGDT.Audio.FadeInOut;
using UnityEngine;



/// <summary>
/// Flame that follows a LineRenderer. Doesn't even care about the Wick script
/// </summary>
public class Flame : MonoBehaviour
{
    public LineRenderer lineRendererToFollow = null;
    [SerializeField] private GameObject flamePrefab = null;



    [Header("MOVEMENT")]
    [SerializeField] private float speed = 0.002f;
    private float currentSectionLength = 0f;
    private float currentSectionRanDistance = 0f;
    private Vector3 currentPointPos = new Vector3(0f, 0f, 0f);
    private Vector3 nextpoinsPos = new Vector3(0f, 0f, 0f);
    public bool moving = false;
    [HideInInspector] bool direction = true;



    [Header("PROPAGATION")]
    [SerializeField] private float distanceToSpread = 0f;
    [Tooltip("The flame duplicates where near other segments of a line renderer, but it shouldn't duplicate with the segments right in front or behind it")]
    [SerializeField] private int indexNonSpreadRange = 5;
    private bool canPropagate = false;
    [SerializeField] private float propagationCooldown = 2f;
    private float propagationStartTime = 0f;
    [HideInInspector] public bool standing = true;

    LineRenderer[] wicksList;
    // Won't refresh the list of wicks in game every section (FindObject)
    // but to ensure that it works well depending on how & when we spawn a new player,
    // it's good to refresh it every X sections ran through by the flame
    // I could have made a system with a central WichManager but this way ensures that it's more fool proof.
    // If performances are too impacted we can switch to a WickManager system keeping tracks of LineRenderers instantiation instead of using a FindObject
    int numberOfectionsBeforeRefreshWicksList = 10;
    int sectionBeforeWicksListRefreshCounter = 0;




    [Header("FX")]
    [SerializeField] private List<ParticleSystem> effects = new List<ParticleSystem>();
    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();


    [Header("COLLIDERS")]
    [SerializeField] private List<Collider2D> colliders = new List<Collider2D>();



    [Header("READ ONLY")]
    [SerializeField] private int currentSectionIndex = 0;









    public void StartMoving() => RestartMovingFromBeginning(0);
    private void Update() => UpdateMoving();


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "water")
            Die();


        if (!moving && col.CompareTag("Player"))
        {
            standing = false;
            // With more than one player it can have more than one wick to propagate to,
            // better to make the system wick agnostic, just check all the line renderers of the scene
            lineRendererToFollow.GetComponent<Meche>().burning = true;
            RestartMovingFromBeginning(lineRendererToFollow.positionCount - 2);
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        else if (moving && col.CompareTag("Player"))
        {
            col.transform.parent.GetComponent<Character>().Die();
            Die();
        }
        // OIL
        else if (moving && col.GetComponent<Oil>() && !col.GetComponent<Oil>().onFire)
            col.GetComponent<Oil>().SetOnFire(true);
        // TORCH
        else if (moving && col.transform.parent.GetComponent<Torch>() && !col.transform.parent.GetComponent<Torch>().onFire)
            col.transform.parent.GetComponent<Torch>().SetOnFire(true);
    }


    #region START & STOP
    public void SetMoving(bool state) => moving = state;
    public void RestartMovingFromBeginning(int index)
    {
        if (lineRendererToFollow)
        {
            canPropagate = false;
            propagationStartTime = Time.time;
            currentSectionIndex = index;
            NewSection(index, 0);
            SetMoving(true);
        }
    }
    #endregion






    #region MOVING
    void NewSection(int newSectionIndex, float startValue)
    {
        if (lineRendererToFollow.positionCount > currentSectionIndex && currentSectionIndex >= 0)
            currentPointPos = lineRendererToFollow.GetPosition(currentSectionIndex);

        // Burn the rope
        int previousSectionDirection = direction ? - 1 : 1;
        if (lineRendererToFollow.positionCount > currentSectionIndex + previousSectionDirection && currentSectionIndex + previousSectionDirection >= 0)
        {
            Vector3 burnPos = lineRendererToFollow.GetPosition(currentSectionIndex + previousSectionDirection);
            burnPos.z = 1;
            lineRendererToFollow.SetPosition(currentSectionIndex + previousSectionDirection, burnPos);
        }




        int nextSectionDirection = direction ? 1 : -1;
        if (lineRendererToFollow.positionCount > currentSectionIndex + nextSectionDirection && currentSectionIndex + nextSectionDirection >= 0)
            nextpoinsPos = lineRendererToFollow.GetPosition(currentSectionIndex + nextSectionDirection);
        // If Z pos not 0, means it has burnt already, can't burn anymore
        if (nextpoinsPos.z > 0.2f)
        {
            // Burn all the rope around
            Vector3 burnPos = lineRendererToFollow.GetPosition(currentSectionIndex);
            burnPos.z = 1;
            lineRendererToFollow.SetPosition(currentSectionIndex, burnPos);
            burnPos = lineRendererToFollow.GetPosition(currentSectionIndex + nextSectionDirection);
            burnPos.z = 1;
            lineRendererToFollow.SetPosition(currentSectionIndex + nextSectionDirection, burnPos);

            // Then die
            Die();
        }


        currentSectionLength = CalculateCurrentSectionLength();
        currentSectionRanDistance = startValue;


        // PROPAGATE
        if (canPropagate && moving)
            CheckForAdjacentSection();
    }

    float CalculateCurrentSectionLength()
    {
        float distance = 0f;
        if (lineRendererToFollow)
            distance = Vector3.Distance(currentPointPos, nextpoinsPos);

        return distance;
    }
    void UpdateMoving()
    {
        if (moving)
        {
            if (Time.time > propagationStartTime + propagationCooldown)
                canPropagate = true;


            currentSectionRanDistance += speed * Time.deltaTime;
            // If reached the end of the section
            if (currentSectionRanDistance >= currentSectionLength)
            {
                currentSectionIndex = direction ? currentSectionIndex + 1 : currentSectionIndex - 1;

                // End of the LineRenderer
                if ((currentSectionIndex >= lineRendererToFollow.positionCount - 1) || (currentSectionIndex == 0))
                    ReachedEnd();
                else
                    NewSection(currentSectionIndex, currentSectionRanDistance - currentSectionLength);
            }

            SetPos();
        }
    }

    void SetPos()
    {
        float currentSectionProgressNormalized = currentSectionRanDistance / currentSectionLength;
        Vector3 nextSectionAddedVector = nextpoinsPos - currentPointPos;
        nextSectionAddedVector *= currentSectionProgressNormalized;
        Vector3 newPosition = currentPointPos + nextSectionAddedVector;
        newPosition.z = -0.5f;
        newPosition = Vector3.Lerp(transform.position, newPosition, 0.08f);
        if (moving)
            if (newPosition.x != float.NaN && newPosition.y != float.NaN)
            {
                transform.position = newPosition;
            }
            

    }
    #endregion







    #region PROPAGATE
    /// <summary>
    /// Triggered once per section of the line renderer it's following the flame runs through
    /// </summary>
    void CheckForAdjacentSection()
    {
        // REFRESH
        // Check if should refresh the list of LineRenderers the flame can propagate to
        sectionBeforeWicksListRefreshCounter--;
        if (sectionBeforeWicksListRefreshCounter <= 0)
            RefreshWicksList();

        // CHECK FOR PROPAGATION
        // With more than one player it can have more than one wick to propagate to,
        // better to make the system wick agnostic, just check all the line renderers of the scene
        for (int y = 0; y < wicksList.Length; y++)
            for (int i = 0; i < wicksList[y].positionCount - 1; i++)
            {
                int indexDifference = Mathf.Abs(currentSectionIndex - i);
                Vector3 position = wicksList[y].GetPosition(i);

                if (Vector3.Distance(position, transform.position) < distanceToSpread && canPropagate && position.z < 0.19 && indexDifference > indexNonSpreadRange)
                {
                    SpreadFire(i, wicksList[y]);
                    canPropagate = false;
                }
            }
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
    }
    void SpreadFire(int index, LineRenderer newFlameLineRendererToFollow)
    {
        if (canPropagate)
        {
            propagationStartTime = Time.time;

            // FLAME 1
            GameObject loadedFlame = Resources.Load("Flame") as GameObject;
            Flame newFlame = GameObject.Instantiate(loadedFlame).GetComponent<Flame>();

            newFlame.transform.position = currentPointPos;
            newFlame.standing = false;
            newFlame.lineRendererToFollow = newFlameLineRendererToFollow;
            newFlame.flamePrefab = flamePrefab;
            newFlame.RestartMovingFromBeginning(index);
            newFlame.direction = direction;

            // FLAME 2
            newFlame = GameObject.Instantiate(loadedFlame).GetComponent<Flame>();

            newFlame.transform.position = currentPointPos;
            newFlame.standing = false;
            newFlame.lineRendererToFollow = newFlameLineRendererToFollow;
            newFlame.flamePrefab = flamePrefab;
            newFlame.RestartMovingFromBeginning(index);
            newFlame.direction = !direction;
        }
    }
    /// <summary>
    /// Refresh the list of LineRenderers the flame can propagate to, so that it can work in multiplayer with multiple wicks
    /// </summary>
    void RefreshWicksList()
    {
        sectionBeforeWicksListRefreshCounter = numberOfectionsBeforeRefreshWicksList;
        wicksList = FindObjectsOfType<LineRenderer>();
    }
    #endregion





    /// <summary>
    /// Triggered when the flame reaches the end of the LineRenderer it's following
    /// </summary>
    void ReachedEnd()
    {
        // If we have more than one player we can't use instances
        //Character.instance.Die();
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (lineRendererToFollow.GetComponent<Meche>().followedPlayer)
            lineRendererToFollow.GetComponent<Meche>().followedPlayer.Die();


        Die();
    }




    #region FLAME DEATH
    public void Die()
    {
        DisableEffects();
        moving = false;
        StartCoroutine(FadeSprites());
        DestroyColliders();

        // Wait for graphics to fade out before fully destroying the fire
        Destroy(gameObject, 5f);
    }

    /// <summary>
    /// Stops the particle systems of the fire, before disappearing
    /// </summary>
    void DisableEffects()
    {
        if (effects != null && effects.Count > 0)
            for (int i = 0; i < effects.Count; i++)
                if (effects[i])
                    effects[i].Stop();
    }

    /// <summary>
    /// Fades out the alpha of the sprites graphics of the fire, if it has any (Unused for now)
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeSprites()
    {
        if (sprites != null && sprites.Count > 0)
        {
            bool notFaded = true;

            while (notFaded)
            {
                notFaded = false;

                for (int i = 0; i < sprites.Count; i++)
                    if (sprites[i] && sprites[i].color.a > 0)
                    {
                        sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, sprites[i].color.a - 0.1f);
                        notFaded = true;
                    }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    /// <summary>
    /// Destroys the colliders of the fire before destroying itself so the FX has time to fadeout
    /// </summary>
    void DestroyColliders()
    {
        if (colliders != null && colliders.Count > 0)
            for (int i = 0; i < colliders.Count; i++)
                if (colliders[i])
                    Destroy(colliders[i]);
    }
    #endregion
}
