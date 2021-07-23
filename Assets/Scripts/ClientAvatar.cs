using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClientAvatar : MonoBehaviour
{
    public float Speed = 2f;
    public Transform OrderTransform;
    public SpriteRenderer TimerFillSprite;

    public SpriteRenderer Body;
    public SpriteRenderer Head;

    public Sprite[] BodySprites;
    public Sprite[] HeadSprites;

    [Space]
    public Client Client;

    public float TargetPositionInLine;

    private float _currentNormalizedPosition = 0f;
    private bool _punchAnimationPlayed = false;

    public void Initialize(Client client)
    {
        this.Body.sprite = BodySprites[Random.Range(0, BodySprites.Length)];
        this.Head.sprite = HeadSprites[Random.Range(0, HeadSprites.Length)];
        this.Client = client;
        _currentNormalizedPosition = 0f;


        var order = client.Order;
        if (client.RandomOrder == -1)
        {
            AddObjectToOrder(PlayerManager.Instance.PrefabsBase[(int)order.Base]);
            foreach (var flavour in order.Flavours)
            {
                AddObjectToOrder(PlayerManager.Instance.PrefabsFlavours[(int)flavour]);
            }
            if (((int)order.Topping) > 0)
                AddObjectToOrder(PlayerManager.Instance.PrefabsToppings[(int)order.Topping - 1]);
        }
        else
        {
            AddObjectToOrder(PlayerManager.Instance.PrefabsRandomOrders[client.RandomOrder]);
        }
    }

    public Vector3 GetPositionAlongTheLine(float normalizedPosition)
    {
        var path = ClientsManager.Instance.ClientsLinePath;
        float segmentedNormalizedPosition = (path.Length - 1) * normalizedPosition;
        if (segmentedNormalizedPosition >= path.Length - 1)
        {
            segmentedNormalizedPosition -= 0.001f;
        }

        Transform to = path[(int)segmentedNormalizedPosition + 1];
        Transform from = path[((int)segmentedNormalizedPosition)];

        float localNormalizedPosition = segmentedNormalizedPosition - ((int)segmentedNormalizedPosition);

        return Vector3.Lerp(from.position, to.position, localNormalizedPosition);
    }

    private void Update()
    {
        TimerFillSprite.material.SetFloat("_Arc2", Client.Timer * 360f);
        TargetPositionInLine = ClientsManager.Instance.ActiveClients.IndexOf(Client);

        if (TargetPositionInLine == 0 && !OrderTransform.gameObject.activeSelf)
        {
            OrderTransform.gameObject.SetActive(true);
        }

        if (TargetPositionInLine < 0 || Client.Punched)
        {
            if(Client.Punched && !_punchAnimationPlayed)
            {
                var animator = this.GetComponent<Animator>();
                animator.Play("GetPunched");
                _punchAnimationPlayed = true;
            }
            if (OrderTransform.gameObject.activeSelf)
            {
                OrderTransform.gameObject.SetActive(false);
            }
            if (TimerFillSprite.transform.parent.gameObject.activeSelf)
            {
                TimerFillSprite.transform.parent.gameObject.SetActive(false);
            }
            this.transform.position += Vector3.left * Speed * 10 * Time.deltaTime;
            if (this.transform.position.x < -7f)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            float targetNormalizedPosition = 1f - (TargetPositionInLine / ((float)ClientsManager.Instance.ActiveClients.Count));
            if (ClientsManager.Instance.ActiveClients.Count == 1)
                targetNormalizedPosition = 1f;

            _currentNormalizedPosition += Mathf.Min(Speed * Time.deltaTime, targetNormalizedPosition - _currentNormalizedPosition);

            this.transform.position = GetPositionAlongTheLine(_currentNormalizedPosition);


            this.transform.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -_currentNormalizedPosition);

        }
    }

    private void AddObjectToOrder(GameObject go)
    {
        Transform b = Instantiate(go, OrderTransform).transform;

        float baseOffset = (OrderTransform.childCount > 2 && OrderTransform.GetChild(1).name == "BaseCone(Clone)") ? 0.35f : 0f;

        if (go.name.StartsWith("Topping"))
        {
            baseOffset -= 0.3f;
        }
        Debug.Log("base name: " + OrderTransform.GetChild(1).name + ", offset: " + baseOffset);
        b.localPosition = new Vector3(0, baseOffset + OrderTransform.childCount * 0.5f, -OrderTransform.childCount * 0.01f);
    }
}
