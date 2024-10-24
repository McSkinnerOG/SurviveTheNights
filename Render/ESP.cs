using System;
using System.Linq;
using NDraw;
using UnityEngine;

namespace SurviveTheNights.Render
{
  public class ESP
  {
    public static bool Enabled = false;
    public static bool AdvancedOptions = false;
    public static float AdvancedOptionsDistance = 5f;
    public static float AdvancedOptionsRadius = 100f;
    #region ESP TYPES
    public static bool EspAnimal = false;
    public static bool EspItem = true;
    public static bool EspPlayer = true;
    public static bool EspStorage = false;
    public static bool EspVehicle = false;
    public static bool EspWeapon = true;
    public static bool EspAmmo = true;
    public static bool EspTools = true;
    public static bool EspTraps = true;
    public static bool EspZombie = false;
    #endregion
    #region ESP OPTIONS 
    public static bool Draw3DBox = true;
    public static bool DrawTracers = true;
    public static bool DrawDistance = true;
    public static bool DrawPosition = true;
    public static bool DrawFacingDirection = true;
    public static float EspDistanceItems = 300;
    public static float EspDistanceWeapons = 500;
    public static float EspDistanceVehicles = 2000f;
    public static float EspDistanceZombies = 500f;
    public static float EspDistancePlayers = 5000f;
    public static float EspDistanceAnimals = 1000f;
    public static float Distance = 0f;
    public static float Radius = 180f;
    #endregion
    public static void RenderStorageESP()
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      foreach(var fom in FixedObjects.instance.fixedObjects.Values.Where(g => g.identifier.Contains("container") && g.attachedObjectsCount > 0))
      {
        Distance = Vector3.Distance(Refs.LP_Position, fom.gameObject.transform.position);
        var w2s = Camera.main.WorldToScreenPoint(fom.transform.position);
        if(w2s.z > 0 && Distance < EspDistanceItems)
        {
          var sradius = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
          var attachedObjects = 0;
          var text = $"            || STORAGE";
          if(sradius < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
          {
            text =
            $"                                 {Convert.ToInt32(Distance)}M\n" +
            $"||============= Storage Info ============||\n" +
            $"|| NAME: {fom.typeName}\n" +
            $"|| POS: {fom.transform.position}\n";
            foreach(var ao in fom.attachedObjectsInfo.Where(aoi => aoi.objectID is not null and not ""))
            {
              attachedObjects++;
              text += $"[{attachedObjects}] - {ao.itemName}\n";
            }
          }
          else
          {
            text = $"            || STORAGE";
            if(DrawDistance) { text += $"|| {Convert.ToInt32(Distance)}m\n"; }
          }
          GUI.color = Utils.GUIColorByDistance(Distance);
          GUI.Label(new Rect(w2s.x - 100, Screen.height - w2s.y, 300, 400), new GUIContent(text));
          GUI.color = Color.white;
        }
      }
    }
    public static void RenderNetViews()
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      foreach(var nvb in Refs.LP_NetView._network._enabledViews.Values)
      {
        Distance = Vector3.Distance(Refs.LP_Position, nvb.gameObject.transform.position);
        var metaData = nvb.gameObject.GetComponent<ObjectMetaData>();
        if(metaData != null)
        {
          var inventory = metaData.itemProperties;
          var category = inventory.category;
          if(EspItem && EspDistanceItems > Distance)
          {
            if(category == SpawnTypes.Food || metaData.itemProperties.category == SpawnTypes.Drink) { RenderFoodAndWater(nvb.gameObject, inventory, category); }
            if(category is SpawnTypes.ItemResources or SpawnTypes.ItemResourcesNonStack) { RenderItems(nvb.gameObject, inventory, category); }
            if(category == SpawnTypes.Medical) { RenderMedical(nvb.gameObject, inventory, category); }
            if(category == SpawnTypes.ToolsNonStack) { RenderTool(nvb.gameObject, inventory, category); }
          }
          if(EspWeapon && EspDistanceWeapons > Distance)
          {
            if(category is SpawnTypes.AmmoNonStack or SpawnTypes.meleeWeaponsNonStack or SpawnTypes.WeaponsNonStack) { RenderWeapons(nvb.gameObject, inventory, category); }
          }
          if(EspVehicle && EspDistanceVehicles > Distance && category == SpawnTypes.Vehicle) { RenderVehicles(nvb.gameObject, inventory); }
        }
        if(EspAnimal && EspDistanceAnimals > Distance && nvb.name.Contains("AI_") && !nvb.name.Contains("Zombie")) { RenderAnimals(nvb.gameObject); }
        if(EspPlayer && EspDistancePlayers > Distance && nvb.name.Contains("Player@Proxy(Clone)")) { RenderPlayer(nvb.gameObject); }
        if(EspZombie && EspDistanceZombies > Distance && nvb.name.Contains("AI_Zombie@Proxy(Clone)")) { RenderZombies(nvb.gameObject); }
      }
    }
    public static void RenderItems(GameObject go, InventorySlot inventory, SpawnTypes category)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      Distance = Vector3.Distance(Refs.LP_Position, go.transform.position);
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      if(w2s.z > 0)
      {
        var renderer = go.GetComponentInChildren<MeshRenderer>();
        var text = $"              {inventory.itemName_Localized}\n";
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
             $"||============== Item =============||\n" +
             $"|| NAME: {inventory.itemName_Localized}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
        }
        else
        {
          text = $"                {inventory.itemName_Localized}  --  {Convert.ToInt32(Distance)}m\n";
          GUI.Label(new Rect(w2s.x - 20, Screen.height - w2s.y, 50, 50), new GUIContent(inventory.dataStore.iTemGUI));
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 130, Screen.height - w2s.y, 400, 500), new GUIContent(text));
        GUI.color = Color.white;
        if(renderer != null && Draw3DBox) { Draw.World.Cube(go.transform.position, renderer.bounds.extents, go.transform.forward, go.transform.up); }
      }
    }
    public static void RenderAnimals(GameObject go)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      Distance = Vector3.Distance(Refs.LP_Position, go.transform.position);
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      if(w2s.z > 0)
      {
        var aiProxy = go.GetComponent<AiProxyPassive>();
        var meshRenderer = go.GetComponentInChildren<SkinnedMeshRenderer>();
        var boxCollider = go.GetComponent<BoxCollider>();
        var position = go.transform.position;
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        var text = $"            {aiProxy.aiSettings.NPCName}  --  {Convert.ToInt32(Distance)}m\n";
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
          $"                                 {Convert.ToInt32(Distance)}M\n" +
          $"||============= Animal Info ============||\n" +
          $"|| NAME: {aiProxy.aiSettings.NPCName}\n" +
          $"|| NAME: {aiProxy.spawnType}\n" +
          $"|| POS: {go.transform.position}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
        }
        else
        {
          text = $"            {aiProxy.aiSettings.NPCName}  --  {Convert.ToInt32(Distance)}m\n";
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 100, Screen.height - w2s.y, 300, 400), text);
        GUI.color = Color.white;
        //TODO:: Change to proper hit-colliders and change based on if LOD model is loaded/visible skinmeshrenderer
        if(Draw3DBox)
        {
          if(meshRenderer != null)
          {
            var bounds = meshRenderer.bounds;
            Draw.World.Cube(new Vector3(position.x, position.y + bounds.extents.y, position.z), bounds.extents, go.transform.forward, go.transform.up);
          }
          else if(meshRenderer == null && boxCollider != null)
          {
            var size = boxCollider.size;
            Draw.World.Cube(new Vector3(position.x, position.y + size.y, position.z), size, go.transform.forward, go.transform.up);
          }
        }
      }
    }
    public static bool FilterFood = false;
    public static bool FilterWater = false;
    public static void RenderFoodAndWater(GameObject go, InventorySlot inventory, SpawnTypes category)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      Distance = Vector3.Distance(Refs.LP_Position, go.transform.position);
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      if(w2s.z > 0)
      {
        var text = $"            {inventory.itemName_Localized}\n";
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
          $"||============== Item =============||\n" +
          $"|| NAME: {inventory.itemName_Localized}\n" +
          $"|| ID: {inventory.typeID}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
          if(inventory.consumable.health != 0) { text += $"|| HEALTH: {inventory.consumable.health}\n"; }
          if(inventory.consumable.stamina != 0) { text += $"|| STAMINA: {inventory.consumable.stamina}\n"; }
          if(inventory.consumable.cals != 0) { text += $"|| CALORIES: {inventory.consumable.cals}\n"; }
          if(inventory.consumable.hydration != 0) { text += $"|| HYDRATION: {inventory.consumable.hydration}\n"; }
          if(inventory.consumable.sicknessChance != 0) { text += $"|| SICKNESS%: {inventory.consumable.sicknessChance}\n"; }
        }
        else
        {
          text =
          $"             {inventory.itemName_Localized}  --  {Convert.ToInt32(Distance)}m\n";
          GUI.Label(new Rect(w2s.x - 20, Screen.height - w2s.y, 50, 50), new GUIContent(inventory.dataStore.iTemGUI));
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 130, Screen.height - w2s.y, 400, 500), new GUIContent(text));
        GUI.color = Color.white;
        var renderer = go.GetComponentInChildren<MeshRenderer>();
        if(renderer != null && Draw3DBox)
        {
          if(FilterFood && category == SpawnTypes.Food) { Draw.World.Cube(go.transform.position, renderer.bounds.size, go.transform.forward, go.transform.up); }
          if(FilterWater && category == SpawnTypes.Drink) { Draw.World.Cube(go.transform.position, renderer.bounds.size, go.transform.forward, go.transform.up); }
        }

      }
    }
    public static void RenderWeapons(GameObject go, InventorySlot inventory, SpawnTypes category)
    {
      Distance = Vector3.Distance(Refs.LP_Position, go.transform.position);
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      if(w2s.z > 0)
      {
        var renderer = go.GetComponentInChildren<MeshRenderer>();
        var text = $"              Weapon: {inventory.itemName_Localized}\n";
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
             $"||============== Weapon =============||\n" +
             $"|| NAME: {inventory.itemName_Localized}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
        }
        else
        {
          text = $"                {inventory.itemName_Localized}  --  {Convert.ToInt32(Distance)}m\n";
          GUI.Label(new Rect(w2s.x - 20, Screen.height - w2s.y, 50, 50), new GUIContent(inventory.dataStore.iTemGUI));
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 130, Screen.height - w2s.y, 400, 500), new GUIContent(text));
        GUI.color = Color.white;

        if(renderer != null && Draw3DBox) { Draw.World.Cube(go.transform.position, renderer.bounds.extents, go.transform.forward, go.transform.up); }
      }
    }
    public static void RenderTool(GameObject go, InventorySlot inventory, SpawnTypes category)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      Distance = Vector3.Distance(Refs.LP_Position, go.transform.position);
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      if(w2s.z > 0)
      {
        var renderer = go.GetComponentInChildren<MeshRenderer>();
        var text = $"              || Tool: {inventory.category}\n";
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
          $"||============== Tool =============||\n" +
          $"|| NAME: {inventory.itemName_Localized}\n" +
          $"|| ID: {inventory.typeID}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
        }
        else
        {
          text = $"                {inventory.category}  --  {Convert.ToInt32(Distance)}m\n";
          GUI.Label(new Rect(w2s.x - 20, Screen.height - w2s.y, 50, 50), new GUIContent(inventory.dataStore.iTemGUI));
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 130, Screen.height - w2s.y, 400, 500), new GUIContent(text));
        GUI.color = Color.white;
        if(renderer != null && Draw3DBox) { Draw.World.Cube(go.transform.position, renderer.bounds.size, go.transform.forward, go.transform.up); }
      }
    }
    public static void RenderMedical(GameObject go, InventorySlot inventory, SpawnTypes category)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      Distance = Vector3.Distance(Refs.LP_Position, go.transform.position);
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      if(w2s.z > 0)
      {
        var renderer = go.GetComponentInChildren<MeshRenderer>();
        var text = $"              || Medical: {inventory.category}\n";
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
          $"||============== Medical =============||\n" +
          $"|| NAME: {inventory.itemName_Localized}\n" +
          $"|| ID: {inventory.typeID}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
          if(inventory.consumable.health != 0) { text += $"|| HEALTH: {inventory.consumable.health}\n"; }
          if(inventory.consumable.stamina != 0) { text += $"|| STAMINA: {inventory.consumable.stamina}\n"; }
          if(inventory.consumable.cals != 0) { text += $"|| CALORIES: {inventory.consumable.cals}\n"; }
          if(inventory.consumable.hydration != 0) { text += $"|| HYDRATION: {inventory.consumable.hydration}\n"; }
          if(inventory.consumable.sicknessChance != 0) { text += $"|| SICKNESS%: {inventory.consumable.sicknessChance}\n"; }
        }
        else
        {
          text = $"                {inventory.category}  --  {Convert.ToInt32(Distance)}m\n";
          GUI.Label(new Rect(w2s.x - 20, Screen.height - w2s.y, 50, 50), new GUIContent(inventory.dataStore.iTemGUI));
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 130, Screen.height - w2s.y, 400, 500), new GUIContent(text));
        GUI.color = Color.white;
        if(renderer != null && Draw3DBox) { Draw.World.Cube(go.transform.position, renderer.bounds.size, go.transform.forward, go.transform.up); }
      }
    }
    public static void RenderPlayer(GameObject go)
    {
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      Distance = Vector3.Distance(Refs.LP_GO.transform.position, go.transform.position);
      if(Camera.main == null) return;
      var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
      var proxy = go.GetComponent<PlayerProxy>();
      var user = proxy.userOnClient;
      if(user.isMe || !(w2s.z > 0)) return;
      var text =
        $"                                 {Convert.ToInt32(Distance)}m\n" +
        $"||============== Player Info =============||\n" +
        $"|| NAME: {user.lastKnowPersonalName}\n" +
        $"|| Friend: {proxy.isFriend}\n" +
        $"|| SID: {user.steam64ID}\n" +
        $"|| POS: {go.transform.position}\n";
      if(Distance < 180)
      {
        var weapons = proxy._proxyWeapons;
        if(weapons != null && weapons.currentSpawnedWeapon != null)
        {
          var attachmentRoot = weapons.currentSpawnedWeapon.transform.Find("Attachments");
          var attachmentsList = $"";
          var attachName = $"";
          var weaponName = weapons.currentSpawnedWeapon.name.Replace("Proxy_", "");
          if(weaponName.Contains("_LH")) { _ = weaponName.Replace("_LH", ""); }
          if(weaponName.Contains("_RH")) { _ = weaponName.Replace("_RH", ""); }
          text +=
            $"||============= Weapon Info ==============||\n" +
            $"|| TYPE: {weaponName}\n";
          //TODO:: FIX ATTACHMENTS CHECK ON WEAPONS, PRETTY SURE ITS MELEE HAVING NO ATTACHMENTS OR SOMETHING JUST GOTTA MAKE A CHECK
          if(attachmentRoot != null)
          {
            foreach(var gso in attachmentRoot.gameObject.GetComponentsInChildren<Transform>().Where(t => t.gameObject.activeSelf == true))
            {
              attachName = gso.name.Replace("Drop_", "");
              if(attachName.Contains("Attachment")) { _ = attachName.Replace("Attachment", ""); }
              if(attachName.EndsWith("_LH") || attachName.EndsWith("_RH")) { _ = attachName.Remove(attachName.Length - 4, 3); }
              if(attachName.Contains("_")) { _ = attachName.Replace("_", " "); }
              attachmentsList += attachName + ":";
            }
            text += $"|| SLOTS: {attachmentsList}\n";
          }
        }
      }
      var vehicle = user.currentVehicle;
      if(vehicle != null)
      {
        var vehicleName = vehicle.name.Replace("Drop_Vehicle", "");
        vehicleName = vehicleName.Replace("(Clone)", "");
        text +=
                   $"||============= Vehicle Info =============||\n" +
                   $"|| TYPE: {vehicleName}\n" +
                   $"|| SEATS: {vehicle.seatsHolder.seats.Count}\n" +
                   $"|| DRIVER: {user.isInVehicleAsDriver}\n";
      }
      GUI.color = Utils.GUIColorByDistance(Distance);
      GUI.Label(new Rect(w2s.x - 130, Screen.height - w2s.y, 400, 500), text);
      GUI.color = Color.white;
    }
    public static void RenderVehicles(GameObject go, InventorySlot inventory)
    {

      Distance = Vector3.Distance(Refs.LP_GO.transform.position, go.transform.position);
      if(Camera.main != null)
      {
        if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
        var w2s = Camera.main.WorldToScreenPoint(go.transform.position);
        if(!(w2s.z > 0)) return;
        var vehicle = go.GetComponent<VehicleClient>();
        var occupiedSeats = vehicle.seatsHolder.seats.Count(v => v.seatOccupied);
        var text = $"            || VEHICLE";
        if(DrawDistance) { text += $"|| {Convert.ToInt32(Distance)}m\n"; }
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        text = sdist < 100
          ? $"||============= Vehicle =============||\n" +
         $"|| TYPE: {inventory.itemName_Localized}\n" +
         $"|| SEATS: {vehicle.seatsHolder.seats.Count}\n" +
         $"|| SEATS OCCUPIED: {occupiedSeats} {vehicle.seatsHolder.seats.Count}\n" +
         $"|| ID: {inventory.typeID}\n"
          : $"            || VEHICLE  --  {Convert.ToInt32(Distance)}m";
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 100, Screen.height - w2s.y, 400, 500), text);
        GUI.color = Color.white;
        MeshCollider meshCollider = null;
        if(meshCollider == null)
        {
          foreach(var t in go.GetComponentsInChildren<Transform>().Where(e => e.name.Contains("Root")))
          {
            meshCollider = t.Find("VehicleCollisionTrigger").gameObject.GetComponent<MeshCollider>();
            break;
          }
        }
        else if(meshCollider != null)
        {
          if(Draw3DBox) { Draw.World.Cube(go.transform.position, go.GetComponent<MeshCollider>().bounds.size, go.transform.forward, go.transform.up); }
        }
      }
    }
    public static void RenderZombies(GameObject go)
    {
      var trans = go.transform;
      var pos = trans.position;
      Distance = Vector3.Distance(Refs.LP_Position, pos);
      if(Camera.main == null) return;
      if(Camera.main.GetComponent<Drawer>() == null) { _ = Camera.main.gameObject.AddComponent<Drawer>(); }
      var w2s = Camera.main.WorldToScreenPoint(pos);
      var aiProxy = go.GetComponent<AiProxy>();
      var renderer = go.GetComponentInChildren<SkinnedMeshRenderer>();
      if(w2s.z > 0)
      {
        var sdist = Vector2.Distance(w2s, new Vector3(Screen.width / 2, Screen.height / 2, 0));
        string text;
        if(sdist < AdvancedOptionsRadius && Distance < AdvancedOptionsDistance)
        {
          text =
           $"||============= Zombie ============||\n" +
           $"|| NAME: {aiProxy.aiSettings.NPCName}\n" +
           $"|| POS: {pos}\n";
          if(DrawDistance) { text += $"|| DISTANCE: {Convert.ToInt32(Distance)}m\n"; }
        }
        else
        {
          text = $"             || ZOMBIE  --  {Convert.ToInt32(Distance)}m\n";
        }
        GUI.color = Utils.GUIColorByDistance(Distance);
        GUI.Label(new Rect(w2s.x - 100, Screen.height - w2s.y, 400, 500), text);
        GUI.color = Color.white;
        if(renderer != null && Draw3DBox) { Draw.World.Cube(new Vector3(pos.x, renderer.bounds.center.y, pos.z), renderer.bounds.size, trans.forward, trans.up); }
      }
    }

  }
}