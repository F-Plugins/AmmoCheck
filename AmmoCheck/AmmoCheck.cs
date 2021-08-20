using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using System;

[assembly: PluginMetadata("Feli.AmmoCheck", DisplayName = "AmmoCheck")]
namespace AmmoCheck
{
    public class AmmoCheck : OpenModUnturnedPlugin
    {
        private readonly IStringLocalizer stringLocalizer;

        public AmmoCheck(
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            this.stringLocalizer = stringLocalizer;
        }

        protected override UniTask OnLoadAsync()
        {
            PlayerEquipment.OnInspectingUseable_Global += PlayerEquipment_OnInspectingUseable_Global;
            return UniTask.CompletedTask;
        }

        private void PlayerEquipment_OnInspectingUseable_Global(PlayerEquipment obj)
        {
            if(obj.useable is UseableGun)
            {
                string message = String.Empty;

                var magazineId = obj.state[8];

                if (magazineId != 0)
                {
                    var magazine = (ItemMagazineAsset)Assets.find(EAssetType.ITEM, magazineId);
                    var ammo = obj.state[10];

                    var ammoQuantity = (double)ammo / (double)magazine.amount;

                    if (ammoQuantity >= 0.9)
                    {
                        message = stringLocalizer["ammo:full"];
                    }
                    else if(ammoQuantity < 0.9 && ammoQuantity >= 0.8)
                    {
                        message = stringLocalizer["ammo:moreHalf"];
                    }
                    else if (ammoQuantity < 0.8 && ammoQuantity >= 0.5)
                    {
                        message = stringLocalizer["ammo:aboutHalf"];
                    }
                    else if(ammoQuantity < 0.5 && ammoQuantity >= 0.35)
                    {
                        message = stringLocalizer["ammo:lessHalf"];
                    }
                    else if(ammoQuantity < 0.35 && ammoQuantity >= 0.1)
                    {
                        message = stringLocalizer["ammo:almostEmpty"];
                    }
                    else
                    {
                        message = stringLocalizer["ammo:empty"];
                    }
                }
                else
                    message = stringLocalizer["ammo:empty"];

                EffectManager.sendUIEffect(2355, 333, obj.player.channel.owner.transportConnection, true, message);
            }
        }

        protected override UniTask OnUnloadAsync()
        {
            PlayerEquipment.OnInspectingUseable_Global -= PlayerEquipment_OnInspectingUseable_Global;
            return UniTask.CompletedTask;
        }
    }
   
}
