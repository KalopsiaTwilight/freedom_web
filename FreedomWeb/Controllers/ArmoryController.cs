using FreedomLogic.DAL;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Armory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    [AllowAnonymous]
    public class ArmoryController : FreedomController
    {
        private readonly DbCharacters _charactersDb;
        private readonly DbDbc _dbcDb;
        private readonly CharacterManager _characterManager;
        
        public ArmoryController(DbCharacters charactersDb, CharacterManager characterManager, DbDbc dbcDb)
        {
            _charactersDb = charactersDb;
            _characterManager = characterManager;
            _dbcDb = dbcDb;
        }


        [HttpGet]
        public async Task<ActionResult> Character([FromRoute] int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ArmoryCharacterViewModel();

            // Retrieve character and customizations;
            var character = _characterManager.GetCharacterById(id.Value);
            var customizations = await _charactersDb.CharacterCustomizations
                .Where(x => x.CharacterId == id.Value)
                .ToListAsync();

            // Retrieve TrinityCore Equipment Data
            var items = await _charactersDb.CharacterInventorySlots
                .Where(x => x.CharacterId == id.Value && x.Bag == 0 && x.Slot < 19)
                .Select(x => new
                {
                    x.Slot,
                    x.ItemInstance,
                    x.ItemInstance.Transmog
                })
                .ToListAsync();

            // Get Item Modified Appearance data needd
            var itemIds = items.Select(x => x.ItemInstance.ItemId).ToList();
            var itemModifiedAppearanceIds = items
                .SelectMany(x => new int?[] { x.ItemInstance.Transmog?.ItemModifiedAppearanceAllSpecs, x.ItemInstance?.Transmog?.SecondaryItemModifiedAppearanceAllSpecs })
                .Where(x => x.HasValue && x.Value > 0)
                .Select(x => x.Value)
                .ToList();

            var itemModifiedAppearances = await _dbcDb.ItemModifiedAppearances
                .Where(i => itemIds.Contains(i.ItemID))
                .Union(
                    _dbcDb.ItemModifiedAppearances.Where(ima => itemModifiedAppearanceIds.Contains(ima.ID))
                )
                .Select(x => new
                {
                    x.ID,
                    x.ItemID,
                    x.ItemAppearanceModifierID,
                    x.ItemAppearance.ItemDisplayInfoID
                })
                .ToListAsync();

            // Get Item Bonuses needed for appearance determination
            var bonusIds = items.SelectMany(x => x.ItemInstance.BonusListIds.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse)).ToList();
            var bonusAppearanceSelectors = await _dbcDb.ItemBonuses
                .Where(ib => ib.Type == 7 && bonusIds.Contains(ib.ParentItemBonusListID))
                .ToListAsync();

            // Get Enchantments referenced
            var enchantIds = items.SelectMany(x => x.ItemInstance.Enchantments.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse))
                .Where(x => x > 0).ToList();
            enchantIds.AddRange(items.Select(x => x.ItemInstance.Transmog?.SpellItemEnchantmentAllSpecs)
                .Where(x => x.HasValue && x.Value > 0)
                .Select(x => x.Value)
            );

            var enchantData = _dbcDb.SpellItemEnchantments.Where(x => enchantIds.Contains(x.ID));

            // Determine Item Display Ids
            var itemsView = new List<CharacterItemViewModel>();
            foreach(var item in items)
            {
                var itemId = item.ItemInstance.ItemId;

                int displayId1 = -1;
                int displayId2 = -1;
                int itemVisual = 0;

                var enchantmentStr = item.ItemInstance.Enchantments.Split(" ").FirstOrDefault();
                if (int.TryParse(enchantmentStr, out var enchantId))
                {
                    itemVisual = enchantData.FirstOrDefault(x => x.ID == enchantId)?.ItemVisual ?? 0;
                }

                // Not Transmogged
                if (item.ItemInstance.Transmog == null)
                {
                    var appearance = itemModifiedAppearances.FirstOrDefault(x => x.ItemID == item.ItemInstance.ItemId);
                    if (!string.IsNullOrEmpty(item.ItemInstance.BonusListIds))
                    {
                        var bonusListIds = item.ItemInstance.BonusListIds.Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse);
                        var appearanceModBonus = bonusAppearanceSelectors.FirstOrDefault(ib => bonusListIds.Contains(ib.ParentItemBonusListID));
                        if (appearanceModBonus != null)
                        {
                            appearance = itemModifiedAppearances.FirstOrDefault(x => x.ItemID == item.ItemInstance.ItemId && x.ItemAppearanceModifierID == appearanceModBonus.Value0);
                        }
                    }
                    if (appearance == null)
                    {
                        continue;
                    }
                    displayId1 = appearance.ItemDisplayInfoID;
                } 
                else
                {
                    var transmog = item.ItemInstance.Transmog;
                    var appearance = itemModifiedAppearances.FirstOrDefault(x => x.ID == transmog.ItemModifiedAppearanceAllSpecs);
                    displayId1 = appearance?.ItemDisplayInfoID ?? 0;
                    if (transmog.SecondaryItemModifiedAppearanceAllSpecs > 0)
                    {
                        var appearance2 = itemModifiedAppearances.FirstOrDefault(x => x.ID == transmog.SecondaryItemModifiedAppearanceAllSpecs);
                        displayId2 = appearance2?.ItemDisplayInfoID ?? 0;
                    }
                    if (transmog.SpellItemEnchantmentAllSpecs > 0)
                    {
                        itemVisual = enchantData.FirstOrDefault(x => x.ID == transmog.SpellItemEnchantmentAllSpecs)?.ItemVisual ?? 0;
                    }
                }

                itemsView.Add(new CharacterItemViewModel()
                {
                    DisplayId = displayId1,
                    DisplayId2 = displayId2,
                    Slot = item.Slot,
                    ItemVisual = itemVisual
                });
            }

            model = new ArmoryCharacterViewModel() { 
                CharacterId = character.Id,
                Class = (int)character.Class,
                Gender = (int)character.Gender,
                Race = (int)character.Race,
                Name = character.Name,
                CustomizationOptions = customizations.Select(x => new CharacterCustomizationOptionViewModel()
                {
                    OptionId = x.CustomizationOptionId,
                    ChoiceId = x.CustomizationChoiceId
                }).ToList(),
                Items = itemsView
            };


            return View(model);
        }
    }
}