using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Card : MonoBehaviour
{
    protected TurnManager turnManager;
    [SerializeField]protected CardData _myCardData;
    private SpriteRenderer _spriteRenderer;

    public void Awake()
    {
        var manager = GameObject.Find("GameManager");
        turnManager = manager.GetComponent<TurnManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _myCardData.cardImage;
    } 

    public virtual void Use()
    {
        Debug.Log("Tried to use an undefined cardtype!");
    }

    public void Discard() 
    {
        Destroy(gameObject);
    }

    public CardType GetCardType(){
        if(_myCardData == null){
            Debug.Log("Tried to load undefined carddata!");
        }
        return _myCardData.cardType;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)){
            StartCoroutine(turnManager.CardSelected(this));
        }
    }

    void OnDestroy()
    {
        turnManager.ExpendCard(this);
    }

}
