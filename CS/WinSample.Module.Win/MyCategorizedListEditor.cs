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
using System.Collections;
using DevExpress.ExpressApp.Editors;

namespace WinSample.Module.Win {
    [ListEditor(typeof(ICategorizedItem))]
    public class MyCategorizedListEditor : CategorizedListEditor {
        public MyCategorizedListEditor(IModelListView info) : base(info) { }
        protected override void OnControlsCreated() {
            base.OnControlsCreated();
            CategoriesListView.SelectionChanged += new EventHandler(CategoriesListView_SelectionChanged);
            ObjectTreeList objectTreeList = CategoriesListView.Editor.Control as ObjectTreeList;
            if (objectTreeList != null) {
                objectTreeList.NodesReloading += new EventHandler(objectTreeList_NodesReloading);
                objectTreeList.NodesReloaded += new EventHandler(objectTreeList_NodesReloaded);
            }
            locker.LockedChanged += new EventHandler<LockedChangedEventArgs>(locker_LockedChanged);
        }
        private Locker locker = new Locker();
        void objectTreeList_NodesReloaded(object sender, EventArgs e) {
            locker.Unlock();
        }

        void objectTreeList_NodesReloading(object sender, EventArgs e) {
            locker.Lock();
        }

        void CategoriesListView_SelectionChanged(object sender, EventArgs e) {
            UpdateGridViewFilter();
        }

        void locker_LockedChanged(object sender, LockedChangedEventArgs e) {
            if (!e.Locked && e.PendingCalls.Contains("UpdateGridViewFilter")) {
                UpdateGridViewFilter();
            }
        }
        private void UpdateGridViewFilter() {
            locker.Call("UpdateGridViewFilter");
            if (!locker.Locked) {
                if (CategoriesListView.CurrentObject != null) {
                    ArrayList categoryKeys = new ArrayList();
                    ITreeNode currentCategory = (ITreeNode)CategoriesListView.CurrentObject;
                    categoryKeys.Add(GetCategoryKey(currentCategory));
                    AddChildrenKeys(currentCategory, categoryKeys);
                    string categoryKeyPropertyName = String.Format("{0}.{1}", CategoryPropertyName, CategoriesListView.ObjectTypeInfo.KeyMember.Name);
                    this.ItemsDataSource.Criteria[CategoryPropertyName] = new InOperator(categoryKeyPropertyName, categoryKeys);
                }
            }
        }
        private void AddChildrenKeys(ITreeNode current, IList categoryKeys) {
            foreach (ITreeNode child in current.Children) {
                categoryKeys.Add(GetCategoryKey(child));
                AddChildrenKeys(child, categoryKeys);
            }
        }
        private object GetCategoryKey(object category) {
            return CategoriesListView.ObjectSpace.GetKeyValue(category);
        }
        public override void Dispose() {
            this.CategoriesListView.CurrentObjectChanged -= new EventHandler(CategoriesListView_SelectionChanged);
            locker.LockedChanged -= new EventHandler<LockedChangedEventArgs>(locker_LockedChanged);
            base.Dispose();
        }
    }
}
