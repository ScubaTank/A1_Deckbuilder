using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    [TextArea]
    public string cardDescription;
    public int cardValue;
    public CardType cardType;
}

[System.Serializable]
public enum CardType
{
    Popularity,
    Bully,
    Nerd,
    Challenge,
    Locker
}
