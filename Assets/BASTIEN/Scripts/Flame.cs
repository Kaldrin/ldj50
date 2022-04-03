using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public LineRenderer lineRendererToFollow = null;
    [SerializeField] private float speed = 0.002f;
    [SerializeField] private bool startOnStart = false;
    [SerializeField] private float distanceToSpread = 0f;
    [SerializeField] private GameObject flamePrefab = null;
    [SerializeField] private int indexNonSpreadRange = 5;
    private bool canPropagate = false;
    [SerializeField] private float propagationCooldown = 2f;
    private float propagationStartTime = 0f;
    private bool standing = true;




    private float currentSectionLength = 0f;
    private float currentSectionRanDistance = 0f;
    private Vector3 currentPointPos = new Vector3(0f, 0f, 0f);
    private Vector3 nextpoinsPos = new Vector3(0f, 0f, 0f);


    [Header("FX")]
    [SerializeField] private List<ParticleSystem> effects = new List<ParticleSystem>();
    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();


    [Header("COLLIDERS")][SerializeField] private List<Collider2D> colliders = new List<Collider2D>();



    [Header("READ ONLY")]
    [SerializeField] private bool moving = false;
    [SerializeField] private int currentSectionIndex = 0;





    private void Start()
    {
        /*
        if (startOnStart)
            Invoke("StartMoving", 3f);
            */
    }
    public void StartMoving() => RestartMovingFromBeginning(0);
    private void Update() => UpdateMoving();


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!moving && col.CompareTag("Player"))
        {
            standing = false;
            RestartMovingFromBeginning(lineRendererToFollow.positionCount - 2);
        }
        else if (moving && col.CompareTag("Player"))
        {
            col.transform.parent.GetComponent<Character>().Die();
            col.transform.parent.GetComponent<AudioSource>().Stop();
            TouchPlayer();

        }
    }


    #region START & STOP
    public void SetMoving(bool state) => moving = state;
    public void RestartMovingFromBeginning(int index)
    {
        if (lineRendererToFollow && lineRendererToFollow.positionCount > 5)
        {
            canPropagate = false;
            propagationStartTime = Time.time;
            currentSectionIndex = index;
            //transform.position = lineRendererToFollow.GetPosition(0);
            NewSection(index, 0);
            SetMoving(true);
        }
        //else
        //Invoke("RestartMovingFromBeginning", 1f);
    }
    #endregion






    #region MOVING
    void NewSection(int newSectionIndex, float startValue)
    {
        currentPointPos = lineRendererToFollow.GetPosition(currentSectionIndex);

        // Burn the rope
        Vector3 burnPos = lineRendererToFollow.GetPosition(currentSectionIndex - 1);
        burnPos.z = 1;
        lineRendererToFollow.SetPosition(currentSectionIndex - 1, burnPos);



        nextpoinsPos = lineRendererToFollow.GetPosition(currentSectionIndex + 1);
        // If Z pos not 0, means it has burnt already, can't burn anymore
        if (nextpoinsPos.z > 0.2f)
            Die();


        currentSectionLength = CalculateCurrentSectionLength();
        currentSectionRanDistance = startValue;
        //UpdateGradient();

        if (canPropagate && moving)
            CheckForAdjacentSection();
        //canPropagate = true;
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
                currentSectionIndex++;
                if (currentSectionIndex == lineRendererToFollow.positionCount - 1)
                    TouchPlayer();
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
        transform.position = newPosition;
    }


    [Obsolete]
    void UpdateGradient()
    {

        Gradient newGradient = new Gradient();
        float ratio = (float)((float)currentSectionIndex / (float)lineRendererToFollow.positionCount);
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];
        GradientColorKey[] colorKeys = new GradientColorKey[2] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 0f) };

        // Leftest key
        alphaKeys[0].alpha = 0f;
        alphaKeys[0].time = 0;

        // Left key
        alphaKeys[1].alpha = 0f;
        alphaKeys[1].time = ratio + (1 / lineRendererToFollow.positionCount) * 5;

        // Right key
        alphaKeys[2].alpha = 1f;
        alphaKeys[2].time = ratio + (1 / lineRendererToFollow.positionCount) * 10;

        // Rightest key
        alphaKeys[3].alpha = 1f;
        alphaKeys[3].time = 1f;


        newGradient.SetKeys(colorKeys, alphaKeys);
        lineRendererToFollow.colorGradient = newGradient;
    }
    #endregion







    #region PROPAGATE
    void CheckForAdjacentSection()
    {
        for (int i = 0; i < lineRendererToFollow.positionCount - 1; i++)
        {
            int indexDifference = Mathf.Abs(currentSectionIndex - i);
            Vector3 position = lineRendererToFollow.GetPosition(i);

            if (canPropagate && position.z == 0 && Vector3.Distance(position, transform.position) < distanceToSpread && indexDifference > indexNonSpreadRange)
            {
                SpreadFire(i);
                canPropagate = false;
            }
        }
    }
    void SpreadFire(int index)
    {
        if (canPropagate)
        {
            propagationStartTime = Time.time;
            Flame newFlame = Instantiate(flamePrefab).GetComponent<Flame>();
            newFlame.standing = false;
            newFlame.lineRendererToFollow = lineRendererToFollow;
            newFlame.RestartMovingFromBeginning(index);
        }
    }
    #endregion






    void TouchPlayer() => Die();

    void Die()
    {
        DisableEffects();
        moving = false;
        StartCoroutine(FadeSprites());
        DestroyColliders();
        Destroy(gameObject, 5f);
    }

    void DisableEffects()
    {
        if (effects != null && effects.Count > 0)
            for (int i = 0; i < effects.Count; i++)
                if (effects[i])
                    effects[i].Stop();
    }

    IEnumerator FadeSprites()
    {
        if (sprites != null && sprites.Count > 0)
        {
            bool notFaded = true;
            while (notFaded)
            {
                notFaded = false;
                for (int i = 0; i < sprites.Count; i++)
                {
                    if (sprites[i] && sprites[i].color.a > 0)
                    {
                        sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, sprites[i].color.a - 0.1f);
                        notFaded = true;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    void DestroyColliders()
    {
        if (colliders != null && colliders.Count > 0)
            for (int i = 0; i < colliders.Count; i++)
                if (colliders[i])
                    Destroy(colliders[i]);
    }
}
