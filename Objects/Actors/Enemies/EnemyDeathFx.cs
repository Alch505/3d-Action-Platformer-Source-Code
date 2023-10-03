using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathFx : MonoBehaviour
{
    [SerializeField] float _timer = 3f;

    private void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        else Destroy(this.gameObject);
    }
}
