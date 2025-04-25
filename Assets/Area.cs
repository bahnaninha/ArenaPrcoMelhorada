using UnityEngine;
using UnityEngine.AI;

public class Area : MonoBehaviour
{
    public float wanderRadius = 3f;
    public float wanderTimer = 0.5f;
    public float detectionRadius = 8f;
    public string enemyTag = "Enemy"; // Defina essa tag no inspetor ou no código
    public float delayBeforeSearch = 2f; // Tempo de espera antes de começar a procurar inimigos

    private NavMeshAgent agent;
    private float timer;
    private float startDelayTimer;
    private bool isReadyToSearch = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        startDelayTimer = delayBeforeSearch; // Inicializa o temporizador de atraso

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent não encontrado!");
        }
    }

    void Update()
    {
        // Se ainda estamos no tempo de atraso antes de começar a buscar inimigos
        if (startDelayTimer > 0)
        {
            startDelayTimer -= Time.deltaTime;
            return; // Não faz nada enquanto o temporizador não chegou a 0
        }

        isReadyToSearch = true; // Após o tempo de atraso, o NPC pode começar a procurar

        timer += Time.deltaTime;

        GameObject closestEnemy = FindClosestEnemyByTag();
        if (closestEnemy != null)
        {
            Debug.Log("Inimigo encontrado: " + closestEnemy.name); // Log para verificar se o inimigo foi encontrado
            agent.SetDestination(closestEnemy.transform.position);
        }
        else if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    GameObject FindClosestEnemyByTag()
    {
        if (!isReadyToSearch) return null; // Não procurar até que o NPC esteja pronto

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == this.gameObject) continue; // Não considerar o próprio NPC

            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            // Verifica se o inimigo está dentro do raio de detecção
            if (dist <= detectionRadius && dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = enemy;
            }
        }

        if (closest != null)
        {
            Debug.Log("Inimigo mais próximo: " + closest.name + ", Distância: " + shortestDistance);
        }

        return closest;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Visualização do raio de detecção no editor
    }
}
