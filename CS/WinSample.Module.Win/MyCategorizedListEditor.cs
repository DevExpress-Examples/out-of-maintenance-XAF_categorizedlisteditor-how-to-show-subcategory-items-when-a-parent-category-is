using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base.General;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Model;

namespace WinSample.Module.Win {
    public class MyCategorizedListEditor : CategorizedListEditor {
        public MyCategorizedListEditor(IModelListView info) : base(info) { }
        protected override object CreateControlsCore() {
            object result = base.CreateControlsCore();
            CategoriesListView.SelectionChanged += new EventHandler(CategoriesListView_SelectionChanged);
            ObjectTreeList objectTreeList = CategoriesListView.Editor.Control as ObjectTreeList;
            if (objectTreeList != null) {
                objectTreeList.NodesReloading += new EventHandler(objectTreeList_NodesReloading);
                objectTreeList.NodesReloaded += new EventHandler(objectTreeList_NodesReloaded);
            }
            locker.LockedChanged += new EventHandler<LockedChangedEventArgs>(locker_LockedChanged);
            return result;
        }
        private Locker locker = new Locker();
        void objectTreeList_NodesReloaded(object sender, EventArgs e) {
            locker.Unlock();
        }

        void objectTreeList_NodesReloading(object sender, EventArgs e) {
            locker.Lock();
        }

        void CategoriesListView_SelectionChanged(object sender, EventArgs e) {
            if (!locker.Locked) {
                UpdateGridViewFilter();
            } else {
                locker.Call("UpdateGridViewFilter");
            }
        }

        void locker_LockedChanged(object sender, LockedChangedEventArgs e) {
            if (!e.Locked && e.PendingCalls.Contains("UpdateGridViewFilter")) {
                UpdateGridViewFilter();
            }

        }
        private void UpdateGridViewFilter() {
            if (CategoriesListView.CurrentObject != null) {
                List<Guid> categories = new List<Guid>();
                BaseObject currentCategory = (BaseObject)CategoriesListView.CurrentObject;
                categories.Add(currentCategory.Oid);
                GetCategories((ITreeNode)CategoriesListView.CurrentObject, categories);
                this.ItemsDataSource.Criteria[CategoryPropertyName] = new InOperator("Category.Oid", categories);
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
            locker.LockedChanged -= new EventHandler<LockedChangedEventArgs>(locker_LockedChanged);
            base.Dispose();
        }
    }
}
