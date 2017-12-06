// list_item_ids
public void Main(string argument) {
  using (var utils = new Utils(this)) {
    try {
      foreach (var entry in utils.AllItemCounts()) {
        utils.Print(string.Format("{0} x {1}", Utils.FormatNumber(entry.Value), entry.Key));
      }
    } catch (Exception e) {
      utils.Print(e);
    }
  }
}
