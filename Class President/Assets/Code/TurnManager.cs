using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _cardPrompt;
    [SerializeField] private TMP_Text _aiCardText;
    [SerializeField] private TMP_Text _aiInfluenceText;
    [SerializeField] private TMP_Text _playerInfluenceText;
    [SerializeField] private TMP_Text _turnText;



    [Header("Card Transforms")]
    [SerializeField] private Transform _cardSlot1;
    [SerializeField] private Transform _cardSlot2;
    [SerializeField] private Transform _cardSlot3;
    [SerializeField] private Transform _cardSlot4;
    [SerializeField] private Transform _cardSlot5;
    [SerializeField] private Transform _cardSlot6;
    [SerializeField] private Transform _cardSlot7;
    [SerializeField] private Transform _aiHandTransform;

    private Transform[] _cardSlotArray = new Transform[7];

    [Header("Cards")]
    [SerializeField] private Card _popularity;
    [SerializeField] private Card _bully;
    [SerializeField] private Card _nerd;
    [SerializeField] private Card _locker;
    [SerializeField] private Card _challenge;

    [Header("Hands")]
    private List<Card> _playerHand = new List<Card>();
    private List<Card> _aiHand = new List<Card>();

    [Header("Gameplay  Vars")]
    [SerializeField]private int _playerInfluence = 3;
    [SerializeField]private int _aiInfluence = 3;
    private bool _isPlayerTurn;

    private Card _cardSelected;


    //setup functions
    void Awake()
    {
        InitHandTransforms();
        _cardPrompt.enabled = false; //for some reason, I have to set this to false first for it to work.
    }
    void Start()
    {
        DrawFirstHand();
    }

    void InitHandTransforms(){
        _cardSlotArray[0] = _cardSlot1;
        _cardSlotArray[1] = _cardSlot2;
        _cardSlotArray[2] = _cardSlot3;
        _cardSlotArray[3] = _cardSlot4;
        _cardSlotArray[4] = _cardSlot5;
        _cardSlotArray[5] = _cardSlot6;
        _cardSlotArray[6] = _cardSlot7;
    }

    //Turn Managing functions
    public IEnumerator CardSelected(Card c){
        _cardPrompt.enabled = true;
        yield return WaitForSelection(c);
    }

    private IEnumerator WaitForSelection(Card c)
    {
        _cardSelected = c;
        //if player presses u, use. if player presses d, discard.

        bool selecting = true;
        while(selecting){
            if(Input.GetKeyDown(KeyCode.U)){
                _cardSelected.Use();
                selecting = false;
                _cardPrompt.enabled = false;
                _cardSelected = null;
                
            }
            if(Input.GetKeyDown(KeyCode.D)){
                _cardSelected.Discard();
                selecting = false;
                _cardPrompt.enabled = false;
                _cardSelected = null;
            }
            yield return null;
        }
    }
    public void NextTurn(){
        SortHand();
        if(CheckWin())return; 

        if(_isPlayerTurn){
            StartCoroutine(AiDelay());
        } else {
            PlayerTurn();
        }
    }
    IEnumerator AiDelay(){
        _isPlayerTurn = false;
        UpdateUI();
        yield return new WaitForSeconds(3);
        AiTurn();
    }
    void PlayerTurn(){
        UpdateUI();
        Debug.Log("Starting player turn!");
        _isPlayerTurn = true;
        DrawCard();
        
    }
    void AiTurn(){

        //if influence is less than max, try to use an influence card
        Debug.Log("Ai starting their turn!");
        DrawCard();
        foreach(Card c in _aiHand.ToArray()){
            if(c.GetCardType() == _popularity.GetCardType() && _aiInfluence < 3){
                c.Use();
                break;
            }
            if(c.GetCardType() == _bully.GetCardType()){
                c.Use();
                break;
            }
            if(c.GetCardType() == _challenge.GetCardType()){
                c.Use();
                break;
            }
            if(c.GetCardType() == _locker.GetCardType()){
                c.Use();
                break;
            }
            if(c.GetCardType() == _nerd.GetCardType()){
                c.Discard();
                break;
            }
        }

    }

    bool CheckWin(){
        //check influence of each player, then count nerdcards of each hand.
        //check influence
        if(_aiInfluence <= 0){
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerWinScene");
            return true;
        } else if (_playerInfluence <= 0) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("AIWinScene");
            return true;
        }

        //count nerdcards
        int nerdPlayerCount = 0;
        int nerdAiCount = 0;
        foreach(Card c in _playerHand.ToArray()){
            if(c.GetCardType() == _nerd.GetCardType()){
                nerdPlayerCount++;
            }
        }

        foreach(Card c in _aiHand.ToArray()){
            if(c.GetCardType() == _nerd.GetCardType()){
                nerdAiCount++;
            }
        }
        if(nerdPlayerCount >= 5){
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerWinScene");
            return true;
        }

        if(nerdAiCount >= 5){
            UnityEngine.SceneManagement.SceneManager.LoadScene("AiWinScene");
            return true;
        }
        return false;
        
    }
    //Card management functions
    void SortHand()
    {
        int i = 0;
        foreach(Card c in _playerHand)
        {
            c.transform.position = _cardSlotArray[i].position;
            i++;
        }
        UpdateUI();
    }

    void DrawFirstHand()
    {
            //give 3 cards to the player
            for(int i = 0; i < 3; i++)
            {
                //create a random card 
                var newCard = Instantiate(RandomCard(), gameObject.transform);
                //set it's pos to playerdeck
                _playerHand.Add(newCard);
            }
            //give 3 cards to the ai
            for(int i = 0; i < 3; i++){
                //have some sort of sprite that shows how many cards the ai has? we don't really need to instantiate them...
                var newCard = Instantiate(RandomCard(), _aiHandTransform);
                _aiHand.Add(newCard);
            }

            //then, proceed to let the player start first
            PlayerTurn();
            UpdateUI();
    }

    private void DrawCard()
    {
        if(_isPlayerTurn){
            if(_playerHand.Count != 7){
                _playerHand.Add(Instantiate(RandomCard(), gameObject.transform));
            } else {
                Debug.Log("Player Deck is full!");
            }
            SortHand();
        } else {
            if(_aiHand.Count != 7){
                _aiHand.Add(Instantiate(RandomCard(), _aiHandTransform));
            } else {
                Debug.Log("AI Deck is full!");
            }
        }
        UpdateUI();
    }

    private Card RandomCard()
    {
        int i = UnityEngine.Random.Range(0, 4);

        switch(i){
            case 0:
                return _popularity;
            case 1:
                return _bully;
            case 2:
                return _nerd;
            case 3: 
                return _locker;
            case 4:
                return _challenge;
            default:
                Debug.Log("Error! Drew invalid card!");
                return _popularity;
        }
    }

    public void ExpendCard(Card c){
        
        if(_isPlayerTurn){

            foreach(Card card in _playerHand.ToArray()){
                if(card == c){
                    _playerHand.Remove(card);
                }
            }
        } else {
            
            foreach(Card card in _aiHand.ToArray()){
                if(card == c){
                    _aiHand.Remove(card);
                }
            }
            Debug.Log("Completed AI Turn! (In expend function)");
        }

        NextTurn();
    }





    //card use functions
    public void UsePopularityCard(int value){

        if(_isPlayerTurn){
            _playerInfluence += value;
        } else {
            _aiInfluence += value;
        }
        
    }

    public void UseBullyCard(int value){
        

        if(_isPlayerTurn){
            var nerdCard = _aiHand.Find(x => x.GetCardType() == _nerd.GetCardType());
            if(nerdCard != null){
                _aiHand.Remove(nerdCard);
            } else{
                _aiInfluence -= value;
            }
        } else {
            var nerdCard = _playerHand.Find(x => x.GetCardType() == _nerd.GetCardType());
            if(nerdCard != null){
                _playerHand.Remove(nerdCard);
            } else{
                _playerInfluence -= value;
            }
        }
    }
    
    public void UseLockerCard(int value){

        for(int i = 0; i < value; i++){
            if(_isPlayerTurn){
                //check if target empty
                if(_aiHand.Count == 0){
                    Debug.Log("Target had no cards to steal!");
                    return;
                }
                //now steal
                var cardStolen = _aiHand[UnityEngine.Random.Range(0, _aiHand.Count)];
                _aiHand.Remove(cardStolen);
                _playerHand.Add(cardStolen);
            } else {
                //check if target empty
                if(_playerHand.Count == 0){
                    Debug.Log("Target had no cards to steal!");
                    return;
                }
                //now steal
                var cardStolen = _playerHand[UnityEngine.Random.Range(0, _playerHand.Count)];
                _playerHand.Remove(cardStolen);
                _aiHand.Add(cardStolen);
            }
        }
    }
    
    public void UseChallengeCard(int value){
        int playerNerdCount = 0;
        int aiNerdCount = 0;
        foreach(Card c in _playerHand){
            if(c.GetCardType() == _nerd.GetCardType()){
                playerNerdCount++;
            }
        }
        foreach(Card c in _aiHand){
            if(c.GetCardType() == _nerd.GetCardType()){
                aiNerdCount++;
            }
        }

        if(aiNerdCount > playerNerdCount){
            _playerInfluence -= value;
        } else if(aiNerdCount < playerNerdCount){
            _aiInfluence -= value;
        } else {
            Debug.Log("Challenge Tie!");
            return;
        }
    }
    
    //UI functions
    private void UpdateUI(){
        _aiCardText.text = "AI Cards: " + _aiHand.Count;
        _aiInfluenceText.text = "AI Influence: " + _aiInfluence;
        _playerInfluenceText.text = "Player Influence: " + _playerInfluence;
        if(_isPlayerTurn){
            _turnText.text = "It's your turn!";

        }else {
            _turnText.text = "It is the AI turn!";
        }
    }

}
