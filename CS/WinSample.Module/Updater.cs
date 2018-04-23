using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSample.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Category category = CreateCategory("Parent", null);
            CreateCategory("Child1", category);
            CreateCategory("Child2", category);
        }
        private Category CreateCategory(string name, HCategory parent) {
            Category category = Session.FindObject<Category>(new BinaryOperator("Name", name));
            if (category == null) {
                category = new Category(Session);
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
            Item item = Session.FindObject<Item>(new BinaryOperator("Name", realName));
            if (item == null) {
                item = new Item(Session);
                item.Name = realName;
                item.Category = category;
                item.Save();
            }
        }
    }
}
