using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Card : MonoBehaviour
{

    [SerializeField] private TMP_Text _nameUI, _descriptionUI;
    [SerializeField] private GameObject _imageObject, _backObject;

    private Image _imageUI;
    private bool _displayCard;
    [field: SerializeField]
    private CardData _myCardData;

    public bool DisplayCard
    {
        get
        {
            return _displayCard;
        }
        set
        {
            _displayCard = value;
            _backObject.gameObject.SetActive(_displayCard);
        }
    }

    void Awake()
    {
        if(_myCardData != null)
        {
            RefreshData();
        }
    }

    private void RefreshData()
    {
        _imageUI = _imageObject.GetComponent<Image>();
        _nameUI.text = _myCardData.cardName;
        _descriptionUI.text = _myCardData.cardDescription;
        _imageUI.sprite = _myCardData.cardImage;
    }
    
    public void UpdateCard(CardData data)
    {
        _myCardData = data;
        RefreshData();
    }

    public CardData GetCardData()
    {
        return _myCardData;
    }
}
