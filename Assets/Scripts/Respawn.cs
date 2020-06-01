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

    private void OnCollisionEnter(Collision other)
	{
		// On death
		if (other.gameObject.tag == "Respawn")
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
