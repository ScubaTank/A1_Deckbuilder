using UnityEngine;

public class ChallengeCard : Card
{

    public override void Use(){
        turnManager.UseChallengeCard(_myCardData.cardValue);
        Destroy(gameObject);
    }
}
