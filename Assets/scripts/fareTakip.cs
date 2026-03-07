using UnityEngine;

public class fareTakip : MonoBehaviour
{
    Plane plane;

    void Start()
    {
        // Eye'ın bulunduğu yükseklikte bir düzlem oluşturuyoruz
        plane = new Plane(Vector3.up, transform.position);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            transform.LookAt(target);
        }
    }
}