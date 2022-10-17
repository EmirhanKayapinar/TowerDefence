using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]


public class EnemyMover : MonoBehaviour
{

    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField] [Range(0f,5f)] float speed = 1f;

    Enemy enemy;
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());

    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void FindPath()
    {
        path.Clear();

        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform item in parent.transform)
        {
            Tile waypoint = item.GetComponent<Tile>();
            if (waypoint != null)
            {
                path.Add(waypoint);
            }
            
            
            
        }
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }
    IEnumerator FollowPath()
    {
        foreach (Tile item in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = item.transform.position;
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent<1f)
            {
                travelPercent += Time.deltaTime*speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}
