public class Utils : IDisposable {

  List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
  IMyProgrammableBlock thisProgrammableBlock;




  int screenRows = 19;
  int screenColumnsNormal = 25;
  int screenColumnsWide = 55;


  List<IMyTextPanel> screens = new List<IMyTextPanel>();
  bool screenIsRaw = false;
  bool screenIsWide = false;

  bool hadException = false;

  string outputText = "";
  static int launchCounter = 0;
  static string launchCounterSpinner = "|/-\\";

  public int ScreenColumns {
    get {
      return screenIsWide ? screenColumnsWide : screenColumnsNormal;
    }
  }
  public int ScreenRows {
    get {
      return screenRows * screens.Count;
    }
  }

  public Utils(MyGridProgram program) {
    launchCounter++;
    //print(string.Format("Started at {0}", System.DateTime.Now));
    program.GridTerminalSystem.GetBlocks(blocks);
    foreach (var block in blocks) {
      var programmableBlock = block as IMyProgrammableBlock;
      if (programmableBlock != null && programmableBlock.IsRunning) {
        thisProgrammableBlock = programmableBlock;
        break;
      }
    }
    if (thisProgrammableBlock == null) {
      printHeader("Unknown");
      Print("failed to find this programmable block");
    } else {
      printHeader(thisProgrammableBlock.CustomName);
      var screenName = "Screen-" + thisProgrammableBlock.CustomName.Substring(3);
      int i = 0;
      while (true) {
        var panel1 = FindBlockIfExists<IMyTextPanel>(string.Format("{0}-{1}", screenName, i), FilterGrid.Current);
        if (panel1 != null) {
          screens.Add(panel1);
        } else {
          panel1 = FindBlockIfExists<IMyTextPanel>(string.Format("{0}-{1}w", screenName, i), FilterGrid.Current);
          if (panel1 != null) {
            screens.Add(panel1);
            screenIsWide = true;
          } else {
            panel1 = FindBlockIfExists<IMyTextPanel>(string.Format("{0}-{1}r", screenName, i), FilterGrid.Current);
            if (panel1 != null) {
              screens.Add(panel1);
              screenIsRaw = true;
            } else {
              break;
            }
          }
        }
        i++;
      }
      if (screens.Count == 0) {
        Print(string.Format("failed to find screen with prefix \"{0}\"", screenName));
      }
      setupScreen();
    }

  }

  void printHeader(string title) {
    Print(string.Format(" ({1}) {0}", title, launchCounterSpinner[launchCounter % 4]));
  }

  void setupScreen() {
    foreach (var screen in screens) {
      screen.ShowPublicTextOnScreen();
      screen.FontSize = 0.928f;
    }
  }

  public void Print(object obj) {
    var text = obj == null ? "null" : obj.ToString();
    outputText += text;
    outputText += "\n";
  }

  public void Print(Exception exception) {
    outputText += exception.ToString();
    outputText += "\n";
    hadException = true;
  }

  void applyOutput() {
    if (screens.Count > 0) {
      if (screenIsRaw) {
        screens[0].WritePublicText(outputText);
      } else {
        var lines = WordWrap(outputText, ScreenColumns);

        for (int i = 0; i < screens.Count; i++) {
          var sb = new StringBuilder();
          for (int line = i * screenRows; line < (i + 1) * screenRows && line < lines.Count; line++) {
            sb.Append(" ");
            sb.Append(lines[line]);
            sb.Append(" ");
            sb.Append("\n");
          }
          screens[i].WritePublicText(sb.ToString());
        }
      }
      //throw new Exception(string.Format(
      //  "\n\nSUCCESS\nOutput displayed using {0} at {1}", screen.CustomName, System.DateTime.Now));

    } else {
      throw new Exception("\n\n" + outputText);
    }
  }

  public void Dispose() {
    if (!hadException) {
      Print("Done");
    }
    applyOutput();
  }
  /*
  float ItemVolume(IMyInventoryItem parItem) {
    float varResult = 0;

    const float varOre = 0.37f;

    const float varIngotCobalt = 0.112f;
    const float varIngotGold = 0.052f;
    const float varIngotGravel = 0.37f;
    const float varIngotIron = 0.127f;
    const float varIngotMagnesium = 0.575f;
    const float varIngotNickel = 0.112f;
    const float varIngotPlatinum = 0.047f;
    const float varIngotSilicon = 0.429f;
    const float varIngotSilver = 0.095f;
    const float varIngotUranium = 0.052f;

    const float varRifle = 14f;
    const float varGrinder = 20f;
    const float varHandDrill = 120f;
    const float varWelder = 8f;
    const float varAmmoMagazine = 0.20f;
    const float varAmmoContainer = 16f;
    const float varAmmoMissile = 60f;

    const float varBulletproofGlass = 8f;
    const float varComputer = 1f;
    const float varContructionComponent = 2f;
    const float varDetectorComponent = 6f;
    const float varDisplay = 6f;
    const float varExplosives = 2f;
    const float varGirder = 2f;
    const float varGravityComponent = 200f;
    const float varInteriorPlate = 5f;
    const float varMedicalComponent = 160f;
    const float varMetalGrid = 15f;
    const float varMotor = 8f;
    const float varPowerCell = 45f;
    const float varRadioComponent = 140f;
    const float varReactorComponent = 8f;
    const float varSteelPlate = 3f;
    const float varSteelTubeLarge = 38f;
    const float varSteelTubeSmall = 2f;
    const float varSolarCell = 50f;
    const float varThrusterComponent = 10f;

    if (parItem.Content.ToString().Contains("Ore")) {
      varResult = varOre;
    }

    if (parItem.Content.ToString().Contains("PhysicalGunObject")) {
      if (parItem.Content.SubtypeName == "AutomaticRifleItem")
        varResult = varRifle;
      else if (parItem.Content.SubtypeName == "AngleGrinderItem")
        varResult = varGrinder;
      else if (parItem.Content.SubtypeName == "HandDrillItem")
        varResult = varHandDrill;
      else if (parItem.Content.SubtypeName == "WelderItem")
        varResult = varWelder;
    }

    if (parItem.Content.ToString().Contains("Component")) {
      if (parItem.Content.SubtypeName == "BulletproofGlass")
        varResult = varBulletproofGlass;
      else if (parItem.Content.SubtypeName == "Construction")
        varResult = varContructionComponent;
      else if (parItem.Content.SubtypeName == "Girder")
        varResult = varGirder;
      else if (parItem.Content.SubtypeName == "MetalGrid")
        varResult = varMetalGrid;
      else if (parItem.Content.SubtypeName == "SmallTube")
        varResult = varSteelTubeSmall;
    }
    return varResult;
  }
  */

  static int BreakLine(string text, int pos, int max) {
    // Find last whitespace in line
    int i = max - 1;
    while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
      i--;
    if (i < 0)
      return max; // No whitespace found; break at maximum length
    // Find start of whitespace
    while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
      i--;
    // Return length of text before whitespace
    return i + 1;
  }

  static List<string> WordWrap(string the_string, int width) {
    int pos, next;
    var result = new List<string>();
    //StringBuilder sb = new StringBuilder();

    // Lucidity check
    if (width < 1) {
      result.Add(the_string);
      return result;
    }

    // Parse each line of text
    for (pos = 0; pos < the_string.Length; pos = next) {
      // Find end of line
      int eol = the_string.IndexOf("\n", pos);

      if (eol == -1)
        next = eol = the_string.Length;
      else
        next = eol + "\n".Length;

      // Copy this line of text, breaking into smaller lines as needed
      if (eol > pos) {
        do {
          int len = eol - pos;

          if (len > width)
            len = BreakLine(the_string, pos, width);
          result.Add(the_string.Substring(pos, len));
          //sb.Append(the_string, pos, len);
          //sb.Append("\n");

          // Trim whitespace following break
          pos += len;

          while (pos < eol && Char.IsWhiteSpace(the_string[pos]))
            pos++;

        } while (eol > pos);
      } else {
        result.Add("");
        //sb.Append("\n"); // Empty line
      }
    }

    return result; // sb.ToString();
  }


  public enum FilterGrid {
    None,
    Current,
  }

  private bool checkFilterGrid(FilterGrid filter, IMyTerminalBlock block) {
    if (filter == FilterGrid.Current) {
      if (thisProgrammableBlock == null || thisProgrammableBlock.CubeGrid != block.CubeGrid) {
        return false;
      }
    }
    return true;
  }

  public List<IMyInventory> Inventories(FilterGrid filter = FilterGrid.None) {
    var result = new List<IMyInventory>();
    foreach (var block in blocks) {
      if (!checkFilterGrid(filter, block)) { continue; }
      for(int i = 0; i < block.InventoryCount; i++) {
        var inventory = block.GetInventory(i);
        if (inventory != null) {
          result.Add(inventory);
        }
      }
    }
    return result;
  }

  public IMyInventory FindInventory(string name, FilterGrid filter = FilterGrid.None) {
    var block = FindBlock<IMyTerminalBlock>(name, filter);
    var inventory = block.GetInventory();
    if (inventory != null) {
      return inventory;
    } else {
      throw new Exception(string.Format("no inventory in block \"{0}\"", name));
    }
  }

  /*
    Ingot/Uranium,
    AmmoMagazine/NATO_5p56x45mm,
    PhysicalGunObject/AutomaticRifleItem,
    PhysicalGunObject/AngleGrinderItem,
    PhysicalGunObject/HandDrillItem,
    PhysicalGunObject/WelderItem,
    Ingot/Iron, Ingot/Platinum,
    Ore/Cobalt, Ore/Silver, Ore/Gold,
    Ingot/Gold, Ingot/Silver,
    Ingot/Magnesium, Ore/Iron, Ore/Ice,
    Component/Girder,
    Component/Superconductor,
    Component/Construction,
    Component/BulletproofGlass,
    Component/LargeTube,
    Component/GravityGenerator,
    Component/Medical,
    Component/RadioCommunication,
    Component/Detector,
    Component/Explosives,
    Component/SolarCell,
    Component/SteelPlate,
    Component/MetalGrid,
    Component/Reactor,
    Component/InteriorPlate,
    Component/Computer,
    Component/Display,
    Component/SmallTube,
    Component/Motor,
    Component/PowerCell, Ingot/Nickel,
    Ingot/Silicon, Ingot/Stone,
    Ingot/Cobalt,
    OxygenContainerObject/OxygenBottle,
    GasContainerObject/HydrogenBottle,
    Component/Thrust,
    AmmoMagazine/NATO_25x184mm
   */
  public string ItemId(IMyInventoryItem item) {
    return item.Content.TypeId.ToString().Replace("MyObjectBuilder_", "") +
      "/" + item.Content.SubtypeName;
  }

  bool tryAddAssemblerTask(IMyProductionBlock assembler, string definition, VRage.MyFixedPoint count) {
    try {
      MyDefinitionId objectIdToAdd = new MyDefinitionId();
      MyDefinitionId.TryParse(definition, out objectIdToAdd);
      assembler.AddQueueItem(objectIdToAdd, count);
      return true;
    } catch(Exception e) {
      return false;
    }
  }

  public bool AddAssemblerTask(IMyProductionBlock assembler, string itemId, VRage.MyFixedPoint count) {
    
    var fixedItemId = "MyObjectBuilder_BlueprintDefinition/" + itemId.Split('/')[1];
    if (fixedItemId.EndsWith("Item")) {
      fixedItemId = fixedItemId.Substring(0, fixedItemId.Count() - 4);
    }
    if (!tryAddAssemblerTask(assembler, fixedItemId, count)) {
      var fixedItemId2 = fixedItemId + "Component";
      if (!tryAddAssemblerTask(assembler, fixedItemId2, count)) {
        Print(string.Format("AddQueueItem failed for {0}", itemId));
        Print(string.Format("tried: {0}, {1}", fixedItemId, fixedItemId2));
        return false;
      }
    }
    //Print(string.Format("queued: {0} x {1}", itemId, count));
    return true;
  }

  public T FindBlock<T>(string name, FilterGrid filter = FilterGrid.None) where T : class {
    foreach (var block in blocks) {
      if (!checkFilterGrid(filter, block)) { continue; }
      if (block.CustomName == name) {
        var result = block as T;
        if (result == null) {
          throw new Exception("failed to cast block \"{0}\"");
        } else {
          return result;
        }
      }
    }
    throw new Exception(string.Format("failed to find block \"{0}\"", name));
  }

  public T FindBlockIfExists<T>(string name, FilterGrid filter = FilterGrid.None) where T : class {
    try {
      return FindBlock<T>(name, filter);
    } catch (Exception e) {
      return null;
    }
  }

  private Dictionary<string, VRage.MyFixedPoint> itemCounts;

  void calculateItemCounts() {
    if (itemCounts != null) {
      return;
    }
    itemCounts = new Dictionary<string, VRage.MyFixedPoint>();
    foreach (var inventory in Inventories(FilterGrid.Current)) {
      foreach (var item in inventory.GetItems()) {
        string id = ItemId(item);
        if (!itemCounts.ContainsKey(id)) { itemCounts[id] = 0; }
        itemCounts[id] += item.Amount;
      }
    }
  }
  public Dictionary<string, VRage.MyFixedPoint> AllItemCounts() {
    calculateItemCounts();
    return itemCounts;
  }
  public VRage.MyFixedPoint ItemCount(string id) {
    calculateItemCounts();
    if (itemCounts.ContainsKey(id)) {
      return itemCounts[id];
    }
    return 0;
  }
  public static string FormatNumber(int num) {
    if (num >= 1000000)
      return (num / 1000000d).ToString("0.###") + "M";
    if (num >= 1000)
      return (num / 1000d).ToString("0.###") + "K";
    return num.ToString("#,0");
  }
  public static string FormatNumber(VRage.MyFixedPoint num) {
    return FormatNumber((int)num);
  }

}
  //... end of Utils ...................................

// assembler_control2
public void Main(string argument) {
  using (var utils = new Utils(this)) {
    try {
      /*var test1 = utils.FindBlock<IMyProductionBlock>("Refinery28");
      utils.Print(test1.GetInventory());
      utils.Print(test1.InventoryCount);
      utils.Print(test1.GetInventory(0));
      utils.Print(test1.GetInventory(1));*/

      var autoAssembler = utils.FindBlock<IMyProductionBlock>("AutoAssembler");
      var autoAssemblerBusy = !autoAssembler.IsQueueEmpty;
      if (autoAssemblerBusy) {
        utils.Print("AutoAssembler is busy");
      } else {
        utils.Print("AutoAssembler is ready");
      }      

      Dictionary<string, int> goal = new Dictionary<string, int>();
      goal["Component/SteelPlate"] = 30000;
      goal["Component/InteriorPlate"] = 1000;
      goal["Component/Construction"] = 5000;
      goal["Component/Motor"] = 1000;
      goal["Component/Computer"] = 1000;
      goal["Component/MetalGrid"] = 100;
      goal["Component/SmallTube"] = 3000;
      goal["Component/LargeTube"] = 2000;
      goal["Component/Display"] = 1000;
      goal["Component/PowerCell"] = 100;

      goal["Component/Girder"] = 100;
      goal["Component/BulletproofGlass"] = 100;
      goal["Component/Reactor"] = 100;
      goal["Component/Thrust"] = 1000;
      goal["Component/GravityGenerator"] = 100;
      goal["Component/Medical"] = 10;
      goal["Component/RadioCommunication"] = 10;
      goal["Component/Detector"] = 100;
      goal["Component/Explosives"] = 100;
      goal["Component/SolarCell"] = 100;
      goal["Component/Superconductor"] = 100;

      foreach(var goal_item in goal) {
        var real_count_item = utils.ItemCount(goal_item.Key);
        var marker = real_count_item < goal_item.Value ? "-" : "+";
        utils.Print(string.Format(
          "{0} {1,-20}: {2,6} / {3,6}", marker, 
          goal_item.Key.Replace("Component/", ""), 
          real_count_item, goal_item.Value));
        if (!autoAssemblerBusy && real_count_item < goal_item.Value) {
          utils.AddAssemblerTask(autoAssembler, goal_item.Key, goal_item.Value - real_count_item);
        }
      }

      List<string> badThings = new List<string>() {
        "PhysicalGunObject/AutomaticRifleItem",
        "PhysicalGunObject/AngleGrinderItem",
        "PhysicalGunObject/HandDrillItem",
        "PhysicalGunObject/WelderItem",
      };
      utils.Print("");
      var autoDisassembler = utils.FindBlock<IMyProductionBlock>("AutoDisassembler");
      if (autoDisassembler.IsQueueEmpty) {
        foreach (var id in badThings) {
          var count = utils.ItemCount(id);
          if (count > 1) {
            if (utils.AddAssemblerTask(autoDisassembler, id, count - 1)) {
              utils.Print(string.Format("disassembling queued: {0} x {1}", count - 1, id));
            }
          }
        }
      } else {
        utils.Print("AutoDisassembler is busy");
      }
      
    } catch (Exception e) {
      utils.Print(e);
    }
  }
}
