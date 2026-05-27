using UnityEngine;
using UnityEngine.AI; // NavMesh sistemini kullanmak için bu kütüphane şarttır!

// Bu satır, bu kodu bir objeye attığımızda NavMeshAgent'in da otomatik eklenmesini sağlar
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Oyuncu ve Menzil Ayarları")]
    public Transform player;
    public float chaseRange = 10f; // Oyuncuyu ne kadar yakından fark edeceği

    [Header("Devriye Ayarları")]
    // Düşmanın gezeceği noktaların listesi
    public Transform[] patrolPoints; 
    private int currentPatrolIndex = 0;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Oyun başlar başlamaz ilk devriye noktasına git
        GoToNextPatrolPoint(); 
    }

    void Update()
    {
        // 1. Oyuncu ile düşman arasındaki mesafeyi matematiksel olarak ölç
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 2. Eğer oyuncu fark edilme menzilinin içindeyse (CHASE - KOVALAMA DURUMU)
        if (distanceToPlayer <= chaseRange)
        {
            // Ajanın hedefini oyuncunun anlık pozisyonu yap
            agent.SetDestination(player.position);
        }
        // 3. Eğer oyuncu uzaktaysa (PATROL - DEVRİYE DURUMU)
        else
        {
            // Eğer ajan mevcut hedefine çok yaklaştıysa ve yeni yol hesaplamıyorsa
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint(); // Sıradaki noktaya ilerle
            }
        }
    }

    // Sıradaki devriye noktasını hesaplayan fonksiyon
    private void GoToNextPatrolPoint()
    {
        // Eğer sahnede devriye noktası belirlenmemişse hata vermesin diye kontrol et
        if (patrolPoints.Length == 0) return;

        // Ajanı sıradaki hedefe yönlendir
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        // İndeksi 1 artır. Eğer son noktaya ulaştıysa başa dön (Mod alma işlemi '%' bunun içindir)
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // Eğitici Ekstra: Unity Editor (Scene) ekranında kovalama menzilini kırmızı bir daire ile çizer.
    // Hocanın da çok hoşuna gidecek profesyonel bir debug detayıdır.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
