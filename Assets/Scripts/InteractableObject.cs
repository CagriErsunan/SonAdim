using UnityEngine;
using DG.Tweening;

public class InteractableObject : MonoBehaviour
{
    public ChoiceManager choiceManager;
    public float moveHeight = 0.5f;
    public float moveDuration = 0.2f;
    private Vector3 originalPosition;
    private bool isAnimating = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    Debug.Log("Objeye tıklandı: " + gameObject.name);
                    choiceManager.Topla();
                    AnimateBounce();
                }
            }
        }
    }

    void AnimateBounce()
    {
        isAnimating = true;
        transform.DOMoveY(originalPosition.y + moveHeight, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMoveY(originalPosition.y, moveDuration)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => isAnimating = false);
            });
    }
}