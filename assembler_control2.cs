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

      foreach (var goal_item in goal) {
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
