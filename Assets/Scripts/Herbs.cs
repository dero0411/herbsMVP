using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Herbs : MonoBehaviour
{

    public GameObject herbsPrefab;
    public float spawnProbability;
    public float speed = 5f;

    private Vector2 targetPosition;
    private Vector2 originalPosition;
    public bool isMoving = true;

    public bool isCarried = false;



    private HerbArray targetPoint;
 
    public void SetTargetPosition(Vector2 target, HerbArray targetPoint)
    {
        originalPosition = transform.position;
        targetPosition = target;
        this.targetPoint = targetPoint;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
           if(isCarried)
            {
                isCarried = false;
                PickUp();
            }
        }

       if (isMoving)
        {
            MoveTowardsTarget();
        }

    }
    void MoveTowardsTarget()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(currentPosition, targetPosition) < 0.01f)
        {
            isMoving = false;
            StartCoroutine(CheckStateAfterDelay(2f));
        }
    }
    IEnumerator CheckStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetPoint.IsFull())
        {

            SetTargetPosition(originalPosition, targetPoint);// �ʱ���ġ�� ���ƿ��°�
            //Transform newTargetTransform = Spawn.targetPoints[Random.Range(0, Spawn.targetPoints.Length)]; //���R�� ���°� ���߿� �ؾ�¡ �����Ĺ�
            //HerbArray newTargetPoint = newTargetTransform.GetComponent<HerbArray>();
            //SetTargetPosition(newTargetTransform.position, newTargetPoint);
            isMoving = true;
        }
        else
        {
            targetPoint.AddHerb(this);
        }
    }

    public void PickUp()
    {
        // Player ��ü�� ã��
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            // Herb�� Transform�� �θ� Player�� Transform���� ����
            transform.SetParent(player.transform);

            // Herb�� Player�� ����ٴϵ��� ����
            isCarried = true;

            // Herb�� Collider�� ��Ȱ��ȭ�Ͽ� �ٸ� ������Ʈ���� �浹�� ����
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
