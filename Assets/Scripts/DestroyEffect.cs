using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

	void Start()
	{
        transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        StartCoroutine(DestroyTimer());
    }

    /// Must be called like so: StartCoroutine(LaserWasFired());
    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
