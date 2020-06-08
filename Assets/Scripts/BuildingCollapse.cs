using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollapse : MonoBehaviour
{

    public Transform ThisTransform = null;
    private int counter = 0;
    public float Speed = 15f;
    public AnimationCurve AnimCurve;

    private void OnTriggerExit(Collider hit)
	{
		// On death
		if (hit.gameObject.tag == "Player")
		{
            counter = 0;
			StartCoroutine(collapse());

		}
	}

    public void uncollapse()
    {
        StopAllCoroutines();
    }

    IEnumerator collapse() {
        Transform ThisTransform = GetComponent<Transform>();
        while ( counter < 800)
        {
            counter ++;
            if (ThisTransform != null)
            {
                ThisTransform.position -= new Vector3(0f,  Speed * AnimCurve.Evaluate(Time.time) * Time.deltaTime, 0f);
            }
            yield return null;
        }

            
	}
}
