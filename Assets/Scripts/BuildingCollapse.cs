using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollapse : MonoBehaviour
{

    public Transform ThisTransform = null;
    private int counter = 0;
    public float Speed = 1f;
    public AnimationCurve AnimCurve;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnTriggerExit(Collider hit)
	{
		// On death
		if (hit.gameObject.tag == "Player")
		{
            counter = 0;
			StartCoroutine(collapse());

		}
	}

    IEnumerator collapse() {
        Transform ThisTransform = GetComponent<Transform>();
        while ( counter < 500)
        {
            Debug.Log(counter);
            counter ++;
            if (ThisTransform != null)
            {
                ThisTransform.position -= new Vector3(0f,  Speed * AnimCurve.Evaluate(Time.time) * Time.deltaTime, 0f);
            }
            yield return null;
        }

            
	}
}
