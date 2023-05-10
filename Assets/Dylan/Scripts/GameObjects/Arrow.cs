using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float speed;
    bool move;
    bool pause = false;
    float lifetime;

    public void Initialize(float _speed, float _lifeTime)
    {
        speed = _speed;
        move = true;
        //Invoke("DestroyArrow", _lifeTime);
        lifetime = _lifeTime;
        StartCoroutine(LifeTimer());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

    public void Pause()
    {
        move = false;
        pause = true;
    }

    public void UnPause()
    {
        move = true;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    IEnumerator LifeTimer()
    {
        float actualLifetime = lifetime;
        while (actualLifetime > 0)
        {
            if (pause)
            {
                yield return null;
            }
            else
            {
                actualLifetime -= .5f;
                yield return new WaitForSecondsRealtime(.5f);
            }
        }
        SelfDestruct();
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
