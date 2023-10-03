using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFx : MonoBehaviour
{
    float _curLife;
    [SerializeField] float _lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        _curLife = _lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_curLife > 0)
        {
            _curLife -= Time.deltaTime;
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }
}
