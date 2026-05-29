using UnityEngine;
using UnityEngine.AI; // NavMesh sistemini kullanmak için bu kütüphane şarttır!

// Bu satır, bu kodu bir objeye attığımızda NavMeshAgent'in da otomatik eklenmesini sağlar
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Oyuncu ve Menzil Ayarları")]
    public Transform player;
    public float chaseRange = 10f;
    public float catchRange = 1.5f; // Oyuncuyu ne kadar yakından fark edeceği

    [Header("Devriye Ayarları")]
    // Düşmanın gezeceği noktaların listesi
    public Transform[] patrolPoints; 
    private int currentPatrolIndex = 0;

    [Header("Animasyon")]
    public Animator enemyAnimator;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Oyun başlar başlamaz ilk devriye noktasına git
        GoToNextPatrolPoint(); 
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= catchRange)
        {
            GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
            if (gameOverManager != null) gameOverManager.ShowGameOver();
            return; 
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // KOVALAMA DURUMU
            agent.SetDestination(player.position);
            
            // YENİ: Koşma animasyonunu başlat
            if (enemyAnimator != null) enemyAnimator.SetBool("isChasing", true); 
        }
        else
        {
            // DEVRİYE (YÜRÜME) DURUMU
            
            // YENİ: Koşmayı bırak, yürümeye dön
            if (enemyAnimator != null) enemyAnimator.SetBool("isChasing", false); 

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint(); 
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
