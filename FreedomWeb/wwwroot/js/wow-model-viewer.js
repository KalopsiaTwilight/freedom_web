if (!window.CONTENT_PATH) {
    window.CONTENT_PATH = `/modelviewer/live/`
}
if (!window.WOTLK_TO_RETAIL_DISPLAY_ID_API) {
    window.WOTLK_TO_RETAIL_DISPLAY_ID_API = `https://wotlk.murlocvillage.com/api/items`
}


if (!window.WH) {
    window.WH = {}
    window.WH.debug = console.log;
    window.WH.defaultAnimation = `Stand`
    window.WH.Wow = {
        ItemFeatureFlags: {
            1: "Emblazoned Tabard",
            2: "No sheathed kit during spell combat anims",
            4: "Hide Pants and Belt",
            8: "Emblazoned Tabard (Rare)",
            16: "Emblazoned Tabard (Epic)",
            32: "Use Spear Ranged Weapon Attachment",
            64: "Inherit character animation",
            128: "Mirror Animation from Right Shoulder to Left",
            256: "Mirror Model When Equipped on Off-Hand",
            512: "Disable Tabard Geo (waist only)",
            1024: "Mirror Model When Equipped on Main -Hand",
            2048: "Mirror Model When Sheathed (Warglaives)",
            4096: "Flip Model When Sheathed",
            8192: "Use Alternate Weapon Trail Endpoint",
            16384: "Force Sheathed if equipped as weapon",
            32768: "Don't close hands",
            65536: "Force Unsheathed for Spell Combat Anims",
            131072: "Brewmaster Unsheathe",
            262144: "Hide Belt Buckle",
            524288: "No Default Bowstring",
            1048576: "Unknown Effect 1",
            2097152: "Unknown Effect 2",
            4194304: "Unknown Effect 3",
            8388608: "Unknown Effect 4",
            16777216: "Unknown Effect 5",
            33554432: "Unknown Effect 6",
            67108864: "Unknown Effect 7",
            134217728: "Unknown Effect 8",
        },
        Item: {
            INVENTORY_TYPE_HEAD: 1,
            INVENTORY_TYPE_NECK: 2,
            INVENTORY_TYPE_SHOULDERS: 3,
            INVENTORY_TYPE_SHIRT: 4,
            INVENTORY_TYPE_CHEST: 5,
            INVENTORY_TYPE_WAIST: 6,
            INVENTORY_TYPE_LEGS: 7,
            INVENTORY_TYPE_FEET: 8,
            INVENTORY_TYPE_WRISTS: 9,
            INVENTORY_TYPE_HANDS: 10,
            INVENTORY_TYPE_FINGER: 11,
            INVENTORY_TYPE_TRINKET: 12,
            INVENTORY_TYPE_ONE_HAND: 13,
            INVENTORY_TYPE_SHIELD: 14,
            INVENTORY_TYPE_RANGED: 15,
            INVENTORY_TYPE_BACK: 16,
            INVENTORY_TYPE_TWO_HAND: 17,
            INVENTORY_TYPE_BAG: 18,
            INVENTORY_TYPE_TABARD: 19,
            INVENTORY_TYPE_ROBE: 20,
            INVENTORY_TYPE_MAIN_HAND: 21,
            INVENTORY_TYPE_OFF_HAND: 22,
            INVENTORY_TYPE_HELD_IN_OFF_HAND: 23,
            INVENTORY_TYPE_PROJECTILE: 24,
            INVENTORY_TYPE_THROWN: 25,
            INVENTORY_TYPE_RANGED_RIGHT: 26,
            INVENTORY_TYPE_QUIVER: 27,
            INVENTORY_TYPE_RELIC: 28,
            INVENTORY_TYPE_PROFESSION_TOOL: 29,
            INVENTORY_TYPE_PROFESSION_ACCESSORY: 30
        },
        ComponentSections: {
            0: "Upper Arm",
            1: "Lower Arm",
            2: "Hand",
            3: "Upper Torso",
            4: "Lower Torso",
            5: "Upper Leg",
            6: "Lower Leg",
            7: "Foot",
            8: "Accesory",
            12: "Cloak"
        },
        GeoSets: {
            0: {
                title: "Head Geoset",
                option: []
            },
            1: {
                title: "Beard / Facial1 Geoset",
                option: []
            },
            2: {
                title: "Sideburns / Facial2 Geoset",
                option: []
            },
            3: {
                title: "Moustache / Facial3 Geoset",
                option: []
            },
            4: {
                title: "Gloves Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    },
                    {
                        name: "Thin",
                        value: 1
                    },
                    {
                        name: "Folded",
                        value: 2
                    },
                    {
                        name: "Thick",
                        value: 3
                    },
                ]
            },
            5: {
                title: "Boots Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    },
                    {
                        name: "High Boot",
                        value: 1
                    },
                    {
                        name: "Folded Boot",
                        value: 2
                    },
                    {
                        name: "Puffed",
                        value: 3
                    },
                    {
                        name: "Boot 4",
                        value: 4
                    },
                ]
            },
            6: {
                title: "Shirt Geoset",
                option: []
            },
            7: {
                title: "Ears Geoset",
                option: []
            },
            8: {
                title: "Sleeves Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: 0
                    },
                    {
                        name: "Flared Sleeve",
                        value: 1
                    },
                    {
                        name: "Puffy Sleeve",
                        value: 2
                    },
                    {
                        name: "Panda Collar Shirt",
                        value: 3
                    },
                ]
            },
            9: {
                title: "Legcuffs Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: 0
                    },
                    {
                        name: "Flared",
                        value: 1
                    },
                    {
                        name: "Ruffled",
                        value: 2
                    },
                    {
                        name: "Panda Pants",
                        value: 3
                    },
                ]
            },
            10: {
                title: "Shirt Doublet Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: 0
                    },
                    {
                        name: "Doublet",
                        value: 1
                    },
                    {
                        name: "Body 2",
                        value: 2
                    },
                    {
                        name: "Body 3",
                        value: 3
                    },
                ]
            },
            11: {
                title: "Pant Doublet Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: 0
                    },
                    {
                        name: "Mini Skirt",
                        value: 1
                    },
                    {
                        name: "Armored Pants",
                        value: 3
                    }
                ]
            },
            12: {
                title: "Tabard Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: 0
                    },
                    {
                        name: "Tabard",
                        value: 1
                    }
                ]
            },
            13: {
                title: "Lower Body Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    },
                    {
                        name: "Long Skirt",
                        value: 1
                    },
                ]
            },
            14: {
                title: "DH/Pandaren F Loincloth Geoset",
                option: []
            },
            15: {
                title: "Cloak Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: 0
                    },
                    {
                        name: "Ankle Length",
                        value: 1
                    },
                    {
                        name: "Knee Length",
                        value: 2
                    },
                    {
                        name: "Split Banner",
                        value: 3
                    },
                    {
                        name: "Tapered Waist",
                        value: 4
                    },
                    {
                        name: "Notched Back",
                        value: 5
                    },
                    {
                        name: "Guild Cloak",
                        value: 6
                    },
                    {
                        name: "Split (Long)",
                        value: 7
                    },
                    {
                        name: "Tapered (Long)",
                        value: 8
                    },
                    {
                        name: "Notched (Long)",
                        value: 9
                    },
                ]
            },
            16: {
                title: "Facial Jewelry Geoset",
                option: []
            },
            17: {
                title: "Eye Effects Geoset",
                option: []
            },
            18: {
                title: "Belt Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    },
                    {
                        name: "Heavy Belt",
                        value: 1
                    },
                    {
                        name: "Panda Cord Belt",
                        value: 2
                    }
                ]
            },
            19: {
                title: "Skin (Bone/Tail) Geoset",
                option: []
            },
            20: {
                title: "Feet Geoset",
                options: [
                    {
                        name: "Toes",
                        value: 0
                    },
                    {
                        name: "Basic Shoes",
                        value: 1
                    },
                ]
            },
            21: {
                title: "Head Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Show Head",
                        value: 0
                    }
                ]
            },
            22: {
                title: "Torso Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    },
                    {
                        name: "Covered Torso",
                        value: 1
                    }
                ]
            },
            23: {
                title: "Hand Attachments Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    }
                ]
            },
            24: {
                title: "Head Attachments Geoset",
                option: []
            },
            25: {
                title: "Facewear Geoset",
                option: []
            },
            26: {
                title: "Shoulders Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Show Shoulders",
                        value: 0
                    },
                    {
                        name: "Non-Mythic Only",
                        value: 1
                    },
                    {
                        name: "Mythic",
                        value: 2
                    }
                ]
            },
            27: {
                title: "Helmet Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Helm 1",
                        value: 0
                    },
                    {
                        name: "Non-Mythic only",
                        value: 2
                    },
                    {
                        name: "Mythic",
                        value: 3
                    },
                ]
            },
            28: {
                title: "Arm Upper Geoset",
                options: [
                    {
                        name: "No Geoset",
                        value: -1
                    },
                    {
                        name: "Default",
                        value: 0
                    }
                ]
            },
            29: {
                title: "Arms Replace Geoset",
                option: []
            },
            30: {
                title: "Legs Replace Geoset",
                option: []
            },
            31: {
                title: "Feet Replace Geoset",
                option: []
            },
            32: {
                title: "Head SwapGeoset",
                option: []
            },
            33: {
                title: "Eyes Geoset",
                option: []
            },
            34: {
                title: "Eyebrows Geoset",
                option: []
            },
            35: {
                title: "Piercings/Earrings Geoset",
                option: []
            },
            36: {
                title: "Necklaces Geoset",
                option: []
            },
            37: {
                title: "Headdress Geoset",
                option: []
            },
            38: {
                title: "Tail Geoset",
                option: []
            },
            39: {
                title: "Misc. Accessory Geoset",
                option: []
            },
            40: {
                title: "Misc. Feature Geoset",
                option: []
            },
            41: {
                title: "Noses (Goblins) Geoset",
                option: []
            },
            42: {
                title: "Hair decoration (LF Draenei) Geoset",
                option: []
            },
            43: {
                title: "Horn decoration (HM Tauren) Geoset",
                option: []
            },
        }
    }
}
const WH = window.WH

const NOT_DISPLAYED_SLOTS = [
    2, // neck
    11, // finger1
    12, // finger1
    13, // trinket1
    14, // trinket2
]

const CHARACTER_PART = {
    Face: `face`,
    "Skin Color": `skin`,
    "Hair Style": `hairStyle`,
    "Hair Color": `hairColor`,
    "Facial Hair": `facialStyle`,
    Mustache: `facialStyle`,
    Beard: `facialStyle`,
    Sideburns: `facialStyle`,
    "Face Shape": `facialStyle`,
    Eyebrow: `facialStyle`,
    "Jaw Features": undefined,
    "Face Features": undefined,
    "Skin Type": undefined,
    Ears: undefined,
    Horns: undefined,
    Blindfold: undefined,
    Tattoo: undefined,
    "Eye Color": undefined,
    "Tattoo Color": undefined,
    Armbands: undefined,
    "Jewelry Color": undefined,
    Bracelets: undefined,
    Necklace: undefined,
    Earring: undefined
}


function optionalChaining(choice) {
    //todo replace by `part.Choices[character[CHARACTER_PART[prop]]]?.Id` when it works on almost all frameworks
    return choice ? choice.Id : undefined
}

/**
 *
 * @param {Object} character - The character object.
 * @param {number} character.face - Description for face.
 * @param {number} character.facialStyle - Description for facialStyle.
 * @param {number} character.gender - Description for gender.
 * @param {number} character.hairColor - Description for hairColor.
 * @param {number} character.hairStyle - Description for hairStyle.
 * @param {Array<Array<number>>} character.items - Description for items. (Optional)
 * @param {number} character.race - Description for race.
 * @param {number} character.skin - Description for skin.
 * @param {Object} fullOptions - Zaming API character options payload.
 * @return {[]}
 */
function getCharacterOptions(character, fullOptions) {
    const options = fullOptions.Options
    const ret = []
    for (const prop in CHARACTER_PART) {
        const part = options.find(e => e.Name === prop)

        if (!part) {
            continue
        }

        const newOption = {
            optionId: part.Id,
            choiceId: (CHARACTER_PART[prop])
                ? optionalChaining(part.Choices[character[CHARACTER_PART[prop]]])
                : character[prop] ? part.Choices[character[prop]] : part.Choices[0].Id
        }
        ret.push(newOption)
    }

    return ret
}

/**
 * This function return the design choices for a character this does not work for NPC / Creature / Items
 * @param {Object} model - The model object to generate options from.
 * @param {{}} fullOptions - The type of the model.
 * @returns {{models: {id: string, type: number}, charCustomization: {options: []}, items: (*|*[])}|{models: {id, type}}
 */
function optionsFromModel(model, fullOptions) {
    const { race, gender } = model


    // slot ids on model viewer
    const characterItems = (model.items) ? model.items.filter(e => !NOT_DISPLAYED_SLOTS.includes(e[0])) : []
    const options = getCharacterOptions(model, fullOptions)


    return {
        items: characterItems,
        charCustomization: {
            options: options
        },
        models: {
            id: race * 2 - 1 + gender,
            type: 16
        },
    }
}


/**
 *
 * @param item{number}: Item id
 * @param slot{number}: Item slot number
 * @param displayId{number}: DisplayId of the item
 * @return {Promise<boolean|*>}
 */
async function getDisplaySlot(item, slot, displayId) {
    if (typeof item !== `number`) {
        throw new Error(`item must be a number`)
    }

    if (typeof slot !== `number`) {
        throw new Error(`slot must be a number`)
    }

    if (typeof displayId !== `number`) {
        throw new Error(`displayId must be a number`)
    }

    try {
        await fetch(`${window.CONTENT_PATH}meta/armor/${slot}/${displayId}.json`)
            .then(response => response.json())

        return {
            displaySlot: slot,
            displayId: displayId
        }
    } catch (e) {
        if (!window.WOTLK_TO_RETAIL_DISPLAY_ID_API) {
            throw Error(`Item not found and window.WOTLK_TO_RETAIL_DISPLAY_ID_API not set`)
        }
        const resp = await fetch(`${window.WOTLK_TO_RETAIL_DISPLAY_ID_API}/${item}/${displayId}`)
            .then((response) => response.json())
        const res = resp.data || resp
        if (res.newDisplayId !== displayId) {
            return {
                displaySlot: slot,
                displayId: res.newDisplayId
            }
        }
    }

    // old slots to new slots
    const retSlot = {
        5: 20, // chest
        16: 21, // main hand
        18: 22 // off hand
    }[slot]

    if (!retSlot) {
        console.warn(`Item: ${item} display: ${displayId} or slot: ${slot} not found for `)

        return {
            displaySlot: slot,
            displayId: displayId
        }
    }

    return {
        displaySlot: retSlot,
        displayId: displayId
    }
}


/**
 * Returns a 2-dimensional list the inner list contains on first position the item slot, the second the item
 * display-id ex: [[1,1170],[3,4925]]
 * @param {*[{item: {entry: number, displayid: number}, transmog: {entry: number, displayid: number}, slot: number}]} equipments
 * @returns {Promise<number[]>}
 */
async function findItemsInEquipments(equipments) {
    for (const equipment of equipments) {
        if (NOT_DISPLAYED_SLOTS.includes(equipment.slot)) {
            continue
        }

        const displayedItem = (Object.keys(equipment.transmog).length !== 0) ? equipment.transmog : equipment.item
        const displaySlot = await getDisplaySlot(
            displayedItem.entry,
            equipment.slot,
            displayedItem.displayid
        )
        equipment.displaySlot = displaySlot.displaySlot
        equipment.displayId = displaySlot.displayId
        Object.assign(displaySlot, equipment)
    }
    return equipments
        .filter(e => e.displaySlot)
        .map(e => [
            e.displaySlot,
            e.displayId
        ]
        )
}


/**
 *
 * @param {number} race
 * @param {number} gender
 * @returns {Promise<Object>}
 */
async function findRaceGenderOptions(race, gender) {
    const raceGender = race * 2 - 1 + gender
    const options = await fetch(`${window.CONTENT_PATH}meta/charactercustomization/${raceGender}.json`)
        .then(
            (response) => response.json()
        )
    if (options.data) {
        return options.data
    }

    return options
}
class WowModelViewer extends ZamModelViewer {
    /**
     * Returns the list of animation names
     * @returns {Array.<string>}
     */
    getListAnimations() {
        return [...new Set(this.renderer.models[0].ap.map(e => e.j))]
    }

    /**
     * Change character distance
     * @param {number} val
     */
    setDistance(val) {
        this.renderer.distance = val
    }

    /**
     * Change the animation
     * @param {string} val
     */
    setAnimation(val) {
        if (!this.getListAnimations().includes(val)) {
            console.warn(`${this.constructor.name}: Animation ${val} not found`)
        }
        this.renderer.models[0].setAnimation(val)
    }

    /**
     * Play / Pause the animation
     * @param {boolean} val
     */
    setAnimPaused(val) {
        this.renderer.models[0].setAnimPaused(val)
    }

    /**
     * Set azimuth value this value is the angle to the azimuth based on PI
     * @param {number} val
     */
    setAzimuth(val) {
        this.renderer.azimuth = val
    }

    /**
     * Set zenith value this value is the angle to the azimuth based on PI
     * @param {number} val
     */
    setZenith(val) {
        this.renderer.zenith = val
    }

    /**
     * Returns azimuth value this value is the angle to the azimuth based on PI
     * @return {number}
     */
    getAzimuth() {
        return this.renderer.azimuth
    }

    /**
     * Returns zenith value this value is the angle to the azimuth based on PI
     * @return {number}
     */
    getZenith() {
        return this.renderer.zenith
    }

    /**
     * This methode is based on `updateViewer` from Paperdoll.js (https://wow.zamimg.com/js/Paperdoll.js?3ee7ec5121)
     *
     * @param slot {number}: Item slot number
     * @param displayId {number}: Item display id
     * @param enchant {number}: Enchant (experimental not tested)
     */
    updateItemViewer(slot, displayId, enchant) {
        const s = window.WH.Wow.Item
        if (slot === s.INVENTORY_TYPE_SHOULDERS) {
            // this.method(`setShouldersOverride`, [this.getShouldersOverrideData()]);
        }
        const a = (slot === s.INVENTORY_TYPE_ROBE) ? s.INVENTORY_TYPE_CHEST : slot

        window.WH.debug(`Clearing model viewer slot:`, a.toString())
        this.method(`clearSlots`, slot.toString())
        if (displayId) {
            window.WH.debug(`Attaching to model viewer slot:`, slot.toString(), `Display ID:`, displayId, `Enchant Visual:`, enchant)
            this.method(`setItems`, [[{
                slot: slot,
                display: displayId,
                visual: enchant || 0
            }]])
        }
    }

    setCustomItem(slot, itemData) {
        const s = window.WH.Wow.Item
        if (slot === s.INVENTORY_TYPE_SHOULDERS) {
            // this.method(`setShouldersOverride`, [this.getShouldersOverrideData()]);
        }
        const a = (slot === s.INVENTORY_TYPE_ROBE) ? s.INVENTORY_TYPE_CHEST : slot
        
        this.method(`clearSlots`, "1,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,19,20,21,22,23,27")
        this.method(`setCustomItem`, [a, itemData]);
    }

    setNewAppearance(options) {
        if (!this.currentCharacterOptions) {
            throw Error(`Character options are not set`)
        }
        const characterOptions = getCharacterOptions(options, this.currentCharacterOptions)
        const race = this.characterRace
        const gender = this.characterGender
        this.method(`setAppearance`, { race: race, gender: gender, options: characterOptions })
    }
}

// Instance variables
WowModelViewer.prototype._currentCharacterOptions = 0
WowModelViewer.prototype._characterGender = null
WowModelViewer.prototype._characterRace = null

// Getter and Setter for currentCharacterOptions
Object.defineProperty(WowModelViewer.prototype, `currentCharacterOptions`, {
    get: function () {
        return this._currentCharacterOptions
    },
    set: function (value) {
        this._currentCharacterOptions = value
    }
})

// Getter and Setter for characterGender
Object.defineProperty(WowModelViewer.prototype, `characterGender`, {
    get: function () {
        return this._characterGender
    },
    set: function (value) {
        this._characterGender = value
    }
})

// Getter and Setter for characterRace
Object.defineProperty(WowModelViewer.prototype, `characterRace`, {
    get: function () {
        return this._characterRace
    },
    set: function (value) {
        this._characterRace = value
    }
})

/**
 *
 * @param aspect {number}: Size of the character
 * @param containerSelector {string}: jQuery selector on the container
 * @param model {{}|{id: number, type: number}}: A json representation of a character
 * @returns {Promise<WowModelViewer>}
 */
async function generateModels(aspect, containerSelector, model) {
    let modelOptions
    let fullOptions
    if (model.id && model.type) {
        const { id, type } = model
        modelOptions = { models: { id, type } }
    } else {
        const { race, gender } = model

        // CHARACTER OPTIONS
        // This is how we describe a character properties
        fullOptions = await findRaceGenderOptions(
            race,
            gender
        )
        modelOptions = optionsFromModel(model, fullOptions)
    }
    const models = {
        type: 2,
        contentPath: window.CONTENT_PATH,
        // eslint-disable-next-line no-undef
        container: jQuery(containerSelector),
        aspect: aspect,
        hd: true,
        ...modelOptions
    }
    window.models = models

    // eslint-disable-next-line no-undef
    const wowModelViewer = await new WowModelViewer(models)
    if (fullOptions) {
        wowModelViewer.currentCharacterOptions = fullOptions
        wowModelViewer.characterGender = model.gender
        wowModelViewer.characterRace = model.race

    }
    return wowModelViewer
}

function getGeoSetsForInventoryType(inventoryType) {
    switch (inventoryType) {
        case WH.Wow.Item.INVENTORY_TYPE_HEAD: return [27, 21];
        case WH.Wow.Item.INVENTORY_TYPE_SHOULDERS: return [26];
        case WH.Wow.Item.INVENTORY_TYPE_SHIRT: return [8, 10];
        case WH.Wow.Item.INVENTORY_TYPE_CHEST: return [8, 10, 13, 22, 28];
        case WH.Wow.Item.INVENTORY_TYPE_WAIST: return [18];
        case WH.Wow.Item.INVENTORY_TYPE_LEGS: return [11, 9, 13];
        case WH.Wow.Item.INVENTORY_TYPE_FEET: return [5, 20];
        case WH.Wow.Item.INVENTORY_TYPE_HANDS: return [4, 23];
        case WH.Wow.Item.INVENTORY_TYPE_BACK: return [15];
        case WH.Wow.Item.INVENTORY_TYPE_TABARD: return [12];
        default: return [];
    }
}

function getComponentSectionsForInventoryType(inventoryType) {
    switch (inventoryType) {
        //case WH.Wow.Item.INVENTORY_TYPE_HEAD: return [1,2];
        case WH.Wow.Item.INVENTORY_TYPE_SHIRT: return [0,1,2,3,4,5,6];
        case WH.Wow.Item.INVENTORY_TYPE_CHEST: return [0,1,2,3,4,5,6];
        case WH.Wow.Item.INVENTORY_TYPE_WAIST: return [4,5];
        case WH.Wow.Item.INVENTORY_TYPE_LEGS: return [5,6,8];
        case WH.Wow.Item.INVENTORY_TYPE_FEET: return [6, 7];
        case WH.Wow.Item.INVENTORY_TYPE_WRISTS: return [1];
        case WH.Wow.Item.INVENTORY_TYPE_HANDS: return [1,2];
        case WH.Wow.Item.INVENTORY_TYPE_TABARD: return [3, 4, 5];
        case WH.Wow.Item.INVENTORY_TYPE_ROBE: return [1, 3, 4, 5, 6];
        case WH.Wow.Item.INVENTORY_TYPE_BACK: return [12];
        default: return [];
    }
}

function getRaceName(race) {
    switch (race) {
        case 0: return "All";
        case 1: return "Human";
        case 2: return "Orc";
        case 3: return "Dwarf";
        case 4: return "Night Elf";
        case 5: return "Undead";
        case 6: return "Tauren";
        case 7: return "Gnome";
        case 8: return "Troll";
        case 9: return "Goblin";
        case 10: return "Blood Elf";
        case 11: return "Draenei";
        case 22: return "Worgen";
        case 23: return "Worgen (Human form)";
        case 24: return "Pandaren (N)";
        case 25: return "Pandaren (A)";
        case 26: return "Pandaren (H)";
        case 27: return "Nightborne";
        case 28: return "Highmountain Tauren";
        case 29: return "Void Elf";
        case 30: return "Lightforged Draenei";
        case 31: return "Zandalari Troll";
        case 32: return "Kul Tiran";
        case 34: return "Dark Iron Dwarf";
        case 35: return "Vulpera";
        case 36: return "Mag'har Orc";
        case 37: return "Mechagnome";
        default: return "Unknown";
    }
}

function getClassName(classId) {
    switch (classId) {
        case 0: return "All";
        case 1: return "Warrior";
        case 2: return "Paladin";
        case 3: return "Hunter";
        case 4: return "Rogue";
        case 5: return "Priest";
        case 6: return "Death Knight";
        case 7: return "Shaman";
        case 8: return "Mage";
        case 9: return "Warlock";
        case 10: return "Monk";
        case 11: return "Druid";
        case 12: return "Demon Hunter";
        default: return "Unknown";
    }
}

function intToByteArray(input) {
    var byteArray = [0, 0, 0, 0];
    for (var i = 0; i < byteArray.length; i++) {
        var byte = input & 0xff;
        byteArray[i] = byte;
        input = (input - byte) / 256;
    }
    return byteArray;
};

function getColorStringFromNumber(input) {
    const [b,g,r,a] = intToByteArray(input);

    return `rgba(${r},${g},${b},${(a/255).toFixed(2)})`
}

function byteArrayToInt(byteArray) {
    var value = 0;
    for (var i = byteArray.length - 1; i >= 0; i--) {
        value = (value * 256) + byteArray[i];
    }
    return value;
};

function rgbaToInt(r, g, b, a) {
    return byteArrayToInt([b, g, r, a]);
}

function byteToHexCode(num) {
    let code = num.toString(16);
    if (code.length === 1) {
        return '0' + code;
    } else {
        return code;
    }
}
