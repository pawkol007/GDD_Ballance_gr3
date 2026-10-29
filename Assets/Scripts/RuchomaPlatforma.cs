using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RuchomaPlatforma : MonoBehaviour
{
    [SerializeField] GameObject PunktA;
    [SerializeField] GameObject PunktB;
    [SerializeField] float speed = 10f;
    [SerializeField] float delay = 1f;
    [SerializeField] GameObject platforma;

    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platforma.transform.position = PunktA.transform.position;
        targetPosition = PunktB.transform.position;
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            while ((targetPosition - platforma.transform.position).sqrMagnitude > 0.01f)
            {
                platforma.transform.position = Vector3.MoveTowards(platforma.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            targetPosition = targetPosition == PunktA.transform.position ? PunktB.transform.position : PunktA.transform.position;

            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
