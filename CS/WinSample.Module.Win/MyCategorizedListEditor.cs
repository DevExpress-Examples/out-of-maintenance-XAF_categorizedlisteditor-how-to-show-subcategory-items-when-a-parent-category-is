using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base.General;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSample.Module.Win {
    public class MyCategorizedListEditor : CategorizedListEditor {
        public MyCategorizedListEditor(DictionaryNode info) : base(info) { }
        protected override object CreateControlsCore() {
            object result = base.CreateControlsCore();
            CategoriesListView.SelectionChanged += new EventHandler(CategoriesListView_SelectionChanged);
            return result;
        }
        void CategoriesListView_SelectionChanged(object sender, EventArgs e) {
             UpdateGridViewFilter();
        }
        private void UpdateGridViewFilter() {
            if (CategoriesListView.CurrentObject != null) {
                List<Guid> categories = new List<Guid>();
                BaseObject currentCategory = (BaseObject)CategoriesListView.CurrentObject;
                categories.Add(currentCategory.Oid);
                GetCategories((ITreeNode)CategoriesListView.CurrentObject, categories);
                this.ItemsDataSource.Criteria[CategoryPropertyName] = new InOperator("Category.Oid", categories);
                RefreshColumns();
            }
        }
        private void GetCategories(ITreeNode current, IList<Guid> childrenCategoryIDs) {
            foreach (ITreeNode child in current.Children) {
                BaseObject categorizedItem = child as BaseObject;
                if (categorizedItem != null) {
                    childrenCategoryIDs.Add(categorizedItem.Oid);
                    GetCategories(child, childrenCategoryIDs);
                }
            }
        }
        public override void Dispose() {
            this.CategoriesListView.CurrentObjectChanged -= new EventHandler(CategoriesListView_SelectionChanged);
            base.Dispose();
        }
    }
}