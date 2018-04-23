using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSample.Module {
    public class Updater : ModuleUpdater {
        public Updater(DevExpress.ExpressApp.IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Category category = CreateCategory("Parent", null);
            CreateCategory("Child1", category);
            CreateCategory("Child2", category);
        }
        private Category CreateCategory(string name, HCategory parent) {
            Category category = ObjectSpace.FindObject<Category>(new BinaryOperator("Name", name));
            if (category == null) {
                category = ObjectSpace.CreateObject<Category>();
                category.Name = name;
                category.Parent = parent;
                category.Save();
                CreateCategorizedItem("Item1", category);
                CreateCategorizedItem("Item2", category);
            }
            return category;
        }
        private void CreateCategorizedItem(string name, Category category) {
            string realName = name + " - " + category.Name;
            Item item = ObjectSpace.FindObject<Item>(new BinaryOperator("Name", realName));
            if (item == null) {
                item = ObjectSpace.CreateObject<Item>();
                item.Name = realName;
                item.Category = category;
                item.Save();
            }
        }
    }
}
