using UnityEngine;
using System.Collections;

public class desactivate : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

    }
}
