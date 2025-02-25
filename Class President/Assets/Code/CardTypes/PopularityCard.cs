using UnityEngine;

public class PopularityCard : Card
{

    public override void Use(){
        turnManager.UsePopularityCard(_myCardData.cardValue);
        Destroy(gameObject);
    }
}
