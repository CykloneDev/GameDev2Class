using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private enum State
    {
        Lowered,
        Lowering,
        Raising,
        Raised
    }

    public Transform spikeHolder;
    public GameObject hitSpike;
    public GameObject hitSpikeCollider;

    public float interval;
    public float raiseTime;
    public float lowerTime;
    public float waitTime;

    private State state = State.Lowered;

    private float spikeHeight = 4f;
    private float spikeLowered = 0.3f;
    private float switchTime = Mathf.NegativeInfinity;

    Vector3 scale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Raise", interval);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Lowering)
        {
            scale = spikeHolder.localScale;
            scale.y = Mathf.Lerp(spikeHeight, spikeLowered, (Time.time - switchTime) / lowerTime);
            spikeHolder.localScale = scale;

            if (scale.y == spikeLowered)
            {
                Invoke("Raise", interval);
                state = State.Lowering;
                hitSpikeCollider.SetActive(false);
            }
        }
        else if (state == State.Raising)
        {
            scale = spikeHolder.localScale;
            scale.y = Mathf.Lerp(spikeLowered, spikeHeight, (Time.time - switchTime) / raiseTime);
            spikeHolder.localScale = scale;

            if (scale.y == spikeHeight)
            {
                Invoke("Lower", interval);
                state = State.Raising;
                hitSpikeCollider.SetActive(true);
                hitSpike.SetActive(false);
            }
        }
    }


    void Raise()
    {
        switchTime = Time.time;
        state = State.Raising;
        hitSpike.SetActive(true);

    }

    void Lower()
    {
        switchTime = Time.time;
        state = State.Lowering;
    }
}