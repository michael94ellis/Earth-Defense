using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

	void Start()
	{
        StartCoroutine(DestroyTimer());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
