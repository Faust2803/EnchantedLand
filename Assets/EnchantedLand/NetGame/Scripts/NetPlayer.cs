using Common.Scripts.Player;
using Unity.Netcode;
using Zenject;
namespace Tanks.NetGame.Scripts
{
    public class NetPlayer : NetworkBehaviour 
    {

        [Inject] public  PlayerManager _playerManager;

        
       
        

        public void PrepareForGame()
        {
            
            
            
        }
        
        

       
    }
}