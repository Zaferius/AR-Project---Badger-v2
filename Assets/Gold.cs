using System;
using DG.Tweening;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public float autoCollectDistance = 0.7f;  // Üzerine yürüyünce otomatik toplama mesafesi
    public float tapCollectDistance = 1.5f;   // Dokunarak toplamak için gereken yakınlık

    private bool isCollected = false;
    
    public float rotateSpeed = 30f;      // saniyede kaç derece dönsün
    public float floatDistance = 0.2f;   // yukarı aşağı kaç birim oynasın
    public float floatDuration = 1.5f;   // yukarı aşağı hareketin süresi
    
    private Vector3 startPos;
    private Vector3 startScale;

    private void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;

        // Yukarı aşağı hover animasyonu (sonsuz loop)
        transform.DOMoveY(startPos.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DOScale(0, 0).OnComplete(()=> 
           transform.DOScale(startScale, 0.2f).SetEase(Ease.OutBack));
    }

    void Update()
    {
        if (isCollected) return;
        
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);

        // 1) Üstüne yürüyünce otomatik toplama
        if (dist < autoCollectDistance)
        {
            Collect();
        }

        // 2) Çok yakına gelince dokunarak toplama
        if (dist < tapCollectDistance && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Collect();
                }
            }
        }
    }

    private void Collect()
    {
        isCollected = true;
        Debug.Log("Altın toplandı!");
        // Buraya skor sistemi bağlanabilir
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        transform.DOKill(); // Bu objeye bağlı tüm DOTween animasyonlarını durdurur
    }
}