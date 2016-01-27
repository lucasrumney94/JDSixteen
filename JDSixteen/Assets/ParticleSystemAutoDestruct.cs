using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestruct : MonoBehaviour
{
    private ParticleSystem ps;


    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}