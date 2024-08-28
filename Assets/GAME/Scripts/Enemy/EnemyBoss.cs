using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private GameObject canisterRef;

    private bool debugKill = false;
    void Start()
    {
        if (!StageHandler.Instance.IsOnPlayerLayer(transform.position.z))
        {
            debugKill = true;
            CombatHandler.Instance.AdvanceTimestamp(transform.position.z);
            Destroy(gameObject);
            return;
        }
        AudioHandler.Instance.PlaySong(AudioHandler.Song.Boss, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Player.Instance.SetCanFly(false);
        CombatHandler.Instance.Pause();
    }
    void OnDisable()
    {
        //If killed by the GC or the game isn't running (coroutines do that sometimes)
        if(!this.gameObject.scene.isLoaded || debugKill) return;

        //EXPLODE
        CombatHandler.Instance.SpawnExplosion(CombatHandler.ExplosionType.big, transform.position);

        //Win the game if it's the space boss!
        if (StageHandler.Instance.currentLayer == StageHandler.GameLayer.Space)
        {
            StageHandler.Instance.Win();
            return;
        }

        //On Death
        GameObject canister = Instantiate(canisterRef);
        canister.transform.position = transform.position;

        //Resume stage music
        AudioHandler.Instance.PlayStageMusic(StageHandler.Instance.currentLayer);

        //legacy
        Player.Instance.SetCanFly(true);

        //Resume combat and reset all level spawn timers (basically play the stage again)
        CombatHandler.Instance.UnPause();
        CombatHandler.Instance.AdvanceTimestamp(0);
        CombatHandler.Instance.AdvanceTimestamp(-20f);
        CombatHandler.Instance.AdvanceTimestamp(-200f);

        
    }
}
