using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float speed;
    bool move;

    public void Initialize(float _speed, float _lifeTime)
    {
        speed = _speed;
        move = true;
        Invoke("DestroyArrow", _lifeTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }   

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}
