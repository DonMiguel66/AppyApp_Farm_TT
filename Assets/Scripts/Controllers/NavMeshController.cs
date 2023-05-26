using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class NavMeshController : BaseController, IExecute
    {
        private List<NavMeshAgent> _navMeshAgents;

        private List<Transform> _destinationPoints;

        private readonly float _randomPointRadius = 2.1f;
        private Transform _patrolZoneCenter;
        private NavMeshPath _navMeshPath;

        public NavMeshController(List<NavMeshAgent> navMeshAgents, Transform patrolZoneCenter)
        {
            _navMeshAgents = navMeshAgents;
            _patrolZoneCenter = patrolZoneCenter;
            _navMeshPath = new NavMeshPath();
            //NavRandomPoint(_navMeshAgent);
            foreach (var navMeshAgent in navMeshAgents)
            {
                NavRandomPoint(navMeshAgent);
            }
        }
        public void Execute()
        {
            foreach (var navMeshAgent in _navMeshAgents)
            {
                CheckAgentDestination(navMeshAgent);
            }
        }

        private void CheckAgentDestination(NavMeshAgent agent)
        {
            if (agent.remainingDistance < 0.2)
            {
                NavRandomPoint(agent);
            }
        }
        
        private void NavRandomPoint(NavMeshAgent navMeshAgent)
        {
            bool getCorrectPoint = false;
            Vector3 randomPoint = _patrolZoneCenter.position;
            while (!getCorrectPoint)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(Random.insideUnitSphere * _randomPointRadius + _patrolZoneCenter.position,
                    out hit, _randomPointRadius, NavMesh.AllAreas);
                randomPoint = hit.position;
                navMeshAgent.CalculatePath(randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) getCorrectPoint = true;

            }
            //Debug.DrawLine(_navMeshAgent.transform.position, randomPoint, Color.red);
            navMeshAgent.SetDestination(randomPoint);
        }
    }
}