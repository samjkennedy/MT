using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomnessController : MonoBehaviour
{
    public static RandomnessController instance;

    /* Good seeds what I done found
    -316799720
    */

    public int Seed;
    private int seed;
    public bool useSeed;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        if (useSeed) {
            seed = Seed;
            Random.seed = seed;
        } else {
            Seed = Random.seed;
        }
    }
}
