using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WallScript : PoolAble
{
    [SerializeField]
    public GameObject planeData;

    public GameObject _player;

    public GameObject _hitParticleObject;

    public Transform _hitParticleTransform;

    public IObjectPool<GameObject> Pool { get; set; }

    private float _dropAmountS;
    private float _dropAmountA;
    private float _dropAmountB;

    public void Start()
    {
        _hitParticleObject = GameObject.Find("BlockHit");
        _player = GameObject.Find("Player");
        _dropAmountB = 5f;
        _dropAmountS = 10f;
        _dropAmountA = 20f;
    }
    public void DestroyWall()
    {
        ReleaseObject();
        SpawnParticle();
        DropAmount();
        //Destroy(gameObject);        
        //DestroyWallLine(0,0);
    }

    private void DestroyWallLine(int x, int z)
    {
        for (int i = 0; i < 4; i++)
        {         
            Destroy(planeData.GetComponent<MapLoader>().wallArray[(int)(gameObject.transform.position.x-0.5f)+x, (int)(gameObject.transform.position.z-0.5f)+z, i]);
        }
    }

    public void SpawnParticle()
    {
        if (_hitParticleObject != null)
        {
            Vector3 position = transform.position;
            Quaternion rotation = Quaternion.identity;

            GameObject particle = Instantiate(_hitParticleObject, position, rotation);

            Destroy(particle, particle.GetComponent<ParticleSystem>().main.duration);
        }
    }

    private void DropAmount()
    {
        float randomValue = Random.Range(0f, 1000f); 

        if(randomValue <= _dropAmountS)
        {
            _player.GetComponent<PlayerController>().oresSamount++;
        }
        else if(randomValue <= _dropAmountA)
        {
            _player.GetComponent<PlayerController>().oresAamount++;
        }
        else if (randomValue <= _dropAmountB)
        {
            _player.GetComponent<PlayerController>().oresBamount++;
        }
    }
}
