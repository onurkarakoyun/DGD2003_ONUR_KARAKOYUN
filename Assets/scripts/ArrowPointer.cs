using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    private Transform targetObj; // Okun bakacağı hedef

    void Update()
    {
        if (targetObj != null)
        {
            // Ok, hedef bilgisayara doğru dönsün
            transform.LookAt(targetObj);
            
            // İstersen sadece Y ekseninde (sağ-sol) dönmesi için yukarıdaki satırı silip şunları yazabilirsin:
            // Vector3 targetPosition = new Vector3(targetObj.position.x, transform.position.y, targetObj.position.z);
            // transform.LookAt(targetPosition);
        }
    }

    // Görev yöneticisi bu fonksiyonu çağırıp yeni hedefi verecek
    public void SetTarget(Transform newTarget)
    {
        targetObj = newTarget;
    }
}