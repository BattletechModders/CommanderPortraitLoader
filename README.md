# CommanderPortraitLoader
Loads your custom pngs as a possible commander icon


Instructions:

1) Put your png into the C:\Program Files (x86)\Steam\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\sprites\Portraits folder
2) Go to C:\Program Files (x86)\Steam\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\data\portraits and edit one of the files to contain your png name without .png in the icon field and set isCommander to true.
  Example for guiTxrPort_Kara_utr.png:
  {
    "Description" : {
        "Id" : "PortraitPreset_1",
        "Name" : "PortraitPreset_1",
        "Details" : "",
        "Icon" : "**guiTxrPort_Kara_utr**"
    },
    "DataManager" : null,
    "backgroundColor" : {
        "r" : 0,
        "g" : 0,
        "b" : 0,
        "a" : 0
    },
    "headMesh" : 0.226,
    "hairMesh" : 2,
    "beardMesh" : 4,
    "suitMesh" : 0,
    "scar" : 8,
    "lightRig" : 2,
    "cameraPosition" : 1,
    "complexionTex" : 3,
    "eyebrowTex" : 2,
    "tattooTex" : 1,
    "actualTattooTex" : 3,
    "weights" : [
        20.26338,
        22.60093,
        14.9322,
        32.04418,
        5.584234,
        4.57507
    ],
    "browOffsetY" : -0.007,
    "browOffsetZ" : 0.00082,
    "noseBridgeScale" : -0.132,
    "noseTipScale" : 0.109,
    "chinScale" : 0.2662281,
    "cheekOffsetY" : 0.00387,
    "eyeWidth" : 0.0042,
    "jawWidth" : 0.066,
    "animation" : 2,
    "isCommander" : **true**,
    "skinToneId" : "Skin_05",
    "eyeColorId" : "Eye_03",
    "hairRootId" : "Hair_07",
    "hairTipId" : "Hair_08",
    "tattooColorId" : "Tattoo_04",
    "lipColorId" : "Lip_01"
}
