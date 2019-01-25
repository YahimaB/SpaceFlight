using UnityEngine;

public class BoundaryTouch : MonoBehaviour {
    public GameController gameController;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            gameController.AddScore(false, 10);
        }
        Destroy(other.gameObject);
    }
}
