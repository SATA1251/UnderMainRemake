using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public GameObject _dropAmountParticleS;
    public GameObject _dropAmountParticleA;
    public GameObject _dropAmountParticleB;

    public void Start()
    {
        _dropAmountParticleS = GameObject.Find("GemDropS");
        _dropAmountParticleA = GameObject.Find("GemDropA");
        _dropAmountParticleB = GameObject.Find("GemDropB");
    }

    public void DestroyGem()
    {        
        if (gameObject.CompareTag("GemS"))
        {
            player.GetComponent<PlayerController>().oresSamount++;
            SpawnParticle(_dropAmountParticleS);
        }
        else if (gameObject.CompareTag("GemA"))
        {
            player.GetComponent<PlayerController>().oresAamount++;
            SpawnParticle(_dropAmountParticleA);
        }
        else if (gameObject.CompareTag("GemB"))
        {
            player.GetComponent<PlayerController>().oresBamount++;
            SpawnParticle(_dropAmountParticleB);
        }
        Destroy(gameObject);
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
}
