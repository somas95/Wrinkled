using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField] private Transform SpawnPoint;								// From where do we spawn the player
    private CharacterController Rigidbody;


    void Awake()
    {
        Rigidbody = GetComponent<CharacterController>();
    }

    private void SpawnCharacter()
	{
		Rigidbody.transform.position = SpawnPoint.transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider hit)
	{
		// On death
		if (hit.gameObject.tag == "Respawn")
		{
			StartCoroutine(die());
			StopCoroutine(die());

		}
	}

    IEnumerator die() {
		yield return new WaitForSeconds (1.75f);
		SpawnCharacter();
	}
}
