using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MoveNPC : MonoBehaviour
{
    public float tempoEntreMovimentos = 3.0f; // Tempo entre movimentos aleatórios
    public float raioMovimento = 10.0f; // Raio em torno do NPC onde ele pode se mover aleatoriamente
    private NavMeshAgent agente;

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        StartCoroutine(MovimentoAleatorio());
    }

    private IEnumerator MovimentoAleatorio()
    {
        while (true)
        {
            // Aguarda o tempo entre movimentos aleatórios
            yield return new WaitForSeconds(tempoEntreMovimentos);

            // Define um ponto aleatório dentro do raio do movimento ao redor do NPC
            Vector3 pontoAleatorio = Random.insideUnitSphere * raioMovimento;
            pontoAleatorio += transform.position;

            // Verifica se o ponto gerado está dentro do NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pontoAleatorio, out hit, raioMovimento, NavMesh.AllAreas))
            {
                // Move o NPC para o ponto aleatório
                agente.SetDestination(hit.position);
            }
        }
    }
}
