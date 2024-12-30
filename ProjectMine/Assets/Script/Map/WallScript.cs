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
    public GameObject _dropAmountParticleS;
    public GameObject _dropAmountParticleA;
    public GameObject _dropAmountParticleB;

    public Transform _hitParticleTransform;

    public IObjectPool<GameObject> Pool { get; set; }

    private float _dropAmountS;
    private float _dropAmountA;
    private float _dropAmountB;

    public void Start()
    {
        _hitParticleObject = GameObject.Find("BlockHit");
        _dropAmountParticleS = GameObject.Find("GemDropS");
        _dropAmountParticleA = GameObject.Find("GemDropA");
        _dropAmountParticleB = GameObject.Find("GemDropB");
        _player = GameObject.Find("Player");
        _dropAmountS = 5f;
        _dropAmountA = 10f;
        _dropAmountB = 20f;
    }
    public void DestroyWall()
    {
        ReleaseObject();
        SpawnParticle(_hitParticleObject);
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

    public void SpawnParticle(GameObject Particle)
    {
        if (Particle != null)
        {
            Vector3 position = transform.position;
            Quaternion rotation = Quaternion.identity;

            GameObject particle = Instantiate(Particle, position, rotation);

            Destroy(particle, particle.GetComponent<ParticleSystem>().main.duration);
        }
    }

    private void DropAmount()
    {
        float randomValue = Random.Range(0f, 1000f); 

        if(randomValue <= _dropAmountS)
        {
            _player.GetComponent<PlayerController>().oresSamount++;
            SpawnParticle(_dropAmountParticleS);
        }
        else if(randomValue <= _dropAmountA)
        {
            _player.GetComponent<PlayerController>().oresAamount++;
            SpawnParticle(_dropAmountParticleA);
        }
        else if (randomValue <= _dropAmountB)
        {
            _player.GetComponent<PlayerController>().oresBamount++;
            SpawnParticle(_dropAmountParticleB);
        }
    }
}
