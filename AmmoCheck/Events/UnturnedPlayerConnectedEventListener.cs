using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Players.Connections.Events;
using SDG.Unturned;
using System.Threading.Tasks;

namespace AmmoCheck.Events
{
    public class UnturnedPlayerConnectedEventListener : IEventListener<UnturnedPlayerConnectedEvent>
    {
        private readonly IConfiguration configuration;
        public UnturnedPlayerConnectedEventListener(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task HandleEventAsync(object sender, UnturnedPlayerConnectedEvent @event)
        {
            if(configuration.GetSection("hideDefaultGunStatus").Get<bool>())
            {
                await UniTask.SwitchToMainThread();
                @event.Player.Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowUseableGunStatus);
            }
        }
    }
}
