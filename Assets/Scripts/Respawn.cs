using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField] private Transform SpawnPoint;								// From where do we spawn the player
    private CharacterController Rigidbody;
	private GameObject[] buildings;
	public float BuildingSpawnY = -75;
	public int NumberOfRespawns = 0;
	private AudioManager audioManager;

    private void SpawnCharacter()
	{
		NumberOfRespawns ++;

		if (NumberOfRespawns >= 5)
		{
			audioManager = AudioManager.instance;       
        	audioManager.Play("dejate");
			audioManager.IncrementVolume("dejate", 30);
		}

		buildings = GameObject.FindGameObjectsWithTag("Building");
		foreach (GameObject building in buildings)
        {
			building.GetComponent<BuildingCollapse>().uncollapse();
            building.transform.position = new Vector3(building.transform.position.x,
													  BuildingSpawnY*2,
													  building.transform.position.z);
        }

		Rigidbody = GetComponent<CharacterController>();
		Rigidbody.transform.position = SpawnPoint.transform.position;
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
		yield return new WaitForSeconds (1f);
		SpawnCharacter();
	}
}
